using System.Collections.Generic; using UnityEngine; using TMPro; using UnityEngine.UI;

/// <summary>
/// From the contacts list btw!!! This is like the shit. The main thang.
/// The main shit. This is where were making the beginning stuff!!!!
/// </summary>

[System.Serializable]
public class InboxData
{
    public Sprite profileImage; // Profile image (MUST BE CIRCLE
    public string name; // Name of contact
    public string emailTitle; // Phone number of contact (will show up in the message threads)
    public string emailPreview; // The preview chat of the latest one (DW THIS IS AUTOMATIC)
    public List<EmailData> emailList; // Current storytelling kinda thing
    public List<StartEmailData> startEmailList; // The lore before the story starts
    public int currentIndex; // Current progression
    public bool isResponding; // I need to remove this, probably making the progression automatic when the choices[] is null (Update: maybe not)
    public GameObject contactUI; // Reference to the UI GameObject for this contact
    public bool isUnread; // Indicates if the chat has unread messages

    // Updating the email preview based on the last email
    public void UpdateEmailPreview(int currentIndex, List<EmailData> currentEmailList, List<StartEmailData> currentStartEmailList)
    {
        string lastEmailText = GetLastEmailText(currentIndex, currentEmailList, currentStartEmailList);
        if (!string.IsNullOrEmpty(lastEmailText)) emailPreview = ShortenText(lastEmailText, 40);
    }

    // Getting the last email sent
    private string GetLastEmailText(int currentIndex, List<EmailData> emailList, List<StartEmailData> startEmailList)
    {
        if (emailList != null && emailList.Count > 0)
        {
            int lastIndex = Mathf.Min(currentIndex, emailList.Count - 1);
            if (lastIndex >= 0) return emailList[lastIndex].emailText;
        }

        if (startEmailList != null && startEmailList.Count > 0) return startEmailList[startEmailList.Count - 1].text;

        return string.Empty;
    }

    // Once it reaches 40 char in the latest message text, it would shorten and leave '...'
    private string ShortenText(string text, int maxLength)
    {
        if (text.Length <= maxLength) return text;
        else return text.Substring(0, maxLength - 3) + "...";
    }

    // Function to update the latest message on the contactUI's "Chat" child GameObject
    public void UpdateLatestEmailOnUI()
    {
        if (contactUI == null) return;

        Transform emailTransform = contactUI.transform.Find("Text");
        if (emailTransform == null) return;

        TextMeshProUGUI emailTMP = emailTransform.GetComponent<TextMeshProUGUI>();
        if (emailTMP == null) return;

        string latestEmail = GetLatestEmailText();
        if (!string.IsNullOrEmpty(latestEmail))
        {
            emailTMP.text = ShortenText(latestEmail, 40);
            emailTMP.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(emailTMP.rectTransform);
            Canvas.ForceUpdateCanvases();
        }
    }

    // Getting the LATEST email text (not last)
    private string GetLatestEmailText()
    {
        if (emailList != null && emailList.Count > 0)
        {
            int lastIndex = Mathf.Min(currentIndex, emailList.Count - 1);
            if (lastIndex >= 0) return emailList[lastIndex].emailText;
        }

        if (startEmailList != null && startEmailList.Count > 0)
        {
            return startEmailList[startEmailList.Count - 1].text;
        }

        return string.Empty;
    }
}
