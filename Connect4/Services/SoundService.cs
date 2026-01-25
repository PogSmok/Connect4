using System.Media;
using System.IO;

/// <summary>
/// Handles playing background music
/// </summary>
public sealed class SoundService
{
    private static readonly string _musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "bg_music.wav");

    private static SoundPlayer? _player;
    public static void PlayBackgroundMusic()
    {
        if (_player == null)
        {
            _player = new SoundPlayer(_musicPath);
            _player.Load();
        }
            
        _player.PlayLooping();
    }

    public static void StopBackgroundMusic()
    {
        _player?.Stop();
    }
}