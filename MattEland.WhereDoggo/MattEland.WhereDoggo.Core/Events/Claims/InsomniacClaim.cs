using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.Core.Events.Claims;

public class InsomniacClaim : ClaimBase
{
    public RoleTypes Role { get; }

    public InsomniacClaim(GamePlayer player, RoleTypes observedRole) : base(player)
    {
        Role = observedRole;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, 
        IHasCard target, 
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }

    /// <inheritdoc />
    public override string Text => Role == Player!.InitialCard.RoleType 
        ? $"{Player} claims to have seen themselves as still the {Player.InitialCard.RoleType.GetFriendlyName()}"
        : $"{Player} claims they were the {Player.InitialCard.RoleType.GetFriendlyName()} but are now the {Role.GetFriendlyName()}";
}