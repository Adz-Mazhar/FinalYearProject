using FinalYearProject.Dialogs;
using FinalYearProject.Extensions;
using FinalYearProject.Helpers;
using FinalYearProject.Models;
using FinalYearProject.Services.Database;
using FinalYearProject.Services.Database.User;
using FinalYearProject.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace FinalYearProject.ViewModels.Pages
{
    public class ColoursPageViewModel : BaseViewModel
    {
        private List<NamedColour> allNamedColours;

        public ColoursPageViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IDocumentObserver<User> userObserver,
                                    IUserDBService userDBService)
            : base(navigationService, dialogService, userObserver)
        {
            SetAllNamedColours();

            Colours = allNamedColours;

            CloseCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            SelectColourCommand = new DelegateCommand<NamedColour>(async colour =>
            {
                try
                {
                    await userDBService.UpdateProfileColourAsync(UserObserver.Document.Id, colour.Colour);
                    CloseCommand.Execute(null);
                }
                catch (System.Exception)
                {
                    DialogExtensions.DisplayMessage(DialogService, "Error!", "An error occured. Please try again.");
                }
            });
        }

        public List<NamedColour> Colours { get; set; }

        public string SearchText { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand SelectColourCommand { get; }

        private void OnSearchTextChanged()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                Colours = allNamedColours;
            }
            else
            {
                Colours = allNamedColours
                    .Where(c => c.Name.Contains(SearchText, System.StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }
        }

        private void SetAllNamedColours()
        {
            allNamedColours = ColourExtensions.GetAllColoursWithNames();

            AddCustomColours();

            allNamedColours = allNamedColours.OrderBy(c => c.Name).ToList();
        }

        private void AddCustomColours()
        {
            allNamedColours.AddRange(new List<NamedColour>
            {
                new NamedColour("Scarlet Red", new Color(1, 0.141, 0)),
                new NamedColour("Cobalt Blue", new Color(0, 0.278, 0.671)),
                new NamedColour("Baby Pink", new Color(0.957, 0.761, 0.761)),
                new NamedColour("Lilac", new Color(0.784, 0.635, 0.784)),
                new NamedColour("Mint Green", new Color(0.596, 1, 0.596)),
                new NamedColour("Copper", new Color(0.722, 0.451, 0.2)),
                new NamedColour("Rose Gold", new Color(0.718, 0.431, 0.475)),
            });
        }
    }
}
