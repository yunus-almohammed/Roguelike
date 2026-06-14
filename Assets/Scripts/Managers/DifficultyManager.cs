public enum Difficulty
{
    Normal = 1,
    Hard = 2,
    Extreme = 3
}

public static class DifficultyManager
{
    public static int SelectedLevelNumber { get; private set; } = 1;
    public static Difficulty SelectedDifficulty { get; private set; } = Difficulty.Normal;

    public static void SelectLevelAndDifficulty(int levelNumber, Difficulty difficulty)
    {
        SelectedLevelNumber = levelNumber;
        SelectedDifficulty = difficulty;
    }

    public static int GetStarsForDifficulty(Difficulty difficulty)
    {
        return (int)difficulty;
    }

    public static string GetDifficultyName(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Normal:
                return "Normal";
            case Difficulty.Hard:
                return "Hard";
            case Difficulty.Extreme:
                return "Extreme";
            default:
                return "Normal";
        }
    }
}
