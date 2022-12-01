namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public static class FakeRoleClaimProvider
{
    public static IClaimProvider? GetClaimProvider(this RoleTypes claimedRole)
    {
        return claimedRole switch
        {
            RoleTypes.Seer => new FakeSeerClaimProvider(),
            RoleTypes.ApprenticeSeer => new FakeApprenticeSeerClaimProvider(),
            RoleTypes.Insomniac => new FakeInsomniacClaimProvider(),
            RoleTypes.Mason => new FakeMasonClaimProvider(),
            RoleTypes.Exposer => new FakeExposerClaimProvider(),
            RoleTypes.Revealer => new FakeRevealerClaimProvider(),
            RoleTypes.Thing => new FakeThingClaimProvider(),
            RoleTypes.Sentinel => new FakeSentinelClaimProvider(),
            _ => null
        };
    }
}