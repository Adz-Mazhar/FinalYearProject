using FinalYearProject.Dialogs;
using FinalYearProject.Events;
using FinalYearProject.Extensions;
using FinalYearProject.Helpers;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Message;
using FinalYearProject.ViewModels.Base;
using FinalYearProject.ViewModels.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;

namespace FinalYearProject.ViewModels.Pages
{
    public class GroupChatPageViewModel : GroupRequiringViewModel
    {
        private static readonly int messageIncrement = 25;

        private readonly IMessageDBService messageDBService;
        private readonly IMessageCollectionObserver messageCollectionService;

        private readonly List<AsyncDialogOption> messageOptions;

        private int existingDBMessageCount;

        public GroupChatPageViewModel(INavigationService navigationService,
                                      IDialogService dialogService,
                                      IDocumentObserver<User> userObserver,
                                      IDocumentObserver<Group> groupObserver,
                                      IEventAggregator eventAggregator,
                                      IMessageDBService messageDBService,
                                      IMessageCollectionObserver messageCollectionService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.messageDBService = messageDBService;
            this.messageCollectionService = messageCollectionService;

            eventAggregator.GetEvent<NewActivityEvent>().Subscribe(
                OnNewActivity,
                ThreadOption.UIThread,
                false,
                e => e.GroupId == GroupObserver.Document.Id);

            eventAggregator.GetEvent<ActivityCompletedEvent>().Subscribe(
                OnActivityCompleted,
                ThreadOption.UIThread,
                false,
                e => e.GroupId == GroupObserver.Document.Id);

            Title = "Chat";

            Username = UserObserver.Document.Username;

            DisplayedMessages = new ObservableCollection<MessageCollection>();

            messageOptions = new List<AsyncDialogOption>
            {
                new AsyncDialogOption
                {
                    Name = "Report",
                    ImageSource = "flagicon.png",
                    Response = GoToReportPage,
                }
            };

            MessageTapCommand = new DelegateCommand<Message>(m =>
            {
                SelectedMessage = m;

                DisplayMessageOptions();
            });

            RefreshCommand = new DelegateCommand(() =>
            {
                DisplayMoreMessages();

                IsRefreshing = false;
            });

            SendMessageCommand = new DelegateCommand(async () =>
            {
                if (!string.IsNullOrEmpty(MessageInput))
                {
                    try
                    {
                        Message newMessage = new(UserObserver.Document, MessageInput);
                        await messageDBService.SendMessageAsync(newMessage, GroupObserver.Document.Id);

                        MessageInput = string.Empty;
                    }
                    catch (Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                }
            });
        }

        // When event is triggered, UI will scroll to bottom
        public event EventHandler MessagesLoaded;

        public string Username { get; set; }

        public string MessageInput { get; set; }

        public ObservableCollection<MessageCollection> DisplayedMessages { get; set; }

        public Message SelectedMessage { get; set; }

        public bool IsRefreshing { get; set; }

        public LayoutState CurrentState { get; set; }

        public ICommand MessageTapCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand SendMessageCommand { get; }

        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentState = LayoutState.Loading;

            var groupId = GroupObserver.Document.Id;

            existingDBMessageCount = await messageDBService.GetExistingMessageCount(groupId);
            if (existingDBMessageCount is 0)
            {
                CurrentState = LayoutState.None;
            }

            messageCollectionService.BeginObserving(groupId, OnNewMessageAdded);
        }

        private async void OnActivityCompleted(ActivityEventArgs e)
        {
            var messageText = $"{e.Username} completed: '{e.ActivityTitle}'! Well done!";
            Message newMessage = new(Constants.SystemUser, messageText);

            await messageDBService.SendMessageAsync(newMessage, GroupObserver.Document.Id);
        }

        private async void OnNewActivity(ActivityEventArgs e)
        {
            var messageText = $"{e.Username} created: '{e.ActivityTitle}'! Let's do it together!";
            Message newMessage = new(Constants.SystemUser, messageText);

            await messageDBService.SendMessageAsync(newMessage, GroupObserver.Document.Id);
        }

