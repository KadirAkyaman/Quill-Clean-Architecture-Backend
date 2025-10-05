using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Quill.Application.DTOs.Subscription;
using Quill.Application.Exceptions;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Services;
using Quill.Domain.Entities;
using Xunit;

namespace Quill.Application.UnitTests.Services
{
    public class SubscriptionServiceTests
    {
        private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly SubscriptionService _subscriptionService;

        public SubscriptionServiceTests()
        {
            _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.SubscriptionRepository).Returns(_subscriptionRepositoryMock.Object);

            _subscriptionService = new SubscriptionService
            (
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        // ---SUBSCRIBE---------------------------------------------------------------------------------------------------
        [Fact]
        public async Task SubscribeAsync_WhenNotAlreadySubscribed_ShouldCreateSubscription()
        {
            // Given
            var subscriberId = 1;
            var subscribedToId = 2;

            var subscriptionCreateDto = new SubscriptionCreateDto
            {
                SubscribedToId = subscribedToId
            };

            _subscriptionRepositoryMock.Setup(repo => repo.FindSubscriptionAsync(subscriberId, subscribedToId, It.IsAny<CancellationToken>())).ReturnsAsync((Subscription?)null);

            _mapperMock.Setup(m => m.Map<SubscriptionDto>(It.IsAny<Subscription>())).Returns(new SubscriptionDto { Id = 1 });

            // When
            var result = await _subscriptionService.SubscribeAsync(subscriberId, subscriptionCreateDto, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(1);

            _subscriptionRepositoryMock.Verify(repo => repo.AddAsync( It.Is<Subscription>(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId), 
                It.IsAny<CancellationToken>()), 
                Times.Once);

            _subscriptionRepositoryMock.Verify(repo => repo.Update(It.IsAny<Subscription>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SubscribeAsync_WhenSubscriptionIsInactive_ShouldReactivateSubscription()
        {
            // Given
            var subscriptionId = 5;
            var subscriberId = 1;
            var subscribedToId = 2;

            var subscriptionCreateDto = new SubscriptionCreateDto
            {
                SubscribedToId = subscribedToId
            };

            var inactiveSubscription = new Subscription
            {
                Id = subscriptionId,
                SubscribedToId = subscribedToId,
                SubscriberId = subscriberId,
                IsActive = false
            };

            _subscriptionRepositoryMock.Setup(repo => repo.FindSubscriptionAsync(subscriberId, subscribedToId, It.IsAny<CancellationToken>())).ReturnsAsync(inactiveSubscription);

            _subscriptionRepositoryMock.Setup(repo => repo.GetByIdWithDetailsAsync(subscriptionId, It.IsAny<CancellationToken>())).ReturnsAsync(inactiveSubscription);

            _mapperMock.Setup(m => m.Map<SubscriptionDto>(inactiveSubscription)).Returns(new SubscriptionDto { Id = subscriptionId });

            // When
            var result = await _subscriptionService.SubscribeAsync(subscriberId, subscriptionCreateDto, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(subscriptionId);

            _subscriptionRepositoryMock.Verify(repo => repo.Update
                (It.Is<Subscription>(s => s.Id == subscriptionId && s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId && s.IsActive == true)), Times.Once);

            _subscriptionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SubscribeAsync_WhenSubscribingToSelf_ShouldThrowInvalidSubscriptionOperationException()
        {
            // Given
            var userId = 5;

            var subscriptionCreateDto = new SubscriptionCreateDto
            {
                SubscribedToId = userId
            };

            // When
            Func<Task> act = async () => await _subscriptionService.SubscribeAsync(userId, subscriptionCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<InvalidSubscriptionOperationException>();

            _subscriptionRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Subscription>(s => s.SubscriberId == userId && s.SubscribedToId == userId), It.IsAny<CancellationToken>()), Times.Never);
            _subscriptionRepositoryMock.Verify(repo => repo.Update(It.Is<Subscription>(s => s.SubscriberId == userId && s.SubscribedToId == userId)), Times.Never);

            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task SubscribeAsync_WhenAlreadyActivelySubscribed_ShouldThrowAlreadySubscribedException()
        {
            // Given
            var subscriptionId = 10;
            var subscriberId = 5;
            var subscribedToId = 6;

            var subscriptionCreateDto = new SubscriptionCreateDto
            {
                SubscribedToId = subscribedToId
            };

            var existingSubscription = new Subscription
            {
                Id = subscriptionId,
                SubscribedToId = subscribedToId,
                SubscriberId = subscriberId,
                IsActive = true
            };

            _subscriptionRepositoryMock.Setup(repo => repo.FindSubscriptionAsync(subscriberId, subscribedToId, It.IsAny<CancellationToken>())).ReturnsAsync(existingSubscription);

            // When
            Func<Task> act = async () => await _subscriptionService.SubscribeAsync(subscriberId, subscriptionCreateDto, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<AlreadySubscribedException>();

            _subscriptionRepositoryMock.Verify(repo => repo.Update(It.Is<Subscription>(s => s.SubscribedToId == subscribedToId && s.SubscriberId == subscriberId && s.Id == subscriptionId)), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ---UNSUBSCRIBE-------------------------------------------------------------------------------------------------
        [Fact]
        public async Task UnsubscribeAsync_WithActiveSubscription_ShouldDeactivateSubscription()
        {
            // Given
            var subscriptionId = 4;
            var subscriberId = 5;
            var subscribedToId = 6;

            var activeSubscription = new Subscription
            {
                Id = subscriptionId,
                SubscriberId = subscriberId,
                SubscribedToId = subscribedToId,
                IsActive = true
            };

            _subscriptionRepositoryMock.Setup(repo => repo.FindSubscriptionAsync(subscriberId, subscribedToId, It.IsAny<CancellationToken>())).ReturnsAsync(activeSubscription);

            // When
            await _subscriptionService.UnsubscribeAsync(subscriberId, subscribedToId, CancellationToken.None);

            // Then
            _subscriptionRepositoryMock.Verify(repo => repo.Update(It.Is<Subscription>(s => s.Id == subscriptionId && s.SubscribedToId == subscribedToId && s.SubscriberId == subscriberId && s.IsActive == false)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UnsubscribeAsync_WhenSubscriptionIsNotFoundOrInactive_ShouldDoNothing(bool isNotFound)
        {
            // Given
            var subscriberId = 1;
            var subscribedToId = 2;

            Subscription? subscriptionFromDb = isNotFound 
                ? null 
                : new Subscription { Id = 99, SubscriberId = subscriberId, SubscribedToId = subscribedToId, IsActive = false };

            _subscriptionRepositoryMock.Setup(repo => repo.FindSubscriptionAsync(subscriberId, subscribedToId, It.IsAny<CancellationToken>())).ReturnsAsync(subscriptionFromDb);

            // When
            await _subscriptionService.UnsubscribeAsync(subscriberId, subscribedToId, CancellationToken.None);

            // Then
            _subscriptionRepositoryMock.Verify(repo => repo.Update(It.IsAny<Subscription>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}