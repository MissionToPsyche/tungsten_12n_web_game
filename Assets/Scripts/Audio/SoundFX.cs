using System;

public enum SoundEffect
{
    [ResourcePath("Audio/SFX/player_walk")]
    PlayerWalk,

    [ResourcePath("Audio/SFX/player_jump")]
    PlayerJump,

    [ResourcePath("Audio/SFX/cave_enter")]
    CaveEnter,

    [ResourcePath("Audio/SFX/cave_exit")]
    CaveExit,

    [ResourcePath("Audio/Music/cave_ambience")]
    CaveAmbience,

    [ResourcePath("Audio/Music/main_menu")]
    MainMenuMusic,

    [ResourcePath("Audio/Music/BigJsFaceInSpace")]
    BigJMusic
}

public class ResourcePathAttribute : Attribute
{
    public string Path { get; private set; }

    public ResourcePathAttribute(string path)
    {
        Path = path;
    }
}