        private void OnNewMessageAdded(Message message)
        {
            var collectionCount = messageCollectionService.Collection.Count;
            if (existingDBMessageCount - collectionCount < messageIncrement)
            {
                SetIsOwnMessage(message);
                AddToMessageGroup(message, true);
            }

            if (CurrentState is LayoutState.Loading && DisplayedMessages.TotalCount() == collectionCount)
            {
                CurrentState = LayoutState.None;
                OnMessagesLoaded();
            }
        }

        private void SetIsOwnMessage(Message message)
        {
            message.IsOwnMessage = message.Sender == UserObserver.Document.Id;
        }

        // Summary:
        //   Adds new message to a message group. If the corresponding message group does not exist, it is
        //   created.
        // 
        // Parameters:
        //   addToEnd: 
        //      If true, the message will be added to the end of the collection. If false, the message 
        //      will be added to the start of the collection.
        private void AddToMessageGroup(Message message, bool addToEnd)
        {
            string dateString = GetMessageDate(message);
            var messageGroup = DisplayedMessages.Where(mg => mg.Date == dateString).FirstOrDefault();

            if (messageGroup is null)
            {
                AddNewMessageCollection(dateString, addToEnd);

                messageGroup = addToEnd ? messageGroup = DisplayedMessages.Last() : messageGroup = DisplayedMessages.First();
            }

            if (addToEnd)
            {
                messageGroup.Add(message);
            }
            else
            {
                messageGroup.Insert(0, message);
            }
        }

        private string GetMessageDate(Message message)
        {
            var timeStamp = message.SentAt.ToDateTime();

            if (timeStamp.Date == DateTime.Today)
            {
                return "Today";
            }
            else if (timeStamp.Date == DateTime.Today.AddDays(-1))
            {
                return "Yesterday";
            }
            else
            {
                return timeStamp.ToString("M");
            }
        }

        private void AddNewMessageCollection(string dateString, bool addToEnd)
        {
            var newMessageGroup = new MessageCollection(dateString);
            if (addToEnd)
            {
                DisplayedMessages.Add(newMessageGroup);
            }
            else
            {
                DisplayedMessages.Insert(0, newMessageGroup);
            }
        }

        private void DisplayMoreMessages()
        {
            var messageCount = DisplayedMessages.TotalCount();
            var messagesToAdd = messageCollectionService.GetMessages(messageIncrement, messageCount, true);

            foreach (var message in messagesToAdd)
            {
                SetIsOwnMessage(message);
                AddToMessageGroup(message, false);
            }
        }

        private void DisplayReportedUserIsBannedError()
        {
            DialogExtensions.DisplayMessage(DialogService, "Error!", "This user has been banned!");
        }

        private void DisplayMessageOptions()
        {
            var param = new DialogParameters
            {
                { "options", messageOptions.Cast<DialogOptionBase>().ToList() },
            };

            DialogService.ShowDialog("OptionsDialog", param, OptionsDialogCallback);
        }

        private async Task GoToReportPage()
        {
            if (GroupObserver.Document.BannedMembers.Contains(SelectedMessage.Sender))
            {
                DisplayReportedUserIsBannedError();
                return;
            }

            var param = new NavigationParameters
            {
                { "message", SelectedMessage },
            };

            await NavigationService.NavigateAsync("ReportUserPage", param);
        }

        private async void OptionsDialogCallback(IDialogResult dialogResult)
        {
            var choice = dialogResult.Parameters.GetValue<DialogOptionBase>("choice") as AsyncDialogOption;

            if (choice is not null)
            {
                await choice.Response.Invoke();
            }
        }

        private void OnMessagesLoaded()
        {
            var handler = MessagesLoaded;
            handler?.Invoke(this, new EventArgs());
        }
    }
}