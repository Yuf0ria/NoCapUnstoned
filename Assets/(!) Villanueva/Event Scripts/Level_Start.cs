using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Level_Start : MonoBehaviour
{

    [Header("Messages")]
    [SerializeField] bool startOnThread;
    [SerializeField] float delayStartUpTime;
    [SerializeField] App_Basic messageApp;
    [SerializeField] App_Messages contactList;
    [SerializeField] GameObject thread;

    [Header("Masks")]
    [SerializeField] Mask[] MaskList;

    void Start()
    {
        // Open on the Message App
        messageApp.OpenApp();
        if (startOnThread)
        {
            StartCoroutine(delayStartUp());
        }


        // Enable all Masks
        for (int i = 0; i <= MaskList.Length - 1; i++)
        {
            MaskList[i].enabled = true;
        }
    }
    
    IEnumerator delayStartUp()
    {
        yield return new WaitForSeconds(delayStartUpTime);
        contactList.OpenMessageThread(thread);
    }
}
