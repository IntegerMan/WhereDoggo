namespace MattEland.WhereDoggo.Core.Tests.Roles;

/// <summary>
/// Tests for the <see cref="ExposerRole"/>
/// </summary>
public class ExposerTests : GameTestsBase
{
    [Test]
    public void ExposerShouldAlwaysRevealAtLeastOneCenterCard()
    {
        Assert.Fail("Not Implemented");
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerShouldExposeCorrectNumberOfCards(int numCards)
    {
        Assert.Fail("Not Implemented");
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerShouldHaveCorrectNumberOfRevealedCardEvents(int numCards)
    {
        Assert.Fail("Not Implemented");
    }    
    
    [Test]
    public void ExposerCausesAllPlayersToKnowExposerIsInPlay()
    {
        Assert.Fail("Not Implemented");
    }
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void ExposerCausesAllPlayersToHaveObserveRevealedCardEvents(int numCards)
    {
        Assert.Fail("Not Implemented");
    }
    
    [Test]
    public void ExposerWhoSkipsExposingShouldHaveCorrectEvent()
    {
        Assert.Fail("Not Implemented");
    }
}