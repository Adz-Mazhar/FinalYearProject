using FinalYearProject.Dialogs;
using FinalYearProject.ViewModels.Base;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace FinalYearProject.ViewModels.Dialogs
{
    public class DetailsDialogViewModel : BaseDialogViewModel
    {
        public DetailsDialogViewModel()
        {
            Properties = new List<string>();
            Values = new List<string>();
        }

        public List<string> Properties { get; private set; }

        public List<string> Values { get; private set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            var details = parameters.GetValue<List<Detail>>("details");

            if (details is not null || details.Count is not 0)
            {
                Properties = details.Select(d => d.Property).ToList();
                Values = details.Select(d => d.Value).ToList();
            }
        }
    }
}
