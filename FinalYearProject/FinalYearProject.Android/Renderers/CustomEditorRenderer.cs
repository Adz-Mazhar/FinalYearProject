using Android.Content;
using FinalYearProject.Controls;
using FinalYearProject.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(CustomEditorRenderer))]
namespace FinalYearProject.Droid.Renderers
{
    class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);

                if (e.NewElement != null)
                {
                    var customControl = (ExpandableEditor)Element;
                    Control.SetMaxLines(customControl.MaxLines);
                }
            }
        }
    }
}