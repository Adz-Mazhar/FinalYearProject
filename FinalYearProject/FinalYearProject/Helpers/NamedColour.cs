using Xamarin.Forms;

namespace FinalYearProject.Helpers
{
    public class NamedColour
    {
        public string Name { get; }

        public Color Colour { get; }

        public NamedColour(string name, Color colour)
        {
            Name = name;
            Colour = colour;
        }
    }
}
