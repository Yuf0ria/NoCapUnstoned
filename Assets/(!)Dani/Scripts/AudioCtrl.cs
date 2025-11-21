using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    //Scripts to call

    private static AudioCtrl controller;

    public Event_Manager event_Manager;
    //GameObject

        private void Awake()
    {
        if(controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(event_Manager.enabled == true)
        {
            SoundEffectManager.Play("Start");
        }
    }
}
