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

            var existingSubscription = await _unitOfWork.SubscriptionRepository
                .FindSubscriptionAsync(subscriberId, subscriptionCreateDto.SubscribedToId, cancellationToken);

            Subscription subscriptionToReturn;

            if (existingSubscription is null)
            {
                var newSubscription = _mapper.Map<Subscription>(subscriptionCreateDto);
                newSubscription.SubscriberId = subscriberId;
                newSubscription.CreatedAt = DateTime.UtcNow;
                newSubscription.IsActive = true;

                await _unitOfWork.SubscriptionRepository.AddAsync(newSubscription, cancellationToken);
                subscriptionToReturn = newSubscription;
            }
            else
            {
                if (existingSubscription.IsActive)
                {
                    throw new AlreadySubscribedException("Already subscribed to this user.");
                }

                existingSubscription.IsActive = true;
                existingSubscription.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.SubscriptionRepository.Update(existingSubscription);
                subscriptionToReturn = existingSubscription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var fullSubscription = await _unitOfWork.SubscriptionRepository
                .GetByIdWithDetailsAsync(subscriptionToReturn.Id, cancellationToken);

            return _mapper.Map<SubscriptionDto>(fullSubscription);
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
            existingSubscription.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.SubscriptionRepository.Update(existingSubscription);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}