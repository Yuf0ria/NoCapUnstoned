using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Email renderer for handling email display and interactions.
/// </summary>
public class EmailRenderer : MonoBehaviour, IEmailRenderer
{
    [Header("Prefabs")]
    public GameObject senderBox; //Original gameobject for sender emails
    public GameObject replierBox; //Original gameobject for replier emails
    public GameObject dialogueBox; //Original gameobject for dialogue choices
    public GameObject timestampBox; //Original gameobject for timestamps

    [Header("UI Elements")]
    public RectTransform content; //ScrollView content
    public GameObject sendButton; //Reference to Send button
    public GameObject openChoicesButton; //Reference to OpenChoices button

    [Header("Paddings n shi")]
    [SerializeField] private float spacing = 20f; //Space between duplicates and choices                                 //ALSO BRUH I COULD HAVE PUT ALL OF THESE INTO ONE </3
    [SerializeField] private float padding = 20f; //Padding on right and bottom sides for text resizing
    [SerializeField] private float dialoguePadding = 20f; //Padding from the left side of the screen for dialogue boxes
    private const float MaxMessageWidth = 555f; //Maximum width for messages

    [Header("Data")]
    public List<EmailData> emailList;
    public List<StartEmailData> startEmailList;
    public InboxListManager inboxManager;

    [Header("Status or smthing")]
    public int currentIndex = 0;
    private float totalHeight = 0;
    private RectTransform lastMessageRectTransform;
    public bool isResponding = false; //Flag to control auto-progression
    private bool isAutoProgressing = false; //Flag to control auto-progression
    private List<GameObject> activeChoices = new List<GameObject>(); //List to track active choice buttons
    public string lastRenderedText = ""; //Last rendered message text for chat preview

    private void Start()
    {
        InitializeButtons();
        DisableOriginalDialogueBox();
        InitializeContentSize();
        RenderStartEmails();
        currentIndex = startEmailList.Count;  // Set currentIndex to the number of start emails to avoid duplication
        StartEmailProgression();
    }

    private void Update()
    {
        HandleManualProgression();
    }



    private void InitializeButtons()
    {
        GameObject replyBoxObj = GameObject.FindGameObjectWithTag("Reply Box");
        if (replyBoxObj != null)
        {
            sendButton = replyBoxObj.transform.Find("Message Box/Send").gameObject;
            openChoicesButton = replyBoxObj.transform.Find("Message Box/OpenChoices").gameObject;
            SetButtonsInteractable(false);
        }
    }

    private void DisableOriginalDialogueBox()
    {
        if (dialogueBox != null) dialogueBox.SetActive(false);
    }

    private void InitializeContentSize()
    {
        RectTransform originalRT = senderBox.GetComponent<RectTransform>();
        totalHeight = originalRT.sizeDelta.y + spacing;
        UpdateContentSize();
    }

    private void RenderStartEmails()
    {
        if (startEmailList != null && startEmailList.Count > 0)
        {
            // Render all start emails
            for (int i = 0; i < startEmailList.Count; i++)
            {
                EmailData data = new EmailData { emailText = startEmailList[i].text, emailTitle = startEmailList[i].title, name = startEmailList[i].name, profile = startEmailList[i].profile };
                RenderEmail(data);
            }
        }
    }



    public void StartEmailProgression()
    {
        if (emailList != null && emailList.Count > 0 && currentIndex < emailList.Count)
        {
            bool alreadyRendered = startEmailList.Any(s => s.text == emailList[currentIndex].emailText && s.title == emailList[currentIndex].emailTitle);
            if (!alreadyRendered)
            {
                RenderEmail(emailList[currentIndex]);
                startEmailList.Add(new StartEmailData { text = emailList[currentIndex].emailText, title = emailList[currentIndex].emailTitle, name = emailList[currentIndex].name, profile = emailList[currentIndex].profile });
            }
            if (emailList[currentIndex].Choices == null || emailList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
            }
            else
            {
                isResponding = true;
                SetButtonsInteractable(true);
            }
        }
    }



    private void HandleManualProgression()
    {
        if (!isResponding && Input.GetKeyDown(KeyCode.Space) && currentIndex < emailList.Count)
        {
            bool alreadyRendered = startEmailList.Any(s => s.text == emailList[currentIndex].emailText && s.title == emailList[currentIndex].emailTitle);
            if (!alreadyRendered)
            {
                RenderEmail(emailList[currentIndex]);
                startEmailList.Add(new StartEmailData { text = emailList[currentIndex].emailText, title = emailList[currentIndex].emailTitle, name = emailList[currentIndex].name, profile = emailList[currentIndex].profile });
            }
            // Update email preview after rendering a message
            if (inboxManager != null)
            {
                inboxManager.UpdateEmailPreview(inboxManager.currentEmailIndex);
                // Update the latest email on the inbox UI
                inboxManager.inbox[inboxManager.currentEmailIndex].UpdateLatestEmailOnUI();
            }
            if (emailList[currentIndex].Choices == null || emailList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
                if (inboxManager != null) inboxManager.inbox[inboxManager.currentEmailIndex].currentIndex = currentIndex;
            }
            else
            {
                isResponding = true;
                SetButtonsInteractable(true);
            }
        }
    }

