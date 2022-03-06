using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestCheckWinner()
    {
        HandType[] playerHands0 = new HandType[] {HandType.Paper, HandType.Paper};
        int result = CanvasManager.CheckWinnerIndex(playerHands0);
        Assert.AreEqual(-1, result);
        HandType[] h1 = new HandType[] { HandType.Paper, HandType.Scissor };
        int r1 = CanvasManager.CheckWinnerIndex(h1);
        Assert.AreEqual(1, r1);
        HandType[] h2 = new HandType[] { HandType.Rock, HandType.Scissor };
        int r2 = CanvasManager.CheckWinnerIndex(h2);
        Assert.AreEqual(0, r2);
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
