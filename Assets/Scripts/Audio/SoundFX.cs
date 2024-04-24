using System;
using UnityEngine;


namespace SFX
{
    public enum Music
    {
        [ResourcePath("Audio/Music/cave_ambience")]
        CaveAmbience,

        [ResourcePath("Audio/Music/main_menu")]
        MainMenu,

        [ResourcePath("Audio/Music/big_js_face_in_space")]
        BigJ
    }

    public enum Cave
    {
        [ResourcePath("Audio/SFX/cave_enter")]
        Enter,

        [ResourcePath("Audio/SFX/cave_exit")]
        Exit,

        [ResourcePath("Audio/SFX/resource_exhausted")]
        MineResource,
    }

    public enum Player
    {
        [ResourcePath("Audio/SFX/player_walk")]
        Walk,

        [ResourcePath("Audio/SFX/player_jump")]
        Jump,
    }

    public enum Satellite
    {
        [ResourcePath("Audio/SFX/satellite_spawn")]
        Spawn,

    }

    public enum MiniGame
    {
        [ResourcePath("Audio/SFX/minigame_won")]
        Won,

        [ResourcePath("Audio/SFX/minigame_error")]
        Error,

        [ResourcePath("Audio/SFX/minigame_switch")]
        Switch,

        [ResourcePath("Audio/SFX/minigame_typing")]
        Typing,

        [ResourcePath("Audio/SFX/minigame_pullcord")]
        PullCord,
    }

    public enum Robot
    {

    }

    public class ResourcePathAttribute : Attribute
    {
        public string resourcePath { get; private set; }

        public ResourcePathAttribute(string path)
        {
            resourcePath = path;
        }
    }
}
