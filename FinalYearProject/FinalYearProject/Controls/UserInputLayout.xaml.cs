using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInputLayout : ContentView
    {
        public UserInputLayout()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(UserInputLayout),
            defaultValue: default(string));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TitleMarginProperty = BindableProperty.Create(
            propertyName: nameof(TitleMargin),
            returnType: typeof(Thickness),
            declaringType: typeof(TitledLayout),
            defaultValue: new Thickness(20, 50, 20, 50));

        public Thickness TitleMargin
        {
            get => (Thickness)GetValue(TitleMarginProperty);
            set => SetValue(TitleMarginProperty, value);
        }

        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(
            propertyName: nameof(ButtonText),
            returnType: typeof(string),
            declaringType: typeof(UserInputLayout),
            defaultValue: default(string));

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public static readonly BindableProperty ButtonHeightRequestProperty = BindableProperty.Create(
            propertyName: nameof(ButtonHeightRequest),
            returnType: typeof(double),
            declaringType: typeof(UserInputLayout),
            defaultValue: default(double));

        public double ButtonHeightRequest
        {
            get => (double)GetValue(ButtonHeightRequestProperty);
            set => SetValue(ButtonHeightRequestProperty, value);
        }

        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(ButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(UserInputLayout),
            defaultValue: default(ICommand));

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public static readonly BindableProperty ButtonCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(ButtonCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(UserInputLayout),
            defaultValue: default);

        public object ButtonCommandParameter
        {
            get => GetValue(ButtonCommandParameterProperty);
            set => SetValue(ButtonCommandParameterProperty, value);
        }

        public static readonly BindableProperty BackButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(BackButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(UserInputLayout),
            defaultValue: default(ICommand));

        public ICommand BackButtonCommand
        {
            get => (ICommand)GetValue(BackButtonCommandProperty);
            set => SetValue(BackButtonCommandProperty, value);
        }

        public static readonly BindableProperty BackButtonCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(BackButtonCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(UserInputLayout),
            defaultValue: default);

        public object BackButtonCommandParameter
        {
            get => GetValue(BackButtonCommandParameterProperty);
            set => SetValue(BackButtonCommandParameterProperty, value);
        }
    }
}