using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MattEland.WhereDoggo.WPFClient.Helpers;

public static class BrushHelpers
{
    public static Brush GetTeamBrush(Teams team)
    {
        return team switch
        {
            Teams.Villagers => Brushes.Blue,
            Teams.Werewolves => Brushes.Red,
            _ => Brushes.Purple
        };
    }
}