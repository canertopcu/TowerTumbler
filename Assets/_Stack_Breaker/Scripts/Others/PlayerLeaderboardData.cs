namespace CBGames
{
    public class PlayerLeaderboardData
    {
        public string Name { private set; get; }
        public void SetName(string name)
        {
            Name = name;
        }

        public int Level { private set; get; }
        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}
