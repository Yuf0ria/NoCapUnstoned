using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioCtrl : MonoBehaviour
{
    //Scripts to call
    private static AudioCtrl instance;
    //GameObject
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MainMusic();
    }

    void Update()
    {
        //SFX
        SFX();
    }


    void MainMusic()
    {
        //& a bit of sfx. why? becuase it loops if you put on update and it doesn't have a key
        //Play when open
        //Change this menu later
        var Menu  = SceneManager.GetSceneByName("Copy_MainMenu").isLoaded;
        if(Menu)
        {
            MusicManager.Play("Menu");
            SoundEffectManager.Play("Start");
        }
        //stop if scene change
        var Tutorial = SceneManager.GetSceneByName("Tutorial").isLoaded;
        if(Tutorial)
        {
            MusicManager.Stop("Menu");
            MusicManager.Play("tutorial");
        }

    }

    void SFX()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SoundEffectManager.Play("Click");
        }
    }
}
