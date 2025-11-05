using System.Collections.Generic; using UnityEngine; using TMPro; using UnityEngine.UI;

/// <summary>
/// From the contacts list btw!!! This is like the shit. The main thang.
/// The main shit. This is where were making the beginning stuff!!!!
/// </summary>

[System.Serializable]
public class ChatData
{
    public Sprite profileImage; // Profile image (MUST BE CIRCLE
    public string name; // Name of contact
    public string phoneNumber; // Phone number of contact (will show up in the message threads)
    public string chat; // The preview chat of the latest one (DW THIS IS AUTOMATIC)
    public List<MessageData> messageList; // Current storytelling kinda thing
    public List<StartMessageData> startMessageList; // The lore before the story starts
    public int currentIndex; // Current progression
    public bool isResponding; // I need to remove this, probably making the progression automatic when the choices[] is null (Update: maybe not)
    public bool isAutoProgressing;
    public float autoProgressTimer; // The timer thing
    public GameObject contactUI; // Reference to the UI GameObject for this contact

    // Updating the chat preview based on the last message
    public void UpdateChatPreview(int currentIndex, List<MessageData> currentMessageList, List<StartMessageData> currentStartMessageList)
    {
        string lastMessageText = GetLastMessageText(currentIndex, currentMessageList, currentStartMessageList);
        if (!string.IsNullOrEmpty(lastMessageText)) chat = ShortenText(lastMessageText, 40);
    }

    // Getting the last message sent
    private string GetLastMessageText(int currentIndex, List<MessageData> messageList, List<StartMessageData> startMessageList)
    {
        if (messageList != null && messageList.Count > 0)
        {
            int lastIndex = Mathf.Min(currentIndex, messageList.Count - 1);
            if (lastIndex >= 0) return messageList[lastIndex].text;
        }

        if (startMessageList != null && startMessageList.Count > 0) return startMessageList[startMessageList.Count - 1].text;

        return string.Empty;
    }

    // Once it reaches 40 char in the latest message text, it would shorten and leave '...'
    private string ShortenText(string text, int maxLength)
    {
        if (text.Length <= maxLength) return text;
        else return text.Substring(0, maxLength - 3) + "...";
    }

    // Function to update the latest message on the contactUI's "Chat" child GameObject
    public void UpdateLatestMessageOnUI()
    {
        if (contactUI == null) return;

        Transform chatTransform = contactUI.transform.Find("Chat");
        if (chatTransform == null) return;

        TextMeshProUGUI chatTMP = chatTransform.GetComponent<TextMeshProUGUI>();
        if (chatTMP == null) return;

        string latestMessage = GetLatestMessageText();
        if (!string.IsNullOrEmpty(latestMessage))
        {
            chatTMP.text = ShortenText(latestMessage, 40);
            chatTMP.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatTMP.rectTransform);
            Canvas.ForceUpdateCanvases();
        }
    }

    // Getting the LATEST message test (not last)
    private string GetLatestMessageText()
    {
        if (messageList != null && messageList.Count > 0)
        {
            int lastIndex = Mathf.Min(currentIndex, messageList.Count - 1);
            if (lastIndex >= 0) return messageList[lastIndex].text;
        }

        if (startMessageList != null && startMessageList.Count > 0)
        {
            return startMessageList[startMessageList.Count - 1].text;
        }

        return string.Empty;
    }
}
