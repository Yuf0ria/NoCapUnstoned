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
        RenderTimestamp();
        InitializeContentSize();
        RenderStartMessages();
        currentIndex = startMessageList.Count;  // Set currentIndex to the number of start messages to avoid duplication
        StartMessageProgression();
    }

    private void Update()
    {
        HandleAutoProgression();
        HandleManualProgression();
        UpdateTimestampText();
    }

    private void UpdateTimestampText()
    {
        GameObject timestamp = GameObject.FindGameObjectWithTag("Timestamp");
        if (timestamp != null)
        {
            Transform timestampTextTransform = timestamp.transform.Find("Time");
            if (timestampTextTransform == null)
            {
                timestampTextTransform = timestamp.transform.Find("Text");
            }
            if (timestampTextTransform != null)
            {
                TextMeshProUGUI timestampTMP = timestampTextTransform.GetComponent<TextMeshProUGUI>();
                if (timestampTMP != null)
                {
                    timestampTMP.enableCulling = false; // Disable culling to prevent disabling when off-screen
                    timestampTMP.enabled = true; // Ensure the component is enabled
                    timestampTextTransform.gameObject.SetActive(true); // Ensure the gameObject is active
                    if (contactManager != null && contactManager.timeScript != null)
                    {
                        timestampTMP.text = contactManager.timeScript.GetCurrentTimeString();
                        LayoutRebuilder.ForceRebuildLayoutImmediate(timestampTMP.rectTransform);
                    }
                }
            }
        }
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
        if (startMessageList != null && startMessageList.Count > 0)
        {
            // Render all start messages below the timestamp
            for (int i = 0; i < startMessageList.Count; i++)
            {
                MessageData data = new MessageData { text = startMessageList[i].text, name = startMessageList[i].name, isSender = startMessageList[i].isSender };
                RenderMessage(data);
            }
        }
    }

    private void RenderTimestamp()
    {
        if (timestampBox == null) return;

        // Destroy any existing timestamp to ensure we use a fresh duplicate
        GameObject existingTimestamp = GameObject.FindGameObjectWithTag("Timestamp");
        if (existingTimestamp != null)
        {
            Destroy(existingTimestamp);
        }

        // Get the original size of the timestampBox prefab
        Vector2 originalSize = timestampBox.GetComponent<RectTransform>().sizeDelta;

        Transform parent = content; // Place inside the content
        GameObject timestampDuplicate = Instantiate(timestampBox, parent);
        timestampDuplicate.tag = "Timestamp";

        // Set anchors to top of the content
        RectTransform timestampRT = timestampDuplicate.GetComponent<RectTransform>();
        timestampRT.anchorMin = new Vector2(0, 1);
        timestampRT.anchorMax = new Vector2(1, 1);
        timestampRT.pivot = new Vector2(0.5f, 1);
        timestampRT.anchoredPosition = new Vector2(0, 0);
        // Use the original size of the timestampBox
        timestampRT.sizeDelta = originalSize;

        // Update timestamp text using TimeScript
        Transform timestampTextTransform = timestampDuplicate.transform.Find("Time");
        if (timestampTextTransform == null)
        {
            if (timestampDuplicate.GetComponent<TextMeshProUGUI>() != null)
            {
                timestampTextTransform = timestampDuplicate.transform;
            }
            else
            {
                timestampTextTransform = timestampDuplicate.transform.Find("Text");
                if (timestampTextTransform == null)
                {
                    // Try alternative names
                    timestampTextTransform = timestampDuplicate.transform.Find("Text (TMP)");
                    if (timestampTextTransform == null)
                    {
                        timestampTextTransform = timestampDuplicate.transform.Find("Text (UI)");
                        if (timestampTextTransform == null)
                        {
                            timestampTextTransform = timestampDuplicate.transform.Find("TextMeshPro");
                            if (timestampTextTransform == null)
                            {
                                // Find any child with TextMeshProUGUI component
                                foreach (Transform child in timestampDuplicate.transform)
                                {
                                    if (child.GetComponent<TextMeshProUGUI>() != null)
                                    {
                                        timestampTextTransform = child;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Find all TMP components in the timestamp duplicate and its children
        List<TextMeshProUGUI> tmpComponents = new List<TextMeshProUGUI>();
        // Check the duplicate itself
        TextMeshProUGUI tmp = timestampDuplicate.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmpComponents.Add(tmp);
            tmp.enableCulling = false; // Disable culling to prevent disabling when off-screen
            tmp.enabled = true; // Enable the TextMeshProUGUI component
            timestampDuplicate.SetActive(true); // Activate the gameObject
        }
        // Check children
        foreach (Transform child in timestampDuplicate.transform)
        {
            tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmpComponents.Add(tmp);
                tmp.enableCulling = false; // Disable culling to prevent disabling when off-screen
                tmp.enabled = true; // Enable the TextMeshProUGUI component
                child.gameObject.SetActive(true); // Activate the gameObject
            }
        }

        if (tmpComponents.Count > 0 && contactManager != null && contactManager.timeScript != null)
        {
            if (tmpComponents.Count >= 2)
            {
                // Set first TMP to hourText, second to minText
                tmpComponents[0].text = contactManager.timeScript.hourText;
                tmpComponents[1].text = contactManager.timeScript.minText;
                // Set all margins to 0 for both
                tmpComponents[0].margin = Vector4.zero;
                tmpComponents[1].margin = Vector4.zero;
                LayoutRebuilder.ForceRebuildLayoutImmediate(tmpComponents[0].rectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(tmpComponents[1].rectTransform);
            }
            else
            {
                // Fallback to full time string if only one TMP
                tmpComponents[0].text = contactManager.timeScript.GetCurrentTimeString();
                tmpComponents[0].margin = Vector4.zero;
                LayoutRebuilder.ForceRebuildLayoutImmediate(tmpComponents[0].rectTransform);
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
                startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, name = messageList[currentIndex].name, isSender = messageList[currentIndex].isSender });
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
                        startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, name = messageList[currentIndex].name, isSender = messageList[currentIndex].isSender });
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
                startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, name = messageList[currentIndex].name, isSender = messageList[currentIndex].isSender });
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

        // If the message thread is not open, mark the chat as unread
        if (contactManager != null && contactManager.messageThreadPanel != null && !contactManager.messageThreadPanel.activeSelf)
        {
            contactManager.contacts[contactManager.currentChatIndex].isUnread = true;
            contactManager.UpdateUnreadIndicator(contactManager.currentChatIndex);
        }

        if (data.Choices != null && data.Choices.Length > 0 && showChoices) RenderChoices(data);
    }

    private string ParseAndRenderText(GameObject duplicate, MessageData data)
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

        string originalText = data.text;
        MatchCollection matches = Regex.Matches(originalText, @"\[([^\]]+)\]");
        string cleanText = Regex.Replace(originalText, @"\[([^\]]+)\]", "$1");

        textTMP.text = cleanText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        if (matches.Count > 0) CreateHighlights(textTransform, textTMP, matches, originalText, data.linkBox);

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

    private void PositionMessage(GameObject duplicate, MessageData data)
    {
        RectTransform duplicateRT = duplicate.GetComponent<RectTransform>();
        float elementHeight = duplicateRT.sizeDelta.y;

        float yPos = CalculateYPosition(elementHeight, data.isSender);
        float xPos = CalculateXPosition(data, duplicateRT);

        duplicateRT.anchoredPosition = new Vector2(xPos, yPos);

        lastMessageRectTransform = duplicateRT;
        float extraSpacing = data.isSender ? 20f : 0f;
        totalHeight += elementHeight + spacing + extraSpacing;
        UpdateContentSize();
    }

    private float CalculateYPosition(float elementHeight, bool isSender)
    {
        float extraSpacing = 30f;
        if (lastMessageRectTransform == null)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            // Account for timestamp height to position first message below it
            GameObject timestamp = GameObject.FindGameObjectWithTag("Timestamp");
            float timestampHeight = 0f;
            if (timestamp != null)
            {
                RectTransform timestampRT = timestamp.GetComponent<RectTransform>();
                if (timestampRT != null)
                {
                    timestampHeight = timestampRT.sizeDelta.y;
                }
            }
            return originalRT.anchoredPosition.y - originalRT.sizeDelta.y / 2 - spacing - extraSpacing - elementHeight / 2 - timestampHeight;
        }
        else return lastMessageRectTransform.anchoredPosition.y - lastMessageRectTransform.sizeDelta.y / 2 - spacing - extraSpacing - elementHeight / 2;
    }

    private float CalculateXPosition(MessageData data, RectTransform duplicateRT)
    {
        if (data.isSender)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            return originalRT.anchoredPosition.x + originalRT.sizeDelta.x - duplicateRT.sizeDelta.x;
        }
        else
        {
            RectTransform originalReplierRT = replierBox.GetComponent<RectTransform>();
            return originalReplierRT.anchoredPosition.x;
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
        if (!startMessageList.Any(s => s.text == messageWithChoices.text && s.isSender == messageWithChoices.isSender)) startMessageList.Add(new StartMessageData { text = messageWithChoices.text, name = messageWithChoices.name, isSender = messageList[currentIndex].isSender });
        if (!startMessageList.Any(s => s.text == choiceText && s.isSender == false)) startMessageList.Add(new StartMessageData { text = choiceText, name = "", isSender = false });
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
            if (!alreadyRendered) startMessageList.Add(new StartMessageData { text = messageList[currentIndex].text, name = messageList[currentIndex].name, isSender = messageList[currentIndex].isSender });
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
            if (child.gameObject != senderBox && child.gameObject != replierBox && child.gameObject.tag != "ContactUI" && child.gameObject.tag != "Timestamp")
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
        RenderTimestamp();
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
