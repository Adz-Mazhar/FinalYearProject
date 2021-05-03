using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Reports;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class ReportUserPageViewModel : GroupRequiringViewModel
    {
        public ReportUserPageViewModel(INavigationService navigationService,
                                       IDialogService dialogService,
                                       IDocumentObserver<User> userObserver,
                                       IDocumentObserver<Group> groupObserver,
                                       IMessageReportDBService messageReportDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            ReportCommand = new DelegateCommand(async () =>
            {
                try
                {
                    MessageReport newMessageReport = new(ReportedMessage.Id,
                                                         ReportedMessage.Sender,
                                                         ReportDescription);

                    await messageReportDBService.AddMessageReportAsync(newMessageReport, GroupObserver.Document.Id);
                }
                catch (System.Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });
        }

        public Message ReportedMessage { get; set; }

        public string ReportDescription { get; set; } = "";

        public ICommand CloseCommand { get; set; }

        public ICommand ReportCommand { get; set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ReportedMessage = parameters.GetValue<Message>("message");
            ReportedMessage.ThrowIfNull(nameof(parameters), "Parameters must include message.");
        }
    }
}
