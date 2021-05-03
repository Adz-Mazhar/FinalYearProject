using FinalYearProject.Dialogs;
using FinalYearProject.Events;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Activity;
using FinalYearProject.ViewModels.Base;
using FinalYearProject.ViewModels.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace FinalYearProject.ViewModels.Pages
{
    public class GroupActivitiesPageViewModel : GroupRequiringViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IActivityDBService activityDBService;

        private readonly ActivityCollection userActivities;
        private readonly ActivityCollection remainingActivities;
        private readonly List<DialogOptionBase> followedOptions;
        private readonly List<DialogOptionBase> notFollowedOptions;

        public GroupActivitiesPageViewModel(INavigationService navigationService,
                                            IDialogService dialogService,
                                            IDocumentObserver<User> userObserver,
                                            IDocumentObserver<Group> groupObserver,
                                            IEventAggregator eventAggregator,
                                            IActivityDBService activityDBService)
            : base(navigationService, dialogService, userObserver, groupObserver)
        {
            this.eventAggregator = eventAggregator;
            this.activityDBService = activityDBService;

            Title = "Activities";

            userActivities = new ActivityCollection("Following");
            remainingActivities = new ActivityCollection("All");

            Activities = new List<ActivityCollection>()
            {
                userActivities,
                remainingActivities
            };

            followedOptions = new List<DialogOptionBase>
            {
                new AsyncDialogOption
                {
                    Name = "Remove",
                    ImageSource = "minusicon.png",
                    Response = UnfollowSelectedActivityAsync,
                },

                new AsyncDialogOption
                {
                    Name = "Complete",
                    ImageSource = "tickicon.png",
                    Response = CompleteSelectedActivityAsync,
                },

                new DialogOption
                {
                    Name = "Details",
                    ImageSource = "infoicon.png",
                    Response = ShowDetails,
                }
            };

            notFollowedOptions = new List<DialogOptionBase>
            {
                new AsyncDialogOption
                {
                    Name = "Add",
                    ImageSource = "plusicon.png",
                    Response = FollowSelectedActivityAsync,
                },

                new DialogOption
                {
                    Name = "Details",
                    ImageSource = "infoicon.png",
                    Response = ShowDetails,
                }
            };

            InitCommands();
        }

        public bool IsRefreshing { get; set; }

        public List<ActivityCollection> Activities { get; }

        public Activity SelectedActivity { get; set; }

        public ICommand NewActivityCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand ActivityTapCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            await RefreshAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetValue<bool>("shouldRefresh"))
            {
                await RefreshAsync();
            }
        }

        private void InitCommands()
        {
            NewActivityCommand = new DelegateCommand(async () =>
            {
                var navParams = new NavigationParameters
                {
                    { "groupId", GroupObserver.Document.Id },
                };

                await NavigationService.NavigateAsync("NewGroupActivityPage", navParams);
            });

            RefreshCommand = new DelegateCommand(async () =>
            {
                IsRefreshing = true;

                await RefreshAsync();

                IsRefreshing = false;
            });

            ActivityTapCommand = new DelegateCommand<Activity>(async a =>
            {
                SelectedActivity = a;

                if (SelectedActivity.EndsAt < DateTime.Now)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "This activity has finished.");

                    await activityDBService.DeleteActivityAsync(GroupObserver.Document.Id, SelectedActivity.Id);
                    RefreshCommand.Execute(null);
                }
                else
                {
                    IDialogParameters param = new DialogParameters();
                    if (SelectedActivity.IsFollowedByUser)
                    {
                        param.Add("options", followedOptions.Cast<DialogOptionBase>().ToList());
                    }
                    else
                    {
                        param.Add("options", notFollowedOptions.Cast<DialogOptionBase>().ToList());
                    }

                    DialogService.ShowDialog("OptionsDialog", param, OptionsDialogCallback);
                }
            });
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            var activities = await activityDBService.GetActivitiesAsync(GroupObserver.Document.Id, nameof(Activity.Title));

            Activities.ForEach(coll => coll.Clear());
            activities.ForEach(activity => AddToCollection(activity));

            IsBusy = false;
        }

        private void AddToCollection(Activity activity)
        {
            activity.IsFollowedByUser = UserObserver.Document.JoinedActivities.Contains(activity.Id);

            if (activity.IsFollowedByUser)
            {
                userActivities.Add(activity);
            }
            else
            {
                remainingActivities.Add(activity);
            }
        }

        private async Task FollowSelectedActivityAsync()
        {
            try
            {
                await activityDBService.FollowActivityAsync(GroupObserver.Document.Id, SelectedActivity.Id, UserObserver.Document.Id);
                RefreshCommand.Execute(null);
            }
            catch (Exception)
            {
                DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
            }
        }

        private async Task UnfollowSelectedActivityAsync()
        {
            try
            {
                await activityDBService.UnfollowActivityAsync(GroupObserver.Document.Id, SelectedActivity.Id, UserObserver.Document.Id);
                RefreshCommand.Execute(null);
            }
            catch (Exception)
            {
                DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
            }
        }

        private async Task CompleteSelectedActivityAsync()
        {
            try
            {
                await activityDBService.CompleteActivityAsync(GroupObserver.Document.Id, SelectedActivity.Id, UserObserver.Document.Id);

                eventAggregator.GetEvent<ActivityCompletedEvent>().Publish(new ActivityEventArgs
                {
                    GroupId = GroupObserver.Document.Id,
                    Username = UserObserver.Document.Username,
                    ActivityTitle = SelectedActivity.Title,
                });

                RefreshCommand.Execute(null);
            }
            catch (Exception)
            {
                DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
            }
        }

        private void ShowDetails()
        {
            var details = new List<Detail>
            {
                new Detail { Property = "Following", Value = $"{SelectedActivity.FollowerCount}"},
                new Detail { Property = "Completed", Value = $"{SelectedActivity.CompletedCount}"},
                new Detail { Property = "Ends", Value = $"{ConvertEndDateTimeToString(SelectedActivity.EndsAt)}"},
            };

            var dialogParams = new DialogParameters
            {
                { "details", details }
            };

            DialogService.ShowDialog("DetailsDialog", dialogParams);
        }

        private string ConvertEndDateTimeToString(DateTime endDateTime)
        {
            var localTime = endDateTime.ToLocalTime();
            return $"{localTime:dddd} {localTime:M}, {localTime:t}";
        }

        private async void OptionsDialogCallback(IDialogResult dialogResult)
        {
            var choice = dialogResult.Parameters.GetValue<DialogOptionBase>("choice");

            if (choice is DialogOption option)
            {
                option.Response.Invoke();
            }
            else if (choice is AsyncDialogOption asyncOption)
            {
                await asyncOption.Response.Invoke();
            }
        }
    }
}
