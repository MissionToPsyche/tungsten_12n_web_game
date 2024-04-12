using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using System;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    private Dictionary<Enum, string> soundEvents;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSoundEffect(packet.SoundEffectPacket sfxpacket)
    {
        AkSoundEngine.PostEvent(soundEvents[sfxpacket.sound], sfxpacket.obj);
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        AkBankManager.LoadBank("Main", false, false);

    }

    void Start()
    {
        soundEvents = new Dictionary<Enum, string>()
        {
            { SFX.Player.Walk, "player_walk" },
            { SFX.Player.Run, "player_run" },
            { SFX.Player.Jump, "player_jump" },
            { SFX.Satellite.Move, "satellite_move" },
            { SFX.Satellite.Scan, "satellite_scan" },
            { SFX.Satellite.Orbit, "satellite_orbit" },
            { SFX.Cave.Enter, "cave_enter" },
            { SFX.Cave.Exit, "cave_exit" },
            { SFX.Music.MainMenu, "Menu Music V1" }
        };
    }
}
