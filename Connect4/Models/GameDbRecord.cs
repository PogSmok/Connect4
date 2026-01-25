using System;
using System.Text.Json;

namespace Connect4.Models
{
    public class GameDbRecord
    {
        public int Id { get; set; }

        // Serialized settings
        public string SettingsJson { get; set; } = string.Empty;

        // Serialized moves
        public string MovesJson { get; set; } = string.Empty;

        public GameResult Result { get; set; }

        public DateTime PlayedAt { get; set; }

        // Non-mapped
        public GameSettingsModel Settings
        {
            get => JsonSerializer.Deserialize<GameSettingsModel>(SettingsJson)!;
            set => SettingsJson = JsonSerializer.Serialize(value);
        }

        // Non-mapped
        public List<int> Moves
        {
            get => JsonSerializer.Deserialize<List<int>>(MovesJson)!;
            set => MovesJson = JsonSerializer.Serialize(value);
        }

        // Non-mapped
        public string PlayersText => $"{Settings.RedPlayerName} vs {Settings.YellowPlayerName}";

        // Non-mapped
        public string ResultText =>
            Result switch
            {
                GameResult.RedWonConnect => $"{Settings.RedPlayerName} won",
                GameResult.YellowWonConnect => $"{Settings.YellowPlayerName} won",
                GameResult.RedWonOnTime => $"{Settings.RedPlayerName} won on time",
                GameResult.YellowWonOnTime => $"{Settings.YellowPlayerName} won on time",
                GameResult.RedWonByForfeit => $"{Settings.RedPlayerName} won by forfeit",
                GameResult.YellowWonByForfeit => $"{Settings.YellowPlayerName} won by forfeit",
                GameResult.Draw => "Draw",
                _ => "Unknown result"
            };

        // Non-mapped
        public string PlayedAtText => PlayedAt.ToString("g");

        public GameDbRecord() { }
    }
}
