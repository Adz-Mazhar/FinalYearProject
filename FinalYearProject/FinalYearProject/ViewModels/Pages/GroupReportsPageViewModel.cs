using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.Services.Database.Message;
using FinalYearProject.Services.Database.Reports;
using FinalYearProject.ViewModels.Base;
using FinalYearProject.ViewModels.Helpers;
using Prism.Commands;
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
    public class GroupReportsPageViewModel : GroupRequiringViewModel
    {
        private readonly IMessageDBService messageDBService;
        private readonly IMessageReportDBService messageReportDBService;

        public GroupReportsPageViewModel(INavigationService navigationService,
                                         IDialogService dialogService,
                                         IDocumentObserver<User> userObserver,
                                         IDocumentObserver<Group> groupObserver,
                                         IGroupDBService groupDBService,
                                         IMessageDBService messageDBService,
                                         IMessageReportDBService messageReportDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.messageDBService = messageDBService;
            this.messageReportDBService = messageReportDBService;

            Title = "Reports";

            MessageReports = new ObservableCollection<MessageReportCollection>();

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            BanCommand = new DelegateCommand<MessageReportCollection>(async reportCollection =>
            {
                try
                {
                    var groupId = GroupObserver.Document.Id;

                    await groupDBService.BanUserFromGroup(groupId, reportCollection.SenderId);
                    RemoveAllReportsFromUserCommand.Execute(reportCollection);
                }
                catch (Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });

            RemoveAllReportsFromUserCommand = new DelegateCommand<MessageReportCollection>(async reportCollection =>
            {
                try
                {
                    var groupId = GroupObserver.Document.Id;

                    var reportIds = reportCollection.Select(r => r.Id);
                    await messageReportDBService.RemoveMessageReportsAsync(groupId, reportIds);

                    await RefreshAsync();
                }
                catch (Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });
        }

        public ObservableCollection<MessageReportCollection> MessageReports { get; set; }

        public LayoutState CurrentState { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand BanCommand { get; private set; }

        public ICommand RemoveAllReportsFromUserCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            await GetMessageReportsAsync();
        }

        private async Task RefreshAsync()
        {
            MessageReports.Clear();
            await GetMessageReportsAsync();
        }

        private async Task GetMessageReportsAsync()
        {
            CurrentState = LayoutState.Loading;

            var groupId = GroupObserver.Document.Id;

            var reports = await messageReportDBService.GetMessageReportsAsync(groupId);
            var reportedMessages = await messageDBService.GetMessagesAsync(groupId, reports.Select(r => r.MessageId).Distinct());

            SetMessageReports(reports, reportedMessages);

            CurrentState = LayoutState.None;
        }

        private void SetMessageReports(IEnumerable<MessageReport> reports, IEnumerable<Message> reportedMessages)
        {
            SetReportDetails(reports, reportedMessages);
            var reportedUserIds = reports.Select(r => r.MessageSenderId).Distinct();

            foreach (var userId in reportedUserIds)
            {
                var reportsFromUser = reports.Where(r => r.MessageSenderId == userId);
                var username = reportsFromUser.First().Message.SenderInfo.Username;

                AddExtendedMessageReportCollection(userId, username, reportsFromUser);
            }
        }

        private void SetReportDetails(IEnumerable<MessageReport> reports,
                                      IEnumerable<Message> reportedMessages)
        {
            foreach (var report in reports)
            {
                report.Message = reportedMessages.First(m => m.Id == report.MessageId);
            } 
        }

        private void AddExtendedMessageReportCollection(string senderId, string senderUsername, IEnumerable<MessageReport> messageReports)
        {
            MessageReports.Add(new MessageReportCollection(senderId, senderUsername, messageReports));
        }
    }
}
