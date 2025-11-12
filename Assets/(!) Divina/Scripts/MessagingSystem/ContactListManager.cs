using UnityEngine; using TMPro; using UnityEngine.UI; using System.Collections.Generic; using System.Linq;
// Almost 400 lines. holy fuck

public class ContactListManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject contactPrefab; // The Contact List prefab
    public RectTransform content; // ScrollView content
    public GameObject messageThreadPanel; // Reference to Message_Thread panel

    [Header("Data")]
    public List<ChatData> contacts; // The list of contacts

    [Header("Settings")]

    private int currentContactIndex = 0;
    private float totalHeight = 0;
    private IMessageRenderer messageRenderer; // Reference to the message renderer
    public int currentChatIndex = -1; // Index of the currently open chat
    private bool isInitialLoad = true; // Flag to track if it's the initial load


    private void Start()
    {
        InitializeContentSize();
        GetMessageRendererReference();
        if (contacts != null && contacts.Count > 0)
        {
            // Set all contacts as unread at start
            for (int i = 0; i < contacts.Count; i++)
            {
                contacts[i].isUnread = true;
            }

            // Instantiate a duplicate for the first contact to avoid using the original prefab
            Transform parent = contactPrefab.transform.parent;
            GameObject firstContactUI = Instantiate(contactPrefab, parent);
            DontDestroyOnLoad(firstContactUI); // Make it persistent across scene loads or parent destruction
            // Set unread indicator to true for duplicated contact
            Transform unreadTransform = firstContactUI.transform.Find("Unread");
            if (unreadTransform != null) unreadTransform.gameObject.SetActive(true);
            contacts[0].contactUI = firstContactUI; // Assign the duplicated GameObject to the first contact
            ConfigureContact(firstContactUI, contacts[0], 0);
            currentContactIndex = 1;
            currentChatIndex = -1; // No chat is open at start, so first contact remains unread
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) AddContact();

        // Background progression for all contacts, but only for current when thread is not active
        for (int i = 0; i < contacts.Count; i++)
        {
            if (contacts[i].isAutoProgressing && (i != currentChatIndex || (messageThreadPanel != null && !messageThreadPanel.activeSelf)))
            {
                contacts[i].autoProgressTimer += Time.deltaTime;
                if (contacts[i].autoProgressTimer >= 1f)
                {
                    contacts[i].autoProgressTimer = 0f;
                    contacts[i].isAutoProgressing = false;
                    if (contacts[i].currentIndex < contacts[i].messageList.Count)
                    {
                        MessageData msg = contacts[i].messageList[contacts[i].currentIndex];
                        // Add to startMessageList
                        if (!contacts[i].startMessageList.Any(s => s.text == msg.text && s.isSender == msg.isSender))
                        {
                            contacts[i].startMessageList.Add(new StartMessageData { text = msg.text, name = msg.name, isSender = msg.isSender });
                        }
                        // Update chat preview
                        contacts[i].chat = ShortenText(msg.text, 40);
                        UpdateContactUI(i);
                        // Advance
                        contacts[i].currentIndex++;
                        if (msg.Choices == null || msg.Choices.Length == 0) contacts[i].isAutoProgressing = true;
                        else contacts[i].isResponding = true;

                        // Set unread if progressing without player opening the chat
                        if (i != currentChatIndex || (messageThreadPanel != null && !messageThreadPanel.activeSelf))
                        {
                            contacts[i].isUnread = true;
                            UpdateUnreadIndicator(i);
                        }
                    }
                }
            }
        }
    }

    private void InitializeContentSize()
    {
        RectTransform originalRT = contactPrefab.GetComponent<RectTransform>();
        totalHeight = originalRT.sizeDelta.y;
        UpdateContentSize();
    }

    private void GetMessageRendererReference()
    {
        if (messageThreadPanel != null)
        {
            messageRenderer = messageThreadPanel.GetComponent<IMessageRenderer>();
            // Set the contact manager reference in the message renderer
            var renderer = messageRenderer as MessageRenderer;
            if (renderer != null) renderer.contactManager = this; 
        }
    } 

    private void AddContact()
    {
        if (currentContactIndex >= contacts.Count) return;

        Transform parent = content.transform;
        GameObject duplicate = Instantiate(contactPrefab, parent);
        DontDestroyOnLoad(duplicate); // dude. idk if removing this would make or break the script at this point... </3

        // Set unread indicator to true for duplicated contact
        Transform unreadTransform = duplicate.transform.Find("Unread");
        if (unreadTransform != null) unreadTransform.gameObject.SetActive(true);

        // Create a deep copy of the ChatData to ensure each contact has its own independent state
        ChatData originalData = contacts[currentContactIndex];
        ChatData newData = new ChatData
        {
            profileImage = originalData.profileImage,
            name = originalData.name,
            phoneNumber = originalData.phoneNumber,
            chat = originalData.chat,
            messageList = DeepCopyMessageList(originalData.messageList),
            startMessageList = DeepCopyStartMessageList(originalData.startMessageList),
            currentIndex = 0, // Reset to 0 for fresh start
            isResponding = false, // Reset state
            isAutoProgressing = false, // Reset state
            autoProgressTimer = 0f, // Reset timer
            isUnread = true, // New contacts should be unread
            contactUI = duplicate // Assign the duplicated GameObject to the new contact
        };
        contacts[currentContactIndex] = newData; // replace with the copy

        // Update the chat preview for the newly duplicated contact
        UpdateChatPreview(currentContactIndex);

        ConfigureContact(duplicate, newData, currentContactIndex);
        PositionContact(duplicate);

        currentContactIndex++;
    }

    //this mf gave me a headAche. THis is just to make sure that it's there SOBS
    private List<MessageData> DeepCopyMessageList(List<MessageData> originalList)
    {
        List<MessageData> newList = new List<MessageData>();
        foreach (var md in originalList)
        {
            MessageData newMd = new MessageData
            {
                text = md.text,
                name = md.name,
                isSender = md.isSender,
                Choices = md.Choices != null ? (string[])md.Choices.Clone() : null,
                linkBox = md.linkBox
            };
            newList.Add(newMd);
        }
        return newList;
    }

    //this one too
    private List<StartMessageData> DeepCopyStartMessageList(List<StartMessageData> originalList)
    {
        List<StartMessageData> newList = new List<StartMessageData>();
        foreach (var smd in originalList)
        {
            StartMessageData newSmd = new StartMessageData
            {
                text = smd.text,
                name = smd.name,
                isSender = smd.isSender
            };
            newList.Add(newSmd);
        }
        return newList;
    }

    private void ConfigureContact(GameObject contact, ChatData data, int chatIndex)
    {
        // Set tag to identify as contact UI
        contact.tag = "ContactUI";

        // Set profile image
        Image profileImg = contact.transform.Find("Profile").GetComponent<Image>();
        if (profileImg != null) profileImg.sprite = data.profileImage;

        // Set name
        TextMeshProUGUI nameTMP = contact.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        if (nameTMP != null) nameTMP.text = data.name;

        // Set chat preview
        TextMeshProUGUI chatTMP = contact.transform.Find("Chat").GetComponent<TextMeshProUGUI>();
        if (chatTMP != null) chatTMP.text = data.chat;

        // Set unread indicator
        Transform unreadTransform = contact.transform.Find("Unread");
        if (unreadTransform != null)
        {
            unreadTransform.gameObject.SetActive(data.isUnread);
        }

        // Add click listener
        Button contactButton = contact.GetComponent<Button>();
        if (contactButton != null) contactButton.onClick.AddListener(() => OnContactSelected(chatIndex));
    }

    private void PositionContact(GameObject contact)
    {
        RectTransform originalRT = contactPrefab.GetComponent<RectTransform>();
        float elementHeight = originalRT.sizeDelta.y;
        RectTransform contactRT = contact.GetComponent<RectTransform>();
        contactRT.anchoredPosition = new Vector2(originalRT.anchoredPosition.x, originalRT.anchoredPosition.y - totalHeight);

        totalHeight += elementHeight;
        UpdateContentSize();
    }

    private void UpdateContentSize()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }

    private void UpdateContactUI(int chatIndex)
    {
        // Only update the UI if the message thread panel is active
        if (messageThreadPanel != null && !messageThreadPanel.activeSelf) return;

        GameObject contactToUpdate = contacts[chatIndex].contactUI;

        if (contactToUpdate != null)
        {
            // Update the chat preview text
            Transform chatTransform = contactToUpdate.transform.Find("Chat");
            if (chatTransform != null)
            {
                TextMeshProUGUI chatTMP = chatTransform.GetComponent<TextMeshProUGUI>();
                if (chatTMP != null)
                {
                    chatTMP.text = contacts[chatIndex].chat;
                    chatTMP.ForceMeshUpdate();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(chatTMP.rectTransform);
                    Canvas.ForceUpdateCanvases();
                }
            }

            // Update unread indicator
            UpdateUnreadIndicator(chatIndex);
        }
    }

    public void UpdateUnreadIndicator(int chatIndex)
    {
        GameObject contactToUpdate = contacts[chatIndex].contactUI;
        if (contactToUpdate != null)
        {
            Transform unreadTransform = contactToUpdate.transform.Find("Unread");
            if (unreadTransform != null)
            {
                unreadTransform.gameObject.SetActive(contacts[chatIndex].isUnread);
            }
        }
    }

    private void OnContactSelected(int chatIndex)
    {
        if (messageRenderer == null || chatIndex < 0 || chatIndex >= contacts.Count) return;

        SaveCurrentChatState();

        // Update the unread indicator for the previous current chat to show it if unread
        if (currentChatIndex >= 0 && currentChatIndex != chatIndex)
        {
            UpdateUnreadIndicator(currentChatIndex);
        }

        currentChatIndex = chatIndex;

        // Mark as read when player opens the chat
        contacts[chatIndex].isUnread = false;
        UpdateUnreadIndicator(chatIndex);

        messageRenderer.ClearMessages();
        ShowMessageThread();

        LoadChatData(chatIndex);
        RestoreChatState(chatIndex);

        // Update the top panel with contact info
        UpdateTopPanel(chatIndex);

        // Force update the contact UI for the selected chat
        UpdateContactUI(chatIndex);

        // Update the latest message on the contact UI after switching back
        contacts[chatIndex].UpdateLatestMessageOnUI();

        // After initial load, set flag to false
        if (isInitialLoad) isInitialLoad = false;
    }

    public void SaveCurrentChatState()
    {
        if (currentChatIndex < 0 || currentChatIndex >= contacts.Count) return;

        // Note: This assumes MessageRenderer has public access to its state
        // In a real implementation, you'd need to modify MessageRenderer to expose state saving/loading
        var renderer = messageRenderer as MessageRenderer;
        if (renderer != null)
        {
            contacts[currentChatIndex] = new ChatData
            {
                profileImage = contacts[currentChatIndex].profileImage,
                name = contacts[currentChatIndex].name,
                phoneNumber = contacts[currentChatIndex].phoneNumber,
                chat = contacts[currentChatIndex].chat,
                messageList = DeepCopyMessageList(renderer.messageList),
                startMessageList = DeepCopyStartMessageList(renderer.startMessageList),
                currentIndex = renderer.currentIndex,
                isResponding = renderer.isResponding,
                isAutoProgressing = renderer.isAutoProgressing,
                autoProgressTimer = renderer.autoProgressTimer,
                contactUI = contacts[currentChatIndex].contactUI
            };

            // Update the chat preview based on the last rendered message
            contacts[currentChatIndex].UpdateChatPreview(renderer.currentIndex, renderer.messageList, renderer.startMessageList);

            // Update the UI for the current contact
            UpdateContactUI(currentChatIndex);

            // Update the chat preview for the current chat index
            UpdateChatPreview(currentChatIndex);
        }
    }

    private void ShowMessageThread()
    {
        if (messageThreadPanel != null) messageThreadPanel.SetActive(true); 
    }

    private void LoadChatData(int chatIndex)
    {
        var renderer = messageRenderer as MessageRenderer;
        if (renderer != null)
        {
            renderer.messageList = DeepCopyMessageList(contacts[chatIndex].messageList);
            renderer.startMessageList = DeepCopyStartMessageList(contacts[chatIndex].startMessageList);
        }
    }

    private void RestoreChatState(int chatIndex)
    {
        var renderer = messageRenderer as MessageRenderer;
        if (renderer == null) return;

        renderer.currentIndex = contacts[chatIndex].currentIndex;
        renderer.isResponding = contacts[chatIndex].isResponding;
        renderer.isAutoProgressing = contacts[chatIndex].isAutoProgressing;
        renderer.autoProgressTimer = contacts[chatIndex].autoProgressTimer;

        // Render start messages (history)
        if (renderer.startMessageList != null)
        {
            foreach (var startMsg in renderer.startMessageList)
            {
                MessageData data = new MessageData { text = startMsg.text, name = startMsg.name, isSender = startMsg.isSender };
                renderer.RenderMessage(data);
            }
        }

        // Update responding state and buttons
        bool hasChoices = renderer.currentIndex < renderer.messageList.Count &&
                          renderer.messageList[renderer.currentIndex].Choices != null &&
                          renderer.messageList[renderer.currentIndex].Choices.Length > 0;

        if (hasChoices)
        {
            bool alreadyRendered = renderer.startMessageList.Any(s => s.text == renderer.messageList[renderer.currentIndex].text && s.isSender == renderer.messageList[renderer.currentIndex].isSender);
            if (!alreadyRendered)
            {
                renderer.RenderMessage(renderer.messageList[renderer.currentIndex], true);
            }
            else
            {
                renderer.RenderChoices(renderer.messageList[renderer.currentIndex]);
            }
            renderer.isResponding = true;
            renderer.SetButtonsInteractable(true);
        }
        else
        {
            renderer.isResponding = false;
            if (renderer.currentIndex == 0 && renderer.messageList.Count > 0)
            {
                renderer.StartMessageProgression();
            }
            else if (renderer.currentIndex < renderer.messageList.Count)
            {
                renderer.isAutoProgressing = true;
                renderer.autoProgressTimer = 0f;
            }
        }
    }

    private void UpdateTopPanel(int chatIndex)
    {
        if (messageThreadPanel == null || chatIndex < 0 || chatIndex >= contacts.Count) return;

        // Find the Top Panel GameObject
        Transform topPanel = messageThreadPanel.transform.Find("Top Panel");
        if (topPanel == null) return;

        // Update Name
        TextMeshProUGUI nameTMP = topPanel.Find("Name")?.GetComponent<TextMeshProUGUI>();
        if (nameTMP != null) nameTMP.text = contacts[chatIndex].name; 

        // Update Phone Number
        TextMeshProUGUI phoneTMP = topPanel.Find("Phone Number")?.GetComponent<TextMeshProUGUI>();
        if (phoneTMP != null) phoneTMP.text = contacts[chatIndex].phoneNumber; 
    }

    public void UpdateChatPreview(int chatIndex)
    {
        // Update the chat preview for the specified chat index
        if (chatIndex >= 0 && chatIndex < contacts.Count)
        { 
            ChatData updatedData = contacts[chatIndex];
            // Use the text from the MessageList at the current index
            if (updatedData.currentIndex < updatedData.messageList.Count) updatedData.chat = ShortenText(updatedData.messageList[updatedData.currentIndex].text, 40); 
            contacts[chatIndex] = updatedData;
            UpdateContactUI(chatIndex); 
        } 
    }

    private string ShortenText(string text, int maxLength)
    {
        if (text.Length <= maxLength) return text;
        else return text.Substring(0, maxLength - 3) + "...";
    }

    public void ReloadMessageThread(int chatIndex)
    {
        if (currentChatIndex == chatIndex && messageRenderer != null)
        {
            LoadChatData(chatIndex);
            messageRenderer.ResetMessaging();
        }
    }
}
