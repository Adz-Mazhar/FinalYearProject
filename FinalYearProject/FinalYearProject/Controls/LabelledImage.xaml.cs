using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelledImage : ContentView
    {
        public LabelledImage()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(LabelledImage),
            defaultValue: default(string));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            propertyName: nameof(FontSize),
            returnType: typeof(double),
            declaringType: typeof(LabelledImage),
            defaultValue: 17.0);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty BoxPaddingProperty = BindableProperty.Create(
            propertyName: nameof(BoxPadding),
            returnType: typeof(Thickness),
            declaringType: typeof(LabelledImage),
            defaultValue: new Thickness(20));

        public Thickness BoxPadding
        {
            get => (Thickness)GetValue(BoxPaddingProperty);
            set => SetValue(BoxPaddingProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            propertyName: nameof(CornerRadius),
            returnType: typeof(float),
            declaringType: typeof(LabelledImage));

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ImageSource),
            returnType: typeof(string),
            declaringType: typeof(LabelledImage),
            defaultValue: default(string));

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty ImageHeightRequestProperty = BindableProperty.Create(
            propertyName: nameof(ImageHeightRequest),
            returnType: typeof(double),
            declaringType: typeof(LabelledImage),
            defaultValue: default);

        public double ImageHeightRequest
        {
            get => (double)GetValue(ImageHeightRequestProperty);
            set => SetValue(ImageHeightRequestProperty, value);
        }

        public static readonly BindableProperty ImageWidthRequestProperty = BindableProperty.Create(
            propertyName: nameof(ImageWidthRequest),
            returnType: typeof(double),
            declaringType: typeof(LabelledImage),
            defaultValue: default);

        public double ImageWidthRequest
        {
            get => (double)GetValue(ImageWidthRequestProperty);
            set => SetValue(ImageWidthRequestProperty, value);
        }

        public static readonly BindableProperty BorderColourProperty = BindableProperty.Create(
            propertyName: nameof(BorderColour),
            returnType: typeof(Color),
            declaringType: typeof(LabelledImage),
            defaultValue: Color.Black);

        public Color BorderColour
        {
            get => (Color)GetValue(BorderColourProperty);
            set => SetValue(BorderColourProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(LabelledImage),
            defaultValue: default(ICommand));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(LabelledImage),
            defaultValue: default);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public Command OnTap
        {
            get => new(() => Execute(Command, CommandParameter));
        }

        public void Execute(ICommand command, object commandParameter)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(commandParameter);
            }
        }
    }
}