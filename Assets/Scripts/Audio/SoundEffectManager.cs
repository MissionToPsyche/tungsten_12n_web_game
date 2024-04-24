using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    [Header("Mutable")]
    public AudioSource soundFXObject;

    private Dictionary<Enum, string> soundEvents;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSoundEffect(packet.SoundEffectPacket pkt)
    {
        // Spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, pkt.transform.position, Quaternion.identity);

        // Assign the AudioCLip
        audioSource.clip = pkt.clip;

        // Assign volume
        audioSource.volume = pkt.volume;

        // Play sound
        audioSource.Play();

        // Get length of sound fx clip
        float clipLength = audioSource.clip.length;

        // Destroy the clip after it is finished playing
        Destroy(audioSource.gameObject, clipLength);
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
            { SFX.Cave.Ambience, "cave_ambience" },
            { SFX.Music.MainMenu, "play_menu_music" },
            { SFX.Music.BigJ, "big_j" }
        };

        // packet.SoundEffectPacket sfxpacket = new packet.SoundEffectPacket(this.gameObject, SFX.Music.BigJ);
        // OnSoundEffect(sfxpacket);
    }
}
