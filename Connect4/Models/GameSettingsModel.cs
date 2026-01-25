namespace Connect4.Models
{
    /// <summary>
    /// Represents the settings for a single Connect 4 game session.
    /// </summary>
    public class GameSettingsModel
    {
        public string RedPlayerName { get; set; } = "Red";
        public string YellowPlayerName { get; set; } = "Yellow";
        public PlayerColor StartingPlayer { get; set; } = PlayerColor.Red;
        public int BaseTimeMinutes { get; set; } = -1; // -1 indicated no time limit has been set
        public int IncrementSeconds { get; set; } = 0;

        public GameSettingsModel() { }
    }
}