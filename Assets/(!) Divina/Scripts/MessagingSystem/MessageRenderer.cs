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

    [Header("UI Elements")]
    public RectTransform content; //ScrollView content
    public GameObject sendButton; //Reference to Send button
    public GameObject openChoicesButton; //Reference to OpenChoices button

    [Header("Paddings n shi")]
    [SerializeField] private float spacing = 50f; //Space between duplicates and choices (increased by +10)
    [SerializeField] private float senderTopPadding = 30f; // Top padding for sender messages (default 30 + extra 30)
    [SerializeField] private float bottomPadding = 10f; // Fixed padding under the last message
    [SerializeField] private float padding = 30f; //Padding on right and bottom sides for text resizing
    [SerializeField] private float dialoguePadding = 20f; //Padding from the left side of the screen for dialogue boxes
    private const float MaxMessageWidth = 555f; //Maximum width for messages
    private const float MaxDialogueWidth = 750f; //Maximum width for messages

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
        // Keep totalHeight as the total of message area (excludes bottom padding).
        totalHeight = originalRT.sizeDelta.y + spacing;
        UpdateContentSize();
    }

    private void RenderStartMessages()
    {
        if (startMessageList != null && startMessageList.Count > 0)
        {
            // Render all start messages
            for (int i = 0; i < startMessageList.Count; i++)
            {
                MessageData data = new MessageData { text = startMessageList[i].text, name = startMessageList[i].name, isSender = startMessageList[i].isSender };
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
                Debug.Log("StartMessageProgression: Processing message at index " + currentIndex + ", Notes: " + (messageList[currentIndex].Notes != null ? messageList[currentIndex].Notes.name : "null"));
                if (messageList[currentIndex].Notes != null)
                {
                    goalTriggered(messageList[currentIndex].Notes);
                }
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
                    Debug.Log("HandleAutoProgression: Processing message at index " + currentIndex + ", Notes: " + (messageList[currentIndex].Notes != null ? messageList[currentIndex].Notes.name : "null"));
                    bool alreadyRendered = startMessageList.Any(s => s.text == messageList[currentIndex].text && s.isSender == messageList[currentIndex].isSender);
                    if (!alreadyRendered)
                    {
                        if (messageList[currentIndex].Notes != null)
                        {
                            goalTriggered(messageList[currentIndex].Notes);
                        }
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
                if (messageList[currentIndex].Notes != null)
                {
                    goalTriggered(messageList[currentIndex].Notes);
                }
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

        textTMP.rectTransform.anchorMin = new Vector2(0, 1);
        textTMP.rectTransform.anchorMax = new Vector2(0, 1);
        textTMP.rectTransform.pivot = new Vector2(0, 1);
        //textTMP.alignment = data.isSender ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.TopRight;
        textTMP.enableWordWrapping = true;

        string cleanText = data.text;

        textTMP.text = cleanText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        float preferredWidth = textTMP.preferredWidth;
        float preferredHeight = textTMP.preferredHeight;
        textTMP.rectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        float clampedWidth = Mathf.Min(preferredWidth + padding, MaxMessageWidth);
        textTMP.rectTransform.sizeDelta = new Vector2(clampedWidth, textTMP.rectTransform.sizeDelta.y);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();
        preferredHeight = textTMP.preferredHeight;
        textTMP.rectTransform.sizeDelta = new Vector2(clampedWidth, preferredHeight);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textTMP.rectTransform);
        textTMP.ForceMeshUpdate();

        ResizeMessageBackground(duplicate, textTMP, clampedWidth, preferredHeight);

        if (data.linkBox != null)
        {
            Debug.Log("Setting up linkBox for message: " + data.text);
            if (textTransform != null)
            {
                Button linkButton = textTransform.GetComponent<Button>();
                if (linkButton == null)
                {
                    linkButton = textTransform.gameObject.AddComponent<Button>();
                    Debug.Log("Added new Button to text");
                }
                else
                {
                    Debug.Log("Using existing Button on text");
                }
                Button linkBoxButton = data.linkBox.GetComponent<Button>();
                if (linkBoxButton != null)
                {
                    Debug.Log("Copying button properties from linkBox");
                    linkButton.interactable = true;
                    linkButton.transition = linkBoxButton.transition;
                    linkButton.colors = linkBoxButton.colors;
                    linkButton.spriteState = linkBoxButton.spriteState;
                    linkButton.animationTriggers = linkBoxButton.animationTriggers;
                    linkButton.navigation = linkBoxButton.navigation;
                    // Set target graphic to the TextMeshPro component
                    TextMeshProUGUI tmp = textTransform.GetComponent<TextMeshProUGUI>();
                    linkButton.targetGraphic = tmp;
                    linkButton.onClick = linkBoxButton.onClick;
                    Debug.Log("Button setup complete, interactable: " + linkButton.interactable);
                }
                else
                {
                    Debug.Log("No Button component found on linkBox prefab");
                }
            }
            else
            {
                Debug.Log("textTransform not found");
            }
        }

        return cleanText;
    }



    private void ResizeMessageBackground(GameObject duplicate, TextMeshProUGUI textTMP, float clampedWidth, float preferredHeight)
    {
        Transform backgroundTransform = duplicate.transform.Find("Background");
        if (backgroundTransform == null) return;

        RectTransform backgroundRT = backgroundTransform.GetComponent<RectTransform>();
        if (backgroundRT == null) return;

        backgroundRT.sizeDelta = new Vector2(MaxMessageWidth + padding, preferredHeight + padding);

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
        // Keep an extra per-message spacing (top padding) for sender boxes.
        // Use senderTopPadding (default 60) so sender messages have the requested top gap.
        float extraSpacing = data.isSender ? senderTopPadding : 0f;
        totalHeight += elementHeight + spacing + extraSpacing;
        UpdateContentSize();
    }

    private float CalculateYPosition(float elementHeight, bool isSender)
    {
        // Top padding for sender messages
        float extraSpacing = isSender ? senderTopPadding : 0f;
        if (lastMessageRectTransform == null)
        {
            RectTransform originalRT = senderBox.GetComponent<RectTransform>();
            // For the very first message placement, do NOT subtract the bottom padding here.
            // bottomPadding is applied once to the content height (UpdateContentSize) so
            // the visual gap under the last message remains constant. Subtracting it
            // here caused the first message to shift and produced an unexpectedly large
            // gap below the last rendered message.
            return originalRT.anchoredPosition.y - originalRT.sizeDelta.y / 2 - spacing - extraSpacing - elementHeight / 2;
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
        float dialogueClampedWidth = Mathf.Min(dialoguePreferredWidth + padding, MaxDialogueWidth);

        // Set the TMP rect width to the clamped width to allow proper wrapping
        dialogueTMP.rectTransform.sizeDelta = new Vector2(dialogueClampedWidth, dialogueTMP.rectTransform.sizeDelta.y);
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueTMP.rectTransform);

        // Get the new preferred height after wrapping
        float newPreferredHeight = dialogueTMP.preferredHeight;
        dialogueRT.sizeDelta = new Vector2(dialogueClampedWidth + padding, newPreferredHeight + padding);
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

    public void goalTriggered(GameObject notes)
    {
        //Changes the font style of the Notes preview to show that it's done
        Debug.Log("goalTriggered called for notes: " + notes.name);
        TextMeshProUGUI noteTitle = notes.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI noteDesc = notes.transform.Find("Description").GetComponent<TextMeshProUGUI>();

        if (noteTitle != null && noteTitle.fontStyle != FontStyles.Strikethrough)
        {
            noteTitle.fontStyle = FontStyles.Strikethrough;
            Debug.Log("Title strikethrough applied");
        }
        else if (noteTitle == null)
        {
            Debug.LogError("Title TextMeshProUGUI not found on notes: " + notes.name);
        }

        if (noteDesc != null && noteDesc.fontStyle != FontStyles.Strikethrough)
        {
            noteDesc.fontStyle = FontStyles.Strikethrough;
            Debug.Log("Description strikethrough applied");
        }
        else if (noteDesc == null)
        {
            Debug.LogError("Description TextMeshProUGUI not found on notes: " + notes.name);
        }
    }

    private void UpdateContentSize()
    {
        // Compute content height from the top of the senderBox down to the bottom
        // of the last rendered message, and add a fixed bottom padding.
        // This prevents the content from accumulating extra empty space as more
        // messages render.
        if (content == null || senderBox == null)
        {
            // fallback to previous behavior
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight + bottomPadding);
            return;
        }

        // If no message has been rendered yet, keep the default small content height
        if (lastMessageRectTransform == null)
        {
            float defaultTop = senderBox.GetComponent<RectTransform>().anchoredPosition.y + senderBox.GetComponent<RectTransform>().sizeDelta.y / 2f;
            float defaultBottom = senderBox.GetComponent<RectTransform>().anchoredPosition.y - senderBox.GetComponent<RectTransform>().sizeDelta.y / 2f;
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Abs(defaultTop - defaultBottom) + bottomPadding);
            return;
        }

        RectTransform topRT = senderBox.GetComponent<RectTransform>();
        float topEdge = topRT.anchoredPosition.y + topRT.sizeDelta.y / 2f;
        float bottomEdge = lastMessageRectTransform.anchoredPosition.y - lastMessageRectTransform.sizeDelta.y / 2f;

        float requiredHeight = Mathf.Abs(topEdge - bottomEdge) + bottomPadding;
        content.sizeDelta = new Vector2(content.sizeDelta.x, requiredHeight);
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
