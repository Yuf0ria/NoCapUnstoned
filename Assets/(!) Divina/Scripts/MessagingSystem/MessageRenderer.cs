using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// THIS IS FUCKING HELL YALL OMFGGGGGG.....
/// if you read this. im so sorry
/// </summary>

/// YALL PLS DO NOT TOUCH THIS SHIT. EVER. DO NOT. SERIOUSLY.
public class MessageRenderer : MonoBehaviour, IMessageRenderer
{
    [Header("Prefabs")]
    public GameObject senderBox; //Original gameobject  for sender messages
    public GameObject replierBox; //Original gameobject for replier messages
    public GameObject dialogueBox; //Original gameobject for dialogue choices
    public GameObject linkBox; //Original gameobject for hyperlink-like highlights

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
    public List<MessageData> messageList; 
    public List<StartMessageData> startMessageList;
    public ContactListManager contactManager;

    [Header("Status or smthing")]
    public int currentIndex = 0;
    private float totalHeight = 0;
    private RectTransform lastMessageRectTransform;
    public bool isResponding = false; //Flag to control auto-progression
    public bool isAutoProgressing = false; //Flag for auto-progression when respond is false
    public float autoProgressTimer = 0f; //Timer for auto-progression
    private List<GameObject> activeChoices = new List<GameObject>(); //List to track active choice buttons
    public string lastRenderedText = ""; //Last rendered message text for chat preview

    private void Start()
    {
        InitializeButtons();
        DisableOriginalDialogueBox();
        InitializeContentSize();
        RenderStartMessages();
        currentIndex = startMessageList.Count;  // Set currentIndex to the number of start messages to avoid duplication
        StartMessageProgression();
    }

