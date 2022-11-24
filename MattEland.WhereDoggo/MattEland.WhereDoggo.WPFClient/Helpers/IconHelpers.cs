using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.WPFClient.Helpers
{
    public static class IconHelpers
    {
        public static string GetRoleIcon(RoleTypes role) => role switch
            {
                RoleTypes.Werewolf => "Solid_Dog",
                RoleTypes.MysticWolf => "Solid_Paw", // Or Solid_ShieldDog, Solid_Paw, Solid_Bone
                RoleTypes.Seer => "Solid_Eye",
                RoleTypes.ApprenticeSeer => "Solid_EyeLowVision",
                RoleTypes.Mason => "Solid_PeopleCarryBox", // or Solid_TrowelBricks
                RoleTypes.Villager => "Solid_Person",
                RoleTypes.Insomniac => "Solid_Bed",
                _ => "Solid_Question"
            };

        public static string GetPhaseIcon(string phase) =>
            phase switch
            {
                "Setup" => "Solid_UserGear",
                "Night" => "Solid_Moon",
                "Day" => "Solid_Sun",
                "Voting" => "Solid_CheckToSlot",
                _ => "Solid_Question"
            };
    }
}
