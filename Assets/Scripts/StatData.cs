public class StatData 
{
    public int winCount;
    public int drawCount;
    public int loseCount;
    public int GetTotalScore()
    {
        return (winCount * 3) + (drawCount);
    }

    public void ResetScore()
    {
        winCount = 0;
        drawCount = 0;
        loseCount = 0;
    }

}
