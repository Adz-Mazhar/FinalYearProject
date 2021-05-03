using FinalYearProject.Events;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Message;
using FinalYearProject.ViewModels.Pages;
using Moq;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinalYearProject.Tests.ViewModels.Pages
{
    public class GroupChatPageViewModelTests
    {
        private MockRepository mockRepository;

        private Mock<INavigationService> mockNavigationService;
        private Mock<IDialogService> mockDialogService;
        private Mock<IDocumentObserver<User>> mockDocumentObserverUser;
        private Mock<IDocumentObserver<Group>> mockDocumentObserverGroup;
        private Mock<IEventAggregator> mockEventAggregator;
        private Mock<IMessageDBService> mockMessageDBService;
        private Mock<IMessageCollectionObserver> mockMessageCollectionObserver;

        public GroupChatPageViewModelTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockNavigationService = mockRepository.Create<INavigationService>();
            mockDialogService = mockRepository.Create<IDialogService>();
            mockDocumentObserverUser = mockRepository.Create<IDocumentObserver<User>>();
            mockDocumentObserverGroup = mockRepository.Create<IDocumentObserver<Group>>();
            mockEventAggregator = mockRepository.Create<IEventAggregator>();
            mockMessageDBService = mockRepository.Create<IMessageDBService>();
            mockMessageCollectionObserver = mockRepository.Create<IMessageCollectionObserver>();
        }

        private GroupChatPageViewModel CreateViewModel()
        {
            return new GroupChatPageViewModel(
                mockNavigationService.Object,
                mockDialogService.Object,
                mockDocumentObserverUser.Object,
                mockDocumentObserverGroup.Object,
                mockEventAggregator.Object,
                mockMessageDBService.Object,
                mockMessageCollectionObserver.Object);
        }

        [Fact]
        public void RefreshCommand_AddsMessagesToDisplayedMessages()
        {
            mockMessageCollectionObserver
                .Setup(m => m.GetMessages(It.Is<int>(i => i >= 3), 0, true))
                .Returns(new List<Message>
                {
                    new Message(),
                    new Message(),
                    new Message(),
                });

            mockEventAggregator.Setup(e => e.GetEvent<NewActivityEvent>()).Returns(new NewActivityEvent());
            mockEventAggregator
                .Setup(e => e.GetEvent<NewActivityEvent>().Subscribe(It.IsAny<Action<ActivityEventArgs>>(), ThreadOption.UIThread, false, It.IsAny<Predicate<ActivityEventArgs>>()))
                .Returns(new SubscriptionToken(token => token.Dispose()));

            mockEventAggregator.Setup(e => e.GetEvent<ActivityCompletedEvent>()).Returns(new ActivityCompletedEvent());
            mockEventAggregator
                .Setup(e => e.GetEvent<ActivityCompletedEvent>().Subscribe(It.IsAny<Action<ActivityEventArgs>>(), ThreadOption.UIThread, false, It.IsAny<Predicate<ActivityEventArgs>>()))
                .Returns(new SubscriptionToken(token => token.Dispose()));

            mockDocumentObserverUser.SetupGet(o => o.Document).Returns(new User());
            mockDocumentObserverGroup.SetupGet(o => o.Document).Returns(new Group());

            var viewModel = CreateViewModel();
            viewModel.RefreshCommand.Execute(null);

            Assert.True(viewModel.DisplayedMessages.Sum(x => x.Count) is 3);
        }
    }
}
