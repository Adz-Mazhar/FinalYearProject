using FinalYearProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FinalYearProject.Extensions
{
    public static class ColourExtensions
    {
        public static List<Color> GetAllColours()
        {
            var colors = new List<Color>();

            foreach (var field in typeof(Color).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field != null && !string.IsNullOrEmpty(field.Name))
                {
                    var color = (Color)field.GetValue(field);
                    colors.Add(color);
                }
            }

            return colors;
        }

        public static List<NamedColour> GetAllColoursWithNames(bool addSpacesBetweenWords = true)
        {
            var coloursWithNames = new List<NamedColour>();

            foreach (var field in typeof(Color).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field != null && !string.IsNullOrEmpty(field.Name))
                {
                    var color = (Color)field.GetValue(field);
                    var name = addSpacesBetweenWords ? AddSpacesBeforeCapitalLetters(field.Name) : field.Name;

                    coloursWithNames.Add(new NamedColour(name, color));
                }
            }

            return coloursWithNames;
        }

        public static Color GetRandomColour()
        {
            var colors = GetAllColours();

            var random = new Random();
            return colors[random.Next(colors.Count)];
        }

        private static string AddSpacesBeforeCapitalLetters(string str)
        {
            return string.Concat(str.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}
