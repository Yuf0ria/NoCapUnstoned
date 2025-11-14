using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Level_Start : MonoBehaviour
{

    [Header("Messages")]
    [SerializeField] bool startOnThread;
    [SerializeField] bool startOnMessages;
    [SerializeField] float delayStartUpTime;
    [SerializeField] App_Basic messageApp;
    [SerializeField] App_Messages contactList;
    [SerializeField] GameObject thread;

    [Header("Masks")]
    [SerializeField] Mask[] MaskList;

    void Start()
    {
        // Enable all Masks
        for (int i = 0; i <= MaskList.Length - 1; i++)
        {
            MaskList[i].enabled = true;
        }

        
        // Open on the Message App
        StartCoroutine(delayStartUp());
    }
    
    IEnumerator delayStartUp()
    {
        if(startOnMessages) messageApp.OpenApp();

        yield return new WaitForSeconds(delayStartUpTime);

        if(startOnThread) contactList.OpenMessageThread(thread);
    }
}
