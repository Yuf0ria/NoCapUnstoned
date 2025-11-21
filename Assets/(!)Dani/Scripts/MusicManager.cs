using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static  MusicManager MusicInstance;
    private static AudioSource audioSource;
    private static MusicLibrary library;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (MusicInstance == null)
        {
            MusicInstance = this;
            audioSource = GetComponent<AudioSource>();
            library = GetComponent<MusicLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = library.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

     public static void Stop(string soundName)
    {
        AudioClip audioClip = library.GetRandomClip(soundName);
        if (audioClip == null)
        {
            audioSource.Stop();
        }
    }

    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(audioClip !=  null)
        {
            audioSource.clip = audioClip;
        }
        if(audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }
}
