using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Email_Respond : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button dalisay_consentForm;
    [SerializeField] private Button lit_consentForm;
    [SerializeField] private Button imageFile;

    [Header("UI")]
    [SerializeField] private GameObject replyEmail;
    [SerializeField] private TextMeshProUGUI filename;
    [SerializeField] private bool hasSent;
    [SerializeField] private Button sendReplyButton; // The one that opens the choices
    [SerializeField] private Button backButton; // The one that goes back.
    [SerializeField] private GameObject Task; // The task gameobject.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        replyEmail.SetActive(false);
        hasSent = false;

        Button replyButton = GetComponent<Button>();
        replyButton.onClick.AddListener(() =>
        {
            replyBoxStatus();
        });

        backButton.onClick.AddListener(() =>
        {
            sendReplyButton.interactable = true;
        });

        dalisay_consentForm.onClick.AddListener(() =>
        {
            filename.text = "> Dalisay_consentform.PDF"; 
            sendMessage();
        });
        lit_consentForm.onClick.AddListener(() =>
        { 
            filename.text = "> LIT_ConsentForm.PDF";
            sendMessage();
        });
        imageFile.onClick.AddListener(() =>
        { 
            filename.text = "> 20251120_054050.jpg";
            sendMessage();
        });
    }

    void sendMessage()
    {
        hasSent = true;
        replyEmail.SetActive(true);
        replyBoxStatus();
        TaskDone();
    }

    void replyBoxStatus()
    {
        if (hasSent)
        {
            sendReplyButton.interactable = false;
        } else
        {
            sendReplyButton.interactable = true;
        }
    }

    void TaskDone()
    {
        //Changes the font style of the Notes preview to strikethrough to show that it's done
        TextMeshProUGUI noteTitle = Task.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI noteDesc = Task.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        noteTitle.fontStyle = FontStyles.Strikethrough;
        noteDesc.fontStyle = FontStyles.Strikethrough;
    }
}
