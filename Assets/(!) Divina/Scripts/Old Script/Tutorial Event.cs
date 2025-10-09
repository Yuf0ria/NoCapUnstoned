using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEvent : MonoBehaviour
{
    //Pure Brute Force

    [Header("Objects")]
    [SerializeField] private GameObject[] notesTask; //
    [SerializeField] private GameObject[] messagingInbox;

    [Header("Objectives")]
    [SerializeField] public int progression;
    [SerializeField] public bool openapp;

    [Header("Items")]
    [SerializeField] private GameObject[] seallie;
    [SerializeField] private TextMeshProUGUI seallie_text;
    [SerializeField] private Button[] buttons;

    [Header("Apps")]
    [SerializeField] public bool isDefault;
    [SerializeField] public bool isNotes;
    [SerializeField] public bool isMessages;
    [SerializeField] public bool isGallery;
    [SerializeField] public bool isSettings;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openapp = true;

        isDefault = true;
        isNotes = false;
        isMessages = false;
        isGallery = false;
        isSettings = false;

        foreach (var task in notesTask)
        {
            task.SetActive(false);
        }
        foreach (var seal in seallie)
        {
            seal.SetActive(false);
        }
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
        foreach (var messaging in messagingInbox)
        {
            messaging.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (progression)
        {
            case 18:
                seallie[0].SetActive(false);
                break;
            case 19: //New task: Message
                seallie[0].SetActive(true);
                messagingInbox[1].SetActive(true);
                messagingInbox[2].SetActive(true);

                seallie_text.text = "Now that you've gotten a grasp on how tasks work, it's time to deal with the other issue: Phishing attacks. Let's try this practice exercise.";

                break;
            default:
                break;
        }

    }

    public void progress()
    {
        progression++;
    }
}
