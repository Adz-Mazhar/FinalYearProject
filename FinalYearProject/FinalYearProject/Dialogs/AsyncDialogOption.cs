using System;
using System.Threading.Tasks;

namespace FinalYearProject.Dialogs
{
    public class AsyncDialogOption : DialogOptionBase
    {
        public Func<Task> Response { get; set; }
    }
}
