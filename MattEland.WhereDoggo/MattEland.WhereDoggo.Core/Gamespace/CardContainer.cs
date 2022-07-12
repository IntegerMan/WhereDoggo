namespace MattEland.WhereDoggo.Core.Gamespace;

/// <summary>
/// Represents something that can have a card in it. This is typically going
/// to be either a <see cref="GamePlayer"/> or a <see cref="CenterCardSlot" />.
/// </summary>
public abstract class CardContainer
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="CardContainer"/> class.
    /// </summary>
    /// <param name="name">The name of the container</param>
    /// <param name="initialRole">The initial card stored in the container</param>
    protected CardContainer(string name, RoleBase initialRole)
    {
        Name = name;
        InitialRole = initialRole;
        CurrentRole = initialRole;
    }

    /// <summary>
    /// Gets the name of the player or card slot
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets the role of the card that was here at the beginning of the game.
    /// </summary>
    public RoleBase InitialRole { get; }
    
    /// <summary>
    /// Gets or sets the role currently in the card slot
    /// </summary>
    public RoleBase CurrentRole { get; set; }

    /// <summary>
    /// Whether or not the card is revealed. Defaults to false but may be true if a <see cref="RevealerRole"/> is present.
    /// </summary>
    public bool IsRevealed { get; set; }

    /// <summary>
    /// Gets the current team the player is on.
    /// </summary>
    public Teams CurrentTeam => CurrentRole.Team;

    /// <summary>
    /// Gets the initial team the player was on at the beginning of the game.
    /// </summary>
    public Teams InitialTeam => InitialRole.Team;

    /// <inheritdoc />
    public override string ToString() => Name;
}