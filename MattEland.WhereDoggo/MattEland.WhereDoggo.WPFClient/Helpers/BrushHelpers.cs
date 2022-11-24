using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MattEland.WhereDoggo.WPFClient.Helpers;

public static class BrushHelpers
{
    public static Brush GetTeamBrush(Teams? team) => team switch
        {
            Teams.Villagers => Brushes.Blue,
            Teams.Werewolves => Brushes.Red,
            null => Brushes.Black,
            _ => Brushes.Purple
        };
    public static Brush GetTeamBackgroundBrush(Teams? team) => team switch
        {
            Teams.Villagers => Brushes.AliceBlue,
            Teams.Werewolves => Brushes.MistyRose,
            null => Brushes.LightGray,
            _ => Brushes.Purple
        };
}