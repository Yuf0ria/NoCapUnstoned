using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private MusicGroup[] MusicGroups;
    private Dictionary<string, List<AudioClip>> MusicDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        MusicDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (MusicGroup soundEffectGroup in MusicGroups)
        {
            MusicDictionary[soundEffectGroup.name] = soundEffectGroup.clip;
        }
    }

    public AudioClip GetRandomClip(String name)
    {
        if (MusicDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = MusicDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable]
public struct MusicGroup
{
    public string name;
    public List<AudioClip> clip;
}
