using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitledLayout : ContentView
    {
        public TitledLayout()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(TitledLayout),
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
    }
}