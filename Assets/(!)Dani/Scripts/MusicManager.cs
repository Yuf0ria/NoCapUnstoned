using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static  MusicManager instance;
    private static AudioSource audioSource;
    private static MusicLibrary library;
    [SerializeField] private Slider musicSlider;
    //Input Variables
    public float step = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
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

    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { setVolume(musicSlider.value); });
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            musicSlider.value += step;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            musicSlider.value -= step;
        }
        
    }

    public static void setVolume(float volume)
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
