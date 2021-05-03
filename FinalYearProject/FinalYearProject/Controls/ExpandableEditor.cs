using PropertyChanged;
using Xamarin.Forms;

namespace FinalYearProject.Controls
{
    [SuppressPropertyChangedWarnings]
    public class ExpandableEditor : Editor
    {
        public ExpandableEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ExpandableEditor()
        {
            TextChanged -= OnTextChanged;
        }

        public static BindableProperty IsExpandableProperty = BindableProperty.Create(
            propertyName: nameof(IsExpandable),
            returnType: typeof(bool),
            declaringType: typeof(ExpandableEditor),
            defaultValue: default(bool));

        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }

        public static BindableProperty MaxLinesProperty = BindableProperty.Create(
            propertyName: nameof(MaxLines),
            returnType: typeof(int),
            declaringType: typeof(ExpandableEditor),
            defaultValue: default(int));

        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
                InvalidateMeasure();
        }
    }
}