    public void RenderEmail(EmailData data, bool showChoices = true)
    {
        GameObject duplicate = Instantiate(senderBox, content);

        string cleanText = ParseAndRenderText(duplicate, data);

        if (string.IsNullOrEmpty(cleanText))
        {
            Destroy(duplicate);
            return;
        }

        // Update last rendered text for chat preview
        lastRenderedText = cleanText;

        PositionEmail(duplicate, data);

        // If the email thread is not open, mark the email as unread
        if (inboxManager != null && inboxManager.emailThreadPanel != null && !inboxManager.emailThreadPanel.activeSelf)
        {
            inboxManager.inbox[inboxManager.currentEmailIndex].isUnread = true;
            inboxManager.UpdateUnreadIndicator(inboxManager.currentEmailIndex);
        }

        if (data.Choices != null && data.Choices.Length > 0 && showChoices) RenderChoices(data);
    }

    private string ParseAndRenderText(GameObject duplicate, EmailData data)
    {
        // Set the name if available
        Transform nameTransform = duplicate.transform.Find("Name");
        if (nameTransform != null)
        {
            TextMeshProUGUI nameTMP = nameTransform.GetComponent<TextMeshProUGUI>();
            if (nameTMP != null)
            {
                nameTMP.text = data.name ?? "";
                LayoutRebuilder.ForceRebuildLayoutImmediate(nameTMP.rectTransform);
            }
        }

        Transform textTransform = duplicate.transform.Find("Text");
        if (textTransform == null) return "";

        TextMeshProUGUI textTMP = textTransform.GetComponent<TextMeshProUGUI>();
        if (textTMP == null) return "";

        string originalText = data.emailText;
        MatchCollection matches = Regex.Matches(originalText, @"\[([^\]]+)\]");
        string cleanText = Regex.Replace(originalText, @"\[([^\]]+)\]", "$1");

        textTMP.text = cleanText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        if (matches.Count > 0 && data.linkBox != null) CreateHighlights(textTransform, textTMP, matches, originalText, data.linkBox);

        ResizeMessageBackground(duplicate, textTMP);

        return cleanText;
    }

    private void CreateHighlights(Transform textTransform, TextMeshProUGUI textTMP, MatchCollection matches, string originalText, GameObject messageLinkBox)
    {
        if (messageLinkBox == null) return; // Skip if no linkBox provided for this message

        foreach (Match match in matches)
        {
            int bracketCountBefore = Regex.Matches(originalText.Substring(0, match.Index), @"\[").Count;
            int startIndex = match.Groups[1].Index - bracketCountBefore - 1;
            int length = match.Groups[1].Length;

            if (startIndex + length > textTMP.textInfo.characterCount) continue;

            TMP_CharacterInfo charInfoStart = textTMP.textInfo.characterInfo[startIndex];
            TMP_CharacterInfo charInfoEnd = textTMP.textInfo.characterInfo[startIndex + length - 1];

            float startX = charInfoStart.bottomLeft.x;
            float endX = charInfoEnd.bottomRight.x;
            float width = endX - startX;
            float height = charInfoStart.ascender - charInfoStart.descender;

            GameObject highlightDuplicate = Instantiate(messageLinkBox, textTransform);
            HideHighlightText(highlightDuplicate);

            RectTransform highlightRT = highlightDuplicate.GetComponent<RectTransform>();
            highlightRT.sizeDelta = new Vector2(width, height);
            highlightRT.localPosition = new Vector3(startX + width / 2, charInfoStart.descender + height / 2, 0);
        }
    }

    private void HideHighlightText(GameObject highlight)
    {
        Transform highlightTextTransform = highlight.transform.Find("Text");
        if (highlightTextTransform != null) highlightTextTransform.gameObject.SetActive(false);
    }

    private void ResizeMessageBackground(GameObject duplicate, TextMeshProUGUI textTMP)
    {
        Transform backgroundTransform = duplicate.transform.Find("Background");
        if (backgroundTransform == null) return;

        RectTransform backgroundRT = backgroundTransform.GetComponent<RectTransform>();
        if (backgroundRT == null) return;

        float preferredWidth = textTMP.preferredWidth;
        float preferredHeight = textTMP.preferredHeight;
        float clampedWidth = Mathf.Min(preferredWidth + padding, MaxMessageWidth);
        backgroundRT.sizeDelta = new Vector2(clampedWidth + padding, preferredHeight + padding);

        RectTransform duplicateRT = duplicate.GetComponent<RectTransform>();
        duplicateRT.sizeDelta = new Vector2(duplicateRT.sizeDelta.x, backgroundRT.sizeDelta.y);
    }

    private void PositionEmail(GameObject duplicate, EmailData data)
    {
        RectTransform duplicateRT = duplicate.GetComponent<RectTransform>();
        float elementHeight = duplicateRT.sizeDelta.y;

        float yPos = CalculateYPosition(elementHeight, false); // Emails are not sender/replier, so false
        float xPos = 0; // Center for emails

        duplicateRT.anchoredPosition = new Vector2(xPos, yPos);

        lastMessageRectTransform = duplicateRT;
        float extraSpacing = 0f; // No extra spacing for emails
        totalHeight += elementHeight + spacing + extraSpacing;
        UpdateContentSize();
    }

    private float CalculateYPosition(float elementHeight, bool isSender)
    {
        float extraSpacing = 30f;
        if (lastMessageRectTransform == null)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            return originalRT.anchoredPosition.y - originalRT.sizeDelta.y / 2 - spacing - extraSpacing - elementHeight / 2;
        }
        else return lastMessageRectTransform.anchoredPosition.y - lastMessageRectTransform.sizeDelta.y / 2 - spacing - extraSpacing - elementHeight / 2;
    }

    public void RenderChoices(EmailData data)
    {
        Transform replyBoxParent = dialogueBox.transform.parent;
        RectTransform lastChoiceRT = null;

        for (int i = 0; i < data.Choices.Length; i++)
        {
            GameObject dialogueDuplicate = Instantiate(dialogueBox, replyBoxParent);
            dialogueDuplicate.SetActive(true);

            ConfigureChoiceText(dialogueDuplicate, data.Choices[i]);
            PositionChoice(dialogueDuplicate, ref lastChoiceRT);
            AddChoiceListener(dialogueDuplicate, data.Choices[i]);

            activeChoices.Add(dialogueDuplicate);
        }
    }

    private void ConfigureChoiceText(GameObject dialogueDuplicate, string choiceText)
    {
        Transform dialogueTextTransform = dialogueDuplicate.transform.Find("Text (TMP)");
        if (dialogueTextTransform == null) return;

        TextMeshProUGUI dialogueTMP = dialogueTextTransform.GetComponent<TextMeshProUGUI>();
        if (dialogueTMP == null) return;

        dialogueTMP.text = choiceText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueTMP.rectTransform);

        RectTransform dialogueRT = dialogueDuplicate.GetComponent<RectTransform>();
        float dialoguePreferredWidth = dialogueTMP.preferredWidth;
        float dialoguePreferredHeight = dialogueTMP.preferredHeight;
        float dialogueClampedWidth = Mathf.Min(dialoguePreferredWidth + padding, MaxMessageWidth);
        dialogueRT.sizeDelta = new Vector2(dialogueClampedWidth + padding, dialoguePreferredHeight + padding);
    }

    private void PositionChoice(GameObject dialogueDuplicate, ref RectTransform lastChoiceRT)
    {
        RectTransform dialogueRT = dialogueDuplicate.GetComponent<RectTransform>();
        float dialogueElementHeight = dialogueRT.sizeDelta.y;

        float dialogueYPos;
        RectTransform originalDialogueRT = dialogueBox.GetComponent<RectTransform>();
        if (lastChoiceRT != null) dialogueYPos = lastChoiceRT.anchoredPosition.y - lastChoiceRT.sizeDelta.y / 2 - spacing - dialogueElementHeight / 2;
        else dialogueYPos = originalDialogueRT.anchoredPosition.y;

        float dialogueXPos = dialoguePadding + dialogueRT.sizeDelta.x / 2;
        dialogueRT.anchoredPosition = new Vector2(dialogueXPos, dialogueYPos);

        lastChoiceRT = dialogueRT;
        totalHeight += dialogueElementHeight + spacing;
        UpdateContentSize();
    }

    private void AddChoiceListener(GameObject dialogueDuplicate, string choiceText)
    {
        Button choiceButton = dialogueDuplicate.GetComponent<Button>();
        if (choiceButton != null)
        {
            choiceButton.onClick.RemoveAllListeners();
            choiceButton.onClick.AddListener(() => OnChoiceSelected(choiceText));
        }
    }

    private void OnChoiceSelected(string choiceText)
    {
        SetButtonsInteractable(false);
        HideActiveChoices();
        EmailData emailWithChoices = emailList[currentIndex];
        EmailData choiceData = new EmailData { emailText = choiceText, emailTitle = "", name = "", Choices = null };
        // Add to history only if not already present
        if (!startEmailList.Any(s => s.text == emailWithChoices.emailText && s.title == emailWithChoices.emailTitle)) startEmailList.Add(new StartEmailData { text = emailWithChoices.emailText, title = emailWithChoices.emailTitle, name = emailWithChoices.name, profile = emailWithChoices.profile });
        if (!startEmailList.Any(s => s.text == choiceText && s.title == "")) startEmailList.Add(new StartEmailData { text = choiceText, title = "", name = "", profile = null });
        // Advance to the next message
        currentIndex += 1;
        RenderEmail(choiceData);
        if (inboxManager != null) inboxManager.inbox[inboxManager.currentEmailIndex].currentIndex = currentIndex;
        // Update the latest email on the inbox UI
        if (inboxManager != null) inboxManager.inbox[inboxManager.currentEmailIndex].UpdateLatestEmailOnUI();
        // Update email preview after choice selection
        if (inboxManager != null) inboxManager.UpdateEmailPreview(inboxManager.currentEmailIndex);
        StartCoroutine(ContinueAfterChoiceSelection());
    }

    private void HideActiveChoices()
    {
        foreach (GameObject choice in activeChoices) if (choice != null) choice.SetActive(false);
        activeChoices.Clear();
    }

    private IEnumerator ContinueAfterChoiceSelection()
    {
        yield return new WaitForSeconds(1.5f);

        if (currentIndex < emailList.Count)
        {
            bool alreadyRendered = startEmailList.Any(s => s.text == emailList[currentIndex].emailText && s.title == emailList[currentIndex].emailTitle);
            if (!alreadyRendered) startEmailList.Add(new StartEmailData { text = emailList[currentIndex].emailText, title = emailList[currentIndex].emailTitle, name = emailList[currentIndex].name, profile = emailList[currentIndex].profile });
            RenderEmail(emailList[currentIndex]);
            // Update email preview after rendering a message
            if (inboxManager != null)
            {
                inboxManager.UpdateEmailPreview(inboxManager.currentEmailIndex);
                // Update the latest email on the inbox UI
                inboxManager.inbox[inboxManager.currentEmailIndex].UpdateLatestEmailOnUI();
            }
            if (emailList[currentIndex].Choices == null || emailList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
                if (inboxManager != null) inboxManager.inbox[inboxManager.currentEmailIndex].currentIndex = currentIndex;
                isAutoProgressing = true;
            }
            else
            {
                isResponding = true;
                SetButtonsInteractable(true);
            }
        }
    }

    public void SetButtonsInteractable(bool interactable)
    {
        if (sendButton != null) sendButton.GetComponent<Button>().interactable = interactable;
        if (openChoicesButton != null) openChoicesButton.GetComponent<Button>().interactable = interactable;
    }

    private void UpdateContentSize()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }

    public void ClearEmail()
    {
        ClearMessageUI();
        ResetStateVariables();
    }

    private void ClearMessageUI()
    {
        foreach (Transform child in content)
        {
            if (child.gameObject != senderBox && child.gameObject.tag != "ContactUI" && child.gameObject.tag != "Timestamp")
            { Destroy(child.gameObject); }
        }

        HideActiveChoices();
        totalHeight = senderBox.GetComponent<RectTransform>().sizeDelta.y + spacing;
        lastMessageRectTransform = null;
        UpdateContentSize();
    }

    private void ResetStateVariables()
    {
        currentIndex = 0;
        isResponding = false;
        isAutoProgressing = false;
    }

    public void ResetEmail()
    {
        ResetStateVariables();
        totalHeight = senderBox.GetComponent<RectTransform>().sizeDelta.y;
        lastMessageRectTransform = null;

        // Clear the UI to prevent duplication and ensure correct order
        ClearMessageUI();
        RenderStartEmails();

        // Resume from the last interaction
        if (inboxManager != null)
        {
            currentIndex = inboxManager.inbox[inboxManager.currentEmailIndex].currentIndex;
            if (currentIndex < emailList.Count && currentIndex > 0)
            {
                if (emailList[currentIndex].Choices == null || emailList[currentIndex].Choices.Length == 0)
                {
                    // No auto-progression
                }
                else
                {
                    RenderEmail(emailList[currentIndex]);
                    isResponding = true;
                    SetButtonsInteractable(true);
                }
            }
        }
    }
}
