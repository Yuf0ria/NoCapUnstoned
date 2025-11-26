using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Security_Research : MonoBehaviour
{
    [SerializeField] private bool isDone;
    [SerializeField] private int clickprogression;

    [Header("Next tasks")]
    [SerializeField] private GameObject interviewSchedule;
    [SerializeField] private GameObject counselingSchedule;

    [Header("Dialogue")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject dialogueGO;
    [SerializeField] private string completionDialogue;

    // Typing coroutine control
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public void addprogression()
    {
        clickprogression++;

        if (clickprogression == 10)
        {
            allSecurityClicked();
        }
    }

    void allSecurityClicked()
    {
        isDone = true;

        //Changes the font style of the Notes preview to strikethrough to show that it's done
        TextMeshProUGUI noteTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI noteDesc = transform.Find("Description").GetComponent<TextMeshProUGUI>();

        noteTitle.fontStyle = FontStyles.Strikethrough;
        noteDesc.fontStyle = FontStyles.Strikethrough;

        // If a dialogue panel is assigned, enable it and show the completion dialogue
        if (dialogueGO != null)
        {
            dialogueGO.SetActive(true);

            // find a button inside dialogueGO and make it close the dialogue when clicked
            Button closeBtn = dialogueGO.GetComponentInChildren<Button>();
            if (closeBtn != null)
            {
                // detach previous listeners to be safe
                closeBtn.onClick.RemoveListener(HideDialogue);
                closeBtn.onClick.AddListener(HideDialogue);
            }
        }

        // Show the typed dialogue message (if dialogueText is assigned)
        ShowDialogue(completionDialogue);

        interviewSchedule.SetActive(true);
        counselingSchedule.SetActive(true);
    }

    private void HideDialogue()
    {
        if (dialogueGO != null)
        {
            dialogueGO.SetActive(false);
        }
    }

    // Public helper to show a dialogue string using a typing animation similar to EventScript.TypeLine
    public void ShowDialogue(string dialogue)
    {
        if (dialogueText == null || string.IsNullOrEmpty(dialogue)) return;

        // If we're currently typing, stop the typing coroutine and finish the line immediately
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueText.text = dialogue;
            isTyping = false;
            return;
        }

        // Start typing the provided dialogue
        typingCoroutine = StartCoroutine(TypeLine(dialogue));
    }

    private IEnumerator TypeLine(string dialogue)
    {
        isTyping = true;
        dialogueText.text = string.Empty;

        int i = 0;
        while (i < dialogue.Length)
        {
            if (dialogue[i] == '<')
            {
                int closeIndex = dialogue.IndexOf('>', i);
                if (closeIndex != -1)
                {
                    dialogueText.text += dialogue.Substring(i, closeIndex - i + 1);
                    i = closeIndex + 1;
                    continue;
                }
                // if malformed tag, fall through and treat as normal char
            }

            dialogueText.text += dialogue[i];
            i++;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
        typingCoroutine = null;
    }
}
