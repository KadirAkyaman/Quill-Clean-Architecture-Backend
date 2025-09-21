using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quill.Application.DTOs.Subscription;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Services;
using Quill.Domain.Entities;

namespace Quill.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<SubscriptionDto>> GetSubscribersAsync(int subscribedToId, CancellationToken cancellationToken)
        {
            var subscribers = await _unitOfWork.SubscriptionRepository.GetSubscribersBySubscribedToIdAsync(subscribedToId, cancellationToken);

            return _mapper.Map<IReadOnlyList<SubscriptionDto>>(subscribers);
        }

        public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(int subscriberId, CancellationToken cancellationToken)
        {
            var subscriptions = await _unitOfWork.SubscriptionRepository.GetSubscriptionsBySubscriberIdAsync(subscriberId, cancellationToken);

            return _mapper.Map<IReadOnlyList<SubscriptionDto>>(subscriptions);
        }

        public async Task<SubscriptionDto> SubscribeAsync(int subscriberId, SubscriptionCreateDto subscriptionCreateDto, CancellationToken cancellationToken)
        {
            if (subscriberId == subscriptionCreateDto.SubscribedToId)
            {
                throw new InvalidSubscriptionOperationException("Users cannot subscribe to themselves.");
            }

            var existingSubscription  = await _unitOfWork.SubscriptionRepository.FindSubscriptionAsync(subscriberId, subscriptionCreateDto.SubscribedToId, cancellationToken);

            Subscription subscriptionToReturn;

            if (existingSubscription is null)                                                                                             // If the user has never subscribed to this user before
            {
                var newSubscription = _mapper.Map<Subscription>(subscriptionCreateDto);
                newSubscription.SubscriberId = subscriberId;

                await _unitOfWork.SubscriptionRepository.AddAsync(newSubscription, cancellationToken);
                subscriptionToReturn = newSubscription;
            }
            else
            {
                if (existingSubscription.IsActive)                                                                                        // If this user is already an active subscriber, it throws an exception
                {
                    throw new AlreadySubscribedException("Already subscribed to this user.");
                }
                                                                                                                                          // If the user has never subscribed to the other user
                existingSubscription.IsActive = true;

                _unitOfWork.SubscriptionRepository.Update(existingSubscription);
                subscriptionToReturn = existingSubscription;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<SubscriptionDto>(subscriptionToReturn);
        }

        public async Task UnsubscribeAsync(int subscriberId, int subscribedToId, CancellationToken cancellationToken)
        {
            if (subscriberId == subscribedToId)
            {
                throw new InvalidSubscriptionOperationException("Users cannot unsubscribe from themselves.");
            }

            var existingSubscription = await _unitOfWork.SubscriptionRepository.FindSubscriptionAsync(subscriberId, subscribedToId, cancellationToken);

            if (existingSubscription is null || !existingSubscription.IsActive)
            {
                return;
            }

            existingSubscription.IsActive = false;
            _unitOfWork.SubscriptionRepository.Update(existingSubscription);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}