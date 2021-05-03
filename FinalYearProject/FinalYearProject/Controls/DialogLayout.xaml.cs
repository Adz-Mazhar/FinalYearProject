using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogLayout : ContentView
    {
        public DialogLayout()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(DialogLayout));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TitleTextColourProperty = BindableProperty.Create(
            propertyName: nameof(TitleTextColour),
            returnType: typeof(Color),
            declaringType: typeof(DialogLayout));

        public Color TitleTextColour
        {
            get => (Color)GetValue(TitleTextColourProperty);
            set => SetValue(TitleTextColourProperty, value);
        }

        public static readonly BindableProperty TitleBackgroundColourProperty = BindableProperty.Create(
            propertyName: nameof(TitleBackgroundColour),
            returnType: typeof(Color),
            declaringType: typeof(DialogLayout));

        public Color TitleBackgroundColour
        {
            get => (Color)GetValue(TitleBackgroundColourProperty);
            set => SetValue(TitleBackgroundColourProperty, value);
        }

        public static BindableProperty ShowOptionBarProperty = BindableProperty.Create(
            propertyName: nameof(ShowOptionBar),
            returnType: typeof(bool),
            declaringType: typeof(DialogLayout),
            defaultValue: true);

        public bool ShowOptionBar
        {
            get { return (bool)GetValue(ShowOptionBarProperty); }
            set { SetValue(ShowOptionBarProperty, value); }
        }

        public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
            propertyName: nameof(CloseCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(DialogLayout));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
    }
}