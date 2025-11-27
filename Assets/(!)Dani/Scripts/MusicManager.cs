using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static  MusicManager instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(audioClip !=  null)
        {
            instance.audioSource.clip = audioClip;
        }
        if(instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                instance.audioSource.Stop();
            }
            instance.audioSource.Play();
        }
    }

    public static void PauseBackgroundMusic()
    {
        instance.audioSource.Pause();
    }
}
