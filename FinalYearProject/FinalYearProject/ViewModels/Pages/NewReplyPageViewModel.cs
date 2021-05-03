using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Post;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class NewReplyPageViewModel : BaseViewModel
    {
        private string groupId;
        private string postId;

        public NewReplyPageViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IDocumentObserver<User> userObserver,
                                    IPostDBService postDBService)
            : base(navigationService, dialogService, userObserver)
        {
            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            AddReplyCommand = new DelegateCommand(
                executeMethod: async () =>
                {
                    try
                    {
                        Reply newReply = new(UserObserver.Document, ReplyText);
                        await postDBService.AddReplyAsync(newReply, groupId, postId);

                        await NavigationService.GoBackAsync(("shouldRefresh", true));
                    }
                    catch (Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(ReplyText);
                })
                .ObservesProperty(() => ReplyText);
        }

        public string ReplyText { get; set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand AddReplyCommand { get; private set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            groupId = parameters.GetValue<string>("groupId");
            groupId.ThrowIfNull(nameof(parameters), "Parameters must include group id.");

            postId = parameters.GetValue<string>("postId");
            postId.ThrowIfNull(nameof(parameters), "Parameters must include post id.");
        }
    }
}
