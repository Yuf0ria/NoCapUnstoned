using UnityEngine;
using UnityEngine.UI;

public class Email_ReadOnly : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button sendReplyButton; // The one that opens the choices
    [SerializeField] private Button backButton; // The one that goes back.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button replyButton = GetComponent<Button>();
        replyButton.onClick.AddListener(() =>
        {
            sendReplyButton.interactable = false;
        });

        backButton.onClick.AddListener(() =>
        {
            sendReplyButton.interactable = true;
        });
    }

    // Update is called once per frame
    void phishing()
    {
        
    }


}
