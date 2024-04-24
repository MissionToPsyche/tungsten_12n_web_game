using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using packet;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [Header("Mutable")]
    public AudioSource soundFXObject;

    [Header("ReadOnly")]
    private Dictionary<SoundEffect, AudioClip> soundMap;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSoundEffect(SoundEffectPacket pkt)
    {
        // Spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, pkt.transform.position, Quaternion.identity);

        // Assign the AudioClip
        if (soundMap.TryGetValue(pkt.soundReference, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.volume = pkt.volume;

            if (audioSource.clip != null)
            {
                // Play sound
                audioSource.Play();

                // Get length of sound fx clip
                float clipLength = audioSource.clip.length;

                // Destroy the clip after it is finished playing
                Destroy(audioSource.gameObject, clipLength);
            }
            else
            {
                Debug.LogError("Audio clip is null: " + pkt.soundReference);
                Destroy(audioSource.gameObject);
            }
        }
        else
        {
            Debug.LogError("Sound effect not found: " + pkt.soundReference);
            Destroy(audioSource.gameObject);
        }
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

        soundMap = new Dictionary<SoundEffect, AudioClip>();
        InitializeSounds();
    }

    private void InitializeSounds()
    {
        foreach (SoundEffect effect in Enum.GetValues(typeof(SoundEffect)))
        {
            var resourcePath = GetResourcePath(effect);
            if (!string.IsNullOrEmpty(resourcePath))
            {
                var clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    soundMap.Add(effect, clip);
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip: " + resourcePath);
                }
            }
        }
    }

    private string GetResourcePath(SoundEffect effect)
    {
        var type = effect.GetType();
        var memInfo = type.GetMember(effect.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(ResourcePathAttribute), false);
        return attributes.Length > 0 ? ((ResourcePathAttribute)attributes[0]).Path : null;
    }
}
