
public enum InputType
{
    Separated,
    Combinated,
    Mouse
}

public static class PlayerInfo
{
    private static InputType inputType = InputType.Combinated;
    private static int maxScore = 0;

    private static int actualScore = 0;
    private static int enemiesKilled = 0;
    private static float totalTime = 0f;
    private static bool isGameOver = false;


    public static void SetInputType(InputType input)
    {
        inputType = input;
    }

    public static InputType GetInputType()
    {
        return inputType;
    }

    public static void SetEndGame(bool gameOver, int actualScore, float totalTime, int enemiesKilled)
    {
        isGameOver = gameOver;
        PlayerInfo.totalTime = totalTime;
        PlayerInfo.actualScore = actualScore;
        PlayerInfo.enemiesKilled = enemiesKilled;
        PlayerInfo.totalTime = totalTime;
    }

    public static bool IsGameOver()
    {
        return isGameOver;
    }

    public static int GetActualScore()
    {
        return actualScore;
    }

    public static int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public static float GetTotalTime()
    {
        return totalTime;
    }

    public static int GetMaxScore()
    {
        return maxScore;
    }
}