    private void Update()
    {
        HandleAutoProgression();
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

    private void RenderStartMessages()
    {
        if (startMessageList != null)
        {
            foreach (var startMsg in startMessageList)
            {
                MessageData data = new MessageData { text = startMsg.text, isSender = startMsg.isSender };
                RenderMessage(data);
            }
        }
    }

    public void StartMessageProgression()
    {
        if (messageList != null && messageList.Count > 0 && currentIndex < messageList.Count)
        {
            bool alreadyRendered = startMessageList.Any(s => s.text == messageList[currentIndex].text && s.isSender == messageList[currentIndex].isSender);
            if (!alreadyRendered)
            {
                RenderMessage(messageList[currentIndex]);
                startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, isSender = messageList[currentIndex].isSender });
            }
            if (messageList[currentIndex].Choices == null || messageList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
                isAutoProgressing = true;
            }
            else
            {
                isResponding = true;
                SetButtonsInteractable(true);
            }
        } 
    }

    private void HandleAutoProgression()
    {
        if (isAutoProgressing)
        {
            autoProgressTimer += Time.deltaTime;
            if (autoProgressTimer >= 1f)
            {
                autoProgressTimer = 0f;
                isAutoProgressing = false;
                if (currentIndex < messageList.Count)
                {
                    bool alreadyRendered = startMessageList.Any(s => s.text == messageList[currentIndex].text && s.isSender == messageList[currentIndex].isSender);
                    if (!alreadyRendered)
                    {
                        RenderMessage(messageList[currentIndex]);
                        startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, isSender = messageList[currentIndex].isSender });
                    }
                    if (messageList[currentIndex].Choices == null || messageList[currentIndex].Choices.Length == 0)
                    {
                        currentIndex++;
                        if (contactManager != null) contactManager.contacts[contactManager.currentChatIndex].currentIndex = currentIndex;
                        isAutoProgressing = true;
                    }
                    else
                    {
                        isResponding = true;
                        SetButtonsInteractable(true);
                    }
                    // Update chat preview after rendering a message
                    if (contactManager != null)
                    {
                        contactManager.UpdateChatPreview(contactManager.currentChatIndex);
                        // Update the latest message on the contact UI
                        contactManager.contacts[contactManager.currentChatIndex].UpdateLatestMessageOnUI();
                    }
                }
            }
        }
    }

    private void HandleManualProgression()
    {
        if (!isResponding && Input.GetKeyDown(KeyCode.Space) && currentIndex < messageList.Count)
        {
            bool alreadyRendered = startMessageList.Any(s => s.text == messageList[currentIndex].text && s.isSender == messageList[currentIndex].isSender);
            if (!alreadyRendered)
            {
                RenderMessage(messageList[currentIndex]);
                startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, isSender = messageList[currentIndex].isSender });
            }
            // Update chat preview after rendering a message
            if (contactManager != null)
            {
                contactManager.UpdateChatPreview(contactManager.currentChatIndex);
                // Update the latest message on the contact UI
                contactManager.contacts[contactManager.currentChatIndex].UpdateLatestMessageOnUI();
            }
            if (messageList[currentIndex].Choices == null || messageList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
                if (contactManager != null) contactManager.contacts[contactManager.currentChatIndex].currentIndex = currentIndex;
                isAutoProgressing = true;
            }
            else
            {
                isResponding = true;
                SetButtonsInteractable(true);
            }
        }
    }

    public void RenderMessage(MessageData data, bool showChoices = true)
    {
        GameObject prefabToUse = data.isSender ? senderBox : replierBox;
        Transform parent = senderBox.transform.parent;
        GameObject duplicate = Instantiate(prefabToUse, parent);

        string cleanText = ParseAndRenderText(duplicate, data);

        if (string.IsNullOrEmpty(cleanText))
        {
            Destroy(duplicate);
            return;
        }

        // Update last rendered text for chat preview
        lastRenderedText = cleanText;

        PositionMessage(duplicate, data);

        if (data.Choices != null && data.Choices.Length > 0 && showChoices) RenderChoices(data); 
    }

    private string ParseAndRenderText(GameObject duplicate, MessageData data)
    {
        Transform textTransform = duplicate.transform.Find("Text");
        if (textTransform == null) return "";

        TextMeshProUGUI textTMP = textTransform.GetComponent<TextMeshProUGUI>();
        if (textTMP == null) return "";

        string originalText = data.text;
        MatchCollection matches = Regex.Matches(originalText, @"\[([^\]]+)\]");
        string cleanText = Regex.Replace(originalText, @"\[([^\]]+)\]", "$1");

        textTMP.text = cleanText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        if (matches.Count > 0) CreateHighlights(textTransform, textTMP, matches, originalText);

        ResizeMessageBackground(duplicate, textTMP);

        return cleanText;
    }

    private void CreateHighlights(Transform textTransform, TextMeshProUGUI textTMP, MatchCollection matches, string originalText)
    {
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

            GameObject highlightDuplicate = Instantiate(linkBox, textTransform);
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

    private void PositionMessage(GameObject duplicate, MessageData data)
    {
        RectTransform duplicateRT = duplicate.GetComponent<RectTransform>();
        float elementHeight = duplicateRT.sizeDelta.y;

        float yPos = CalculateYPosition(elementHeight);
        float xPos = CalculateXPosition(data, duplicateRT);

        duplicateRT.anchoredPosition = new Vector2(xPos, yPos);

        lastMessageRectTransform = duplicateRT;
        totalHeight += elementHeight + spacing;
        UpdateContentSize();
    }

    private float CalculateYPosition(float elementHeight)
    {
        if (lastMessageRectTransform == null)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            return originalRT.anchoredPosition.y - originalRT.sizeDelta.y / 2 - spacing - elementHeight / 2;
        }
        else return lastMessageRectTransform.anchoredPosition.y - lastMessageRectTransform.sizeDelta.y / 2 - spacing - elementHeight / 2;
    }

    private float CalculateXPosition(MessageData data, RectTransform duplicateRT)
    {
        if (data.isSender)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            return originalRT.anchoredPosition.x;
        }
        else
        {
            RectTransform originalReplierRT = replierBox.GetComponent<RectTransform>();
            return originalReplierRT.anchoredPosition.x + originalReplierRT.sizeDelta.x - duplicateRT.sizeDelta.x;
        }
    }

    public void RenderChoices(MessageData data)
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
        MessageData messageWithChoices = messageList[currentIndex];
        MessageData choiceData = new MessageData { text = choiceText, isSender = false, Choices = null };
        // Add to history only if not already present
        if (!startMessageList.Any(s => s.text == messageWithChoices.text && s.isSender == messageWithChoices.isSender)) startMessageList.Add(new StartMessageData { text = messageWithChoices.text, isSender = messageList[currentIndex].isSender });
        if (!startMessageList.Any(s => s.text == choiceText && s.isSender == false)) startMessageList.Add(new StartMessageData { text = choiceText, isSender = false });
        // Advance to the next message
        currentIndex += 1;
        RenderMessage(choiceData);
        if (contactManager != null) contactManager.contacts[contactManager.currentChatIndex].currentIndex = currentIndex;
        // Update the latest message on the contact UI
        if (contactManager != null) contactManager.contacts[contactManager.currentChatIndex].UpdateLatestMessageOnUI();
        // Update chat preview after choice selection
        if (contactManager != null) contactManager.UpdateChatPreview(contactManager.currentChatIndex);
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

        if (currentIndex < messageList.Count)
        {
            bool alreadyRendered = startMessageList.Any(s => s.text == messageList[currentIndex].text && s.isSender == messageList[currentIndex].isSender);
            if (!alreadyRendered) startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, isSender = messageList[currentIndex].isSender });
            RenderMessage(messageList[currentIndex]);
            // Update chat preview after rendering a message
            if (contactManager != null)
            { 
                contactManager.UpdateChatPreview(contactManager.currentChatIndex);
                // Update the latest message on the contact UI
                contactManager.contacts[contactManager.currentChatIndex].UpdateLatestMessageOnUI();
            } 
            if (messageList[currentIndex].Choices == null || messageList[currentIndex].Choices.Length == 0)
            {
                currentIndex++;
                if (contactManager != null) contactManager.contacts[contactManager.currentChatIndex].currentIndex = currentIndex;
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

    public void ClearMessages()
    {
        ClearMessageUI();
        ResetStateVariables();
    }

    private void ClearMessageUI()
    {
        foreach (Transform child in content)
        { 
            if (child.gameObject != senderBox && child.gameObject != replierBox && child.gameObject.tag != "ContactUI")
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
        autoProgressTimer = 0f;
    }

    public void ResetMessaging()
    {
        ResetStateVariables();
        totalHeight = senderBox.GetComponent<RectTransform>().sizeDelta.y;
        lastMessageRectTransform = null;

        // Clear the UI to prevent duplication and ensure correct order
        ClearMessageUI();
        RenderStartMessages();

        // Resume from the last interaction
        if (contactManager != null)
        {
            currentIndex = contactManager.contacts[contactManager.currentChatIndex].currentIndex;
            if (currentIndex < messageList.Count && currentIndex > 0)
            {
                if (messageList[currentIndex].Choices == null || messageList[currentIndex].Choices.Length == 0)
                {
                    isAutoProgressing = true;
                }
                else
                {
                    RenderMessage(messageList[currentIndex]);
                    isResponding = true;
                    SetButtonsInteractable(true);
                }
            }
        }
    }
}
