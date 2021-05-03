using FinalYearProject.Dialogs;
using FinalYearProject.Models;
using FinalYearProject.Services.Authentication;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Activity;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.Services.Database.Message;
using FinalYearProject.Services.Database.Post;
using FinalYearProject.Services.Database.Reports;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Dialogs;
using FinalYearProject.ViewModels.Pages;
using FinalYearProject.Views;
using Prism;
using Prism.Ioc;
using System.Threading.Tasks;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace FinalYearProject
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            InitNavigation();
        }

        protected async void InitNavigation()
        {
            var authService = Container.Resolve<IAuthService>();

            if (!authService.IsSignedIn())
            {
                await GoToLogin();
            }
            else
            {
                await LoadMainApp();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.RegisterSingleton<IGroupDBService, GroupDBService>();
            containerRegistry.RegisterSingleton<IUserDBService, UserDBService>();
            containerRegistry.RegisterSingleton<IMessageDBService, MessageDBService>();
            containerRegistry.RegisterSingleton<IActivityDBService, ActivityDBService>();
            containerRegistry.RegisterSingleton<IPostDBService, PostDBService>();
            containerRegistry.RegisterSingleton<IMessageReportDBService, MessageReportDBService>();
            containerRegistry.RegisterSingleton<IDocumentObserver<User>, UserDocumentObserver>();
            containerRegistry.RegisterSingleton<IDocumentObserver<Group>, GroupDocumentObserver>();
            containerRegistry.RegisterSingleton<IMessageCollectionObserver, MessageCollectionObserver>();

            containerRegistry.RegisterDialog<OptionsDialog, OptionsDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmationDialog, ConfirmationDialogViewModel>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
            containerRegistry.RegisterDialog<DetailsDialog, DetailsDialogViewModel>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoadingPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<UserSettingsPage, UserSettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<NewGroupPage, NewGroupPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupChatPage, GroupChatPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupsPage, GroupsPageViewModel>();
            containerRegistry.RegisterForNavigation<JoinGroupPage, JoinGroupPageViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<ColoursPage, ColoursPageViewModel>();
            containerRegistry.RegisterForNavigation<ReportUserPage, ReportUserPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupTabbedPage, GroupTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupActivitiesPage, GroupActivitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<NewGroupActivityPage, NewGroupActivityPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeUsernamePage, ChangeUsernamePageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupSettingsPage, GroupSettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupInfoPage, GroupInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeGroupNamePage, ChangeGroupNamePageViewModel>();
            containerRegistry.RegisterForNavigation<GroupReportsPage, GroupReportsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeGroupDescriptionPage, ChangeGroupDescriptionPageViewModel>();
            containerRegistry.RegisterForNavigation<GroupPostsPage, GroupPostsPageViewModel>();
            containerRegistry.RegisterForNavigation<NewPostPage, NewPostPageViewModel>();
            containerRegistry.RegisterForNavigation<PostDetailsPage, PostDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<NewReplyPage, NewReplyPageViewModel>();
            containerRegistry.RegisterForNavigation<UserPostsPage, UserPostsPageViewModel>();
        }

        public async Task GoToLogin()
        {
            var userObserver = Container.Resolve<IDocumentObserver<User>>();
            if (userObserver.IsObserving)
            {
                userObserver.StopObserving();
            }

            var groupObserver = Container.Resolve<IDocumentObserver<Group>>();
            var messageObserver = Container.Resolve<IMessageCollectionObserver>();

            await NavigationService.NavigateAsync($"/NavigationPage/{nameof(LoginPage)}");
        }

        public async Task LoadMainApp()
        {
            await NavigationService.NavigateAsync($"/NavigationPage/{nameof(LoadingPage)}");

            var authService = Container.Resolve<IAuthService>();
            var userObserver = Container.Resolve<IDocumentObserver<User>>();

            //await Testing();

            userObserver.BeginObserving(authService.GetUserId());
            await Extensions.TaskExtensions.WaitUntil(() => userObserver.Document is not null);

            await NavigationService.NavigateAsync($"/NavigationPage/{nameof(MainTabbedPage)}?selectedTab={nameof(ProfilePage)}");
        }

        private async Task Testing()
        {
            var testService = new TestService();
            await testService.Test();
        }
    }

    public class TestService : BaseDBService
    {
        public async Task Test()
        {

        }
    }
}