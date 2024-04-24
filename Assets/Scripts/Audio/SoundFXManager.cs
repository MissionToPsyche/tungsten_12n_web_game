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
    private Dictionary<Enum, AudioClip> soundMap;

    private List<AudioSource> activeAudioSources = new();

    // -------------------------------------------------------------------
    // Handle events

    public void PlaySound(Enum type, Transform transform, float volume)
    {
        // Spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);

        // Assign the AudioClip
        if (soundMap.TryGetValue(type, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.volume = volume;

            if (audioSource.clip != null)
            {
                audioSource.Play();
                Destroy(audioSource.gameObject, clip.length);
                activeAudioSources.Add(audioSource); // Track the source
            }
            else
            {
                Debug.LogError("Audio clip is null: " + type);
                Destroy(audioSource.gameObject);
            }
        }
        else
        {
            Debug.LogError("Sound effect not found: " + type);
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

        soundMap = new Dictionary<Enum, AudioClip>();
        InitializeSounds();
    }

    private void InitializeSounds()
    {
        LoadSoundsForEnumType(typeof(SFX.Music));
        LoadSoundsForEnumType(typeof(SFX.Cave));
        LoadSoundsForEnumType(typeof(SFX.Player));
        LoadSoundsForEnumType(typeof(SFX.Satellite));
        LoadSoundsForEnumType(typeof(SFX.Robot));
        LoadSoundsForEnumType(typeof(SFX.MiniGame));
    }

    private void LoadSoundsForEnumType(Type enumType)
    {
        foreach (Enum effect in Enum.GetValues(enumType))
        {
            var resourcePath = GetResourcePath(effect);
            if (!string.IsNullOrEmpty(resourcePath))
            {
                var clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    soundMap.Add(effect, clip);
                    // Debug.Log("Loaded sound for " + effect + ": " + resourcePath);
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip for " + effect + ": " + resourcePath);
                }
            }
            else
            {
                Debug.LogWarning("No resource path found for " + effect);
            }
        }
    }

    private string GetResourcePath(Enum effect)
    {
        var type = effect.GetType();
        var memInfo = type.GetMember(effect.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(SFX.ResourcePathAttribute), false);
        return attributes.Length > 0 ? ((SFX.ResourcePathAttribute)attributes[0]).resourcePath : null;
    }

    public void StopAllSounds()
    {
        for (int i = activeAudioSources.Count - 1; i >= 0; i--)
        {
            AudioSource source = activeAudioSources[i];
            if (source != null)
            {
                source.Stop();
                GameObject.Destroy(source.gameObject);
            }
        }
        activeAudioSources.Clear(); // Clear the list after stopping all sounds
    }

    public void StopSoundsOfType(Type enumType)
    {
        List<AudioSource> sourcesToRemove = new List<AudioSource>();

        foreach (var entry in soundMap)
        {
            if (entry.Key.GetType() == enumType)
            {
                // Find all audio sources playing this clip and stop them
                for (int i = activeAudioSources.Count - 1; i >= 0; i--)
                {
                    AudioSource source = activeAudioSources[i];
                    if (source != null && source.clip == entry.Value)
                    {
                        source.Stop();
                        GameObject.Destroy(source.gameObject);  // Destroy the object
                        sourcesToRemove.Add(source);
                    }
                }
            }
        }

        // Cleanly remove the sources from the active list
        foreach (AudioSource source in sourcesToRemove)
        {
            activeAudioSources.Remove(source);
        }
    }
}
