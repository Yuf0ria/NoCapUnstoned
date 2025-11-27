using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioCtrl : MonoBehaviour
{
    //Scripts to call
    // private static AudioCtrl instance;
    //GameObject
    
    // private void Awake()
    // {
    //     if(instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

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
        //stop if scene change
        var Tutorial = SceneManager.GetSceneByName("Tutorial").isLoaded;
        if(Tutorial)
        {
            MusicManager.Play("tutorial");
        }
        var Chap1 = SceneManager.GetSceneByName("Chapter 1").isLoaded;
        if(Chap1)
        {
            MusicManager.Stop("tutorial");
            MusicManager.Play("Chap1");
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
