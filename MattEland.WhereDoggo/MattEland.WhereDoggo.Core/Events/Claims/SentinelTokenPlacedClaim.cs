﻿namespace MattEland.WhereDoggo.Core.Events.Claims;

public class SentinelTokenPlacedClaim : ClaimBase
{
    public IHasCard Target { get; }

    public SentinelTokenPlacedClaim(GamePlayer player, IHasCard target) : base(player)
    {
        Target = target;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer,
        IHasCard target,
        CardProbabilities probabilities)
    {
        // This should pretty easily be confirmable unless evils are claiming revealer too
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims to be the {Player.InitialCard.RoleType.GetFriendlyName()} that put the token on {Target}";
}