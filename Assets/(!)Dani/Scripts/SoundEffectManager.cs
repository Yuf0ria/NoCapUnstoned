using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private static AudioSource AudioSource;
    private static SoundEffectLibrary library;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        library = GetComponent<SoundEffectLibrary>();
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = library.GetRandomClip(soundName);
        if(audioClip != null )
        {
            AudioSource.PlayOneShot(audioClip);
        }
    }
    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged();});
    }

    public static void SetVolume(float volume)
    {
        AudioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
