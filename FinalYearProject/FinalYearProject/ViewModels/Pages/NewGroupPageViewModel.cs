using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.Group;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FinalYearProject.ViewModels.Pages
{
    public class NewGroupPageViewModel : BaseViewModel
    {
        private static readonly int codeLength = 10;

        public NewGroupPageViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IDocumentObserver<User> userObserver,
                                     IGroupDBService groupDBService)
            : base(navigationService, dialogService, userObserver)
        {
            Categories = Enum.GetValues(typeof(GroupCategory))
                .Cast<GroupCategory>()
                .Select(c => c.ToString().AddSpaceBeforeCapitalLetters())
                .ToList();

            ChosenCategory = Categories[0];

            CreateGroupCommand = new DelegateCommand(
                executeMethod: async () =>
                {
                    try
                    {
                        Group newGroup = new(GroupName,
                                             GroupDescription,
                                             UserObserver.Document.Id,
                                             Enum.Parse<GroupCategory>(ChosenCategory.RemoveSpaces()),
                                             Group.GenerateRandomCode(codeLength));

                        await groupDBService.AddGroupAsync(newGroup);

                        CloseCommand.Execute(null);
                    }
                    catch (Exception)
                    {
                        DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                    }
                },
                canExecuteMethod: () =>
                {
                    return !string.IsNullOrWhiteSpace(GroupName);
                })
                .ObservesProperty(() => GroupName);

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public string GroupName { get; set; }

        public string GroupDescription { get; set; } = "";

        public string ChosenCategory { get; set; }

        public List<string> Categories { get; set; }

        public ICommand CreateGroupCommand { get; }

        public ICommand CloseCommand { get; set; }
    }
}