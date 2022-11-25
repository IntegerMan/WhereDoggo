namespace MattEland.WhereDoggo.WPFClient.ViewModels;

public class RoleProbabilityViewModel : RoleViewModel
{
    private readonly decimal _probability;

    public RoleProbabilityViewModel(RoleTypes role, decimal probability) : base(role)
    {
        _probability = probability;
    }

    public string ProbabilityText => $"{_probability:P0}";

    public override string ToolTip => $"{Text}: {ProbabilityText}";
}