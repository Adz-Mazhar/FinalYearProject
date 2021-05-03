using FinalYearProject.Dialogs;
using FinalYearProject.Events;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Activity;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class NewGroupActivityPageViewModel : BaseViewModel
    {
        private string groupId;

        public NewGroupActivityPageViewModel(INavigationService navigationService,
                                             IDialogService dialogService,
                                             IDocumentObserver<User> userObserver,
                                             IEventAggregator eventAggregator,
                                             IActivityDBService activityDBService)
            : base(navigationService, dialogService, userObserver)
        {
            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            CreateActivityCommand = new DelegateCommand(
                executeMethod: async () =>
                {
                    DateTime endDateTime = Date + Time;
                    if (endDateTime < DateTime.Now)
                    {
                        DisplayError("Date and Time are not valid");
                        return;
                    }

                    try
                    {
                        Activity newActivity = new(ActivityTitle, UserObserver.Document, endDateTime);
                        await activityDBService.AddActivityAsync(newActivity, groupId);

                        eventAggregator.GetEvent<NewActivityEvent>().Publish(new ActivityEventArgs
                        {
                            ActivityTitle = ActivityTitle,
                            Username = UserObserver.Document.Username,
                            GroupId = groupId,
                        });

                        await NavigationService.GoBackAsync(("shouldRefresh", true));
                    }
                    catch (Exception)
                    {
                        DisplayError("An error occured.Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(ActivityTitle);
                })
                .ObservesProperty(() => ActivityTitle);
        }

        public string ActivityTitle { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        public TimeSpan Time { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand CreateActivityCommand { get; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            groupId = parameters.GetValue<string>("groupId");
            groupId.ThrowIfNull(nameof(groupId));
        }

        private void DisplayError(string message)
        {
            DialogExtensions.DisplayMessage(DialogService, "Error!", message);
        }
    }
}
