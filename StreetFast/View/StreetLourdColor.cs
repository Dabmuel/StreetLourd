using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StreetLourd.View
{
    static class StreetLourdColor
    {
        static public SolidColorBrush RaceColor(string Type)
        {
            switch (Type)
            {
                case "Rally":
                    return new SolidColorBrush(Color.FromRgb(250, 135, 4));
                case "Cross-Country":
                    return new SolidColorBrush(Color.FromRgb(53, 221, 81));
                case "Circuit":
                    return new SolidColorBrush(Color.FromRgb(15, 159, 230));
                default:
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        static public SolidColorBrush RaceOverColor(string Type)
        {
            switch (Type)
            {
                case "Rally":
                    return new SolidColorBrush(Color.FromRgb(200, 108, 3));
                case "Cross-Country":
                    return new SolidColorBrush(Color.FromRgb(42, 177, 65));
                case "Circuit":
                    return new SolidColorBrush(Color.FromRgb(12, 127, 174));
                default:
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        static public SolidColorBrush ClassColor(string Class)
        {
            switch (Class)
            {
                case "X":
                    return new SolidColorBrush(Color.FromRgb(105, 182, 72));
                case "S2":
                    return new SolidColorBrush(Color.FromRgb(76, 87, 230));
                case "S1":
                    return new SolidColorBrush(Color.FromRgb(127, 55, 241));
                case "A":
                    return new SolidColorBrush(Color.FromRgb(188, 60, 35));
                case "B":
                    return new SolidColorBrush(Color.FromRgb(211, 90, 37));
                case "C":
                    return new SolidColorBrush(Color.FromRgb(234, 202, 49));
                case "D":
                    return new SolidColorBrush(Color.FromRgb(107, 185, 236));
                default:
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        static public SolidColorBrush ResearchColor = new SolidColorBrush(Color.FromArgb(128, 167, 37, 220));
    }
}
