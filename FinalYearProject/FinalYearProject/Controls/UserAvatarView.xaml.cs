using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAvatarView : ContentView
    {
        public UserAvatarView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ColourProperty = BindableProperty.Create(
            propertyName: nameof(Colour),
            returnType: typeof(Color),
            declaringType: typeof(UserAvatarView),
            defaultValue: default(Color));

        public Color Colour
        {
            get => (Color)GetValue(ColourProperty);
            set => SetValue(ColourProperty, value);
        }

        public static readonly BindableProperty SizeProperty = BindableProperty.Create(
            propertyName: nameof(Size),
            returnType: typeof(double),
            declaringType: typeof(UserAvatarView),
            defaultValue: default(double));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SizeProperty.PropertyName)
            {
                avatar.CornerRadius = CalculateCornerRadius();
            }
        }

        private int CalculateCornerRadius()
        {
            return (int)(Size / 2);
        }
    }
}