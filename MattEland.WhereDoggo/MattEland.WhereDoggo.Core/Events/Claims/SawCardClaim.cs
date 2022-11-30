using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattEland.WhereDoggo.Core.Events.Claims;

public class SawCardClaim : ClaimBase
{
    public IHasCard Card { get; }
    public RoleTypes Role { get; }

    public SawCardClaim(GamePlayer player, IHasCard card, RoleTypes observedRole) : base(player)
    {
        Card = card;
        Role = observedRole;
    }

    public override void UpdatePlayerPerceptions(GamePlayer observer, 
        IHasCard target, 
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }
}