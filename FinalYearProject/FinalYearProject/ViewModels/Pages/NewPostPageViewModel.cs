using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Post;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class NewPostPageViewModel : BaseViewModel
    {
        private readonly IPostDBService postDBService;
        private string groupId;

        public NewPostPageViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IDocumentObserver<User> userObserver,
                                    IPostDBService postDBService)
            : base(navigationService, dialogService, userObserver)
        {
            this.postDBService = postDBService;

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            AddPostCommand = new DelegateCommand(
                executeMethod: async () =>
                {
                    try
                    {
                        Post newPost = new(UserObserver.Document, PostText);
                        await postDBService.AddPostAsync(newPost, groupId);

                        await NavigationService.GoBackAsync(("shouldRefresh", true));
                    }
                    catch (Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(PostText);
                })
                .ObservesProperty(() => PostText);
        }

        public string PostText { get; set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand AddPostCommand { get; private set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            groupId = parameters.GetValue<string>("groupId");
            groupId.ThrowIfNull(nameof(parameters), "Parameters must include group id.");
        }
    }
}
