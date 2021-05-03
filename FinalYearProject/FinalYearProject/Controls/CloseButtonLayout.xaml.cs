using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseButtonLayout : ContentView
    {
        public CloseButtonLayout()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(CloseButtonLayout),
            defaultValue: default(ICommand));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
                    propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(CloseButtonLayout),
            defaultValue: default);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
    }
}