using Unity.VisualScripting;
using UnityEditor;
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
        //Play when open
        var Menu  = SceneManager.GetSceneByName("Copy_MainMenu").isLoaded;
        if(Menu)
        {
            MusicManager.Play("Menu");
        }
        //stop if scene change
    }

    void SFX()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SoundEffectManager.Play("Click");
        }
    }
}
