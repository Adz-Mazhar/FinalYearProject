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
    public class UserPostsPageViewModel : BaseViewModel
    {
        private readonly IPostDBService postDBService;
        public UserPostsPageViewModel(INavigationService navigationService,
                                      IDialogService dialogService,
                                      IDocumentObserver<User> userObserver,
                                      IPostDBService postDBService)
            : base(navigationService, dialogService, userObserver)
        {
            this.postDBService = postDBService;

            CloseCommand = new DelegateCommand(async () => await NavigationService.GoBackAsync());
        }

        public List<Post> Posts { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public override async void Initialize(INavigationParameters parameters)
        {
            Posts = (await postDBService.GetPostsAsync(UserObserver.Document.Posts)).ToList();
        }
    }
}
