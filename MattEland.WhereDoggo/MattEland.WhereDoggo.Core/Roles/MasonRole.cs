﻿using MattEland.WhereDoggo.Core.Engine.Phases;
using Microsoft.ML.Probabilistic.Collections;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents a Mason role from One Night Ultimate Werewolf (base game).
/// Masons wake up in the night and observe other masons.
/// </summary>
/// <href>http://onenightultimate.com/?p=48</href>
[RoleFor(RoleTypes.Mason)]
public class MasonRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Mason;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new MasonNightAction();
        }
    }
}