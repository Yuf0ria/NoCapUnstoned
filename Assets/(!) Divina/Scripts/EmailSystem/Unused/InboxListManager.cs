using UnityEngine; using TMPro; using UnityEngine.UI; using System.Collections.Generic; using System.Linq;

public class InboxListManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject inboxPrefab; // The Inbox List prefab
    public RectTransform content; // ScrollView content
    public GameObject emailThreadPanel; // Reference to Email_Thread panel

    [Header("Data")]
    public List<InboxData> inbox; // The list of inboxes

    [Header("Settings")]
    private int currentInboxIndex = 0;
    private float totalHeight = 0;
    public IEmailRenderer emailRenderer; // Reference to the email renderer
    public int currentEmailIndex = -1; // Index of the currently open email thread
    private bool isInitialLoad = true; // Flag to track if it's the initial load

    private void Start()
    {
        InitializeContentSize();
        GetEmailRendererReference();
        if (inbox != null && inbox.Count > 0)
        {
            // Set all inboxes as unread at start
            for (int i = 0; i < inbox.Count; i++)
            {
                inbox[i].isUnread = true;
            }

            // Instantiate a duplicate for the first inbox to avoid using the original prefab
            Transform parent = content.transform;
            GameObject firstInboxUI = Instantiate(inboxPrefab, parent);
            DontDestroyOnLoad(firstInboxUI); // Make it persistent across scene loads or parent destruction
            // Set unread indicator to true for duplicated inbox
            Transform unreadTransform = firstInboxUI.transform.Find("Unread");
            if (unreadTransform != null) unreadTransform.gameObject.SetActive(true);
            inbox[0].contactUI = firstInboxUI; // Assign the duplicated GameObject to the first inbox
            ConfigureInbox(firstInboxUI, inbox[0], 0);
            currentInboxIndex = 1;
            currentEmailIndex = -1; // No email is open at start, so first inbox remains unread
        }
    }

    private void Update()
    {
        HandleBackgroundProgression();
        if (Input.GetKeyDown(KeyCode.Space)) AddInbox();
    }

    private void HandleBackgroundProgression()
    {
        // Background progression removed as per user request
    }

    private void AddInbox()
    {
        if (currentInboxIndex >= inbox.Count) return;

        Transform parent = content.transform;
        GameObject duplicate = Instantiate(inboxPrefab, parent);
        DontDestroyOnLoad(duplicate);

        // Set unread indicator to true for duplicated inbox
        Transform unreadTransform = duplicate.transform.Find("Unread");
        if (unreadTransform != null) unreadTransform.gameObject.SetActive(true);

        // Create a deep copy of the InboxData to ensure each inbox has its own independent state
        InboxData originalData = inbox[currentInboxIndex];
        InboxData newData = new InboxData
        {
            profileImage = originalData.profileImage,
            name = originalData.name,
            emailTitle = originalData.emailTitle,
            emailPreview = originalData.emailPreview,
            emailList = DeepCopyEmailList(originalData.emailList),
            currentIndex = 0, // Reset to 0 for fresh start
            isUnread = true, // New inboxes should be unread
            contactUI = duplicate // Assign the duplicated GameObject to the new inbox
        };

        // Update the email preview for the newly duplicated inbox
        UpdateEmailPreview(currentInboxIndex);

        ConfigureInbox(duplicate, newData, currentInboxIndex);
        PositionInbox(duplicate);

        currentInboxIndex++;
    }

    private List<EmailData> DeepCopyEmailList(List<EmailData> originalList)
    {
        List<EmailData> newList = new List<EmailData>();
        foreach (var ed in originalList)
        {
            EmailData newEd = new EmailData
            {
                emailText = ed.emailText,
                emailTitle = ed.emailTitle,
                name = ed.name,
                profile = ed.profile,
                Choices = ed.Choices != null ? (string[])ed.Choices.Clone() : null,
                linkBox = ed.linkBox
            };
            newList.Add(newEd);
        }
        return newList;
    }

    private void InitializeContentSize()
    {
        RectTransform originalRT = inboxPrefab.GetComponent<RectTransform>();
        totalHeight = originalRT.sizeDelta.y;
        UpdateContentSize();
    }

    private void GetEmailRendererReference()
    {
        if (emailThreadPanel != null)
        {
            emailRenderer = emailThreadPanel.GetComponent<IEmailRenderer>();
            // Set the inbox manager reference in the email renderer
            var renderer = emailRenderer as EmailRenderer;
            if (renderer != null) renderer.inboxManager = this;
        }
    }

    private void ConfigureInbox(GameObject inboxUI, InboxData data, int inboxIndex)
    {
        // Set tag to identify as inbox UI
        inboxUI.tag = "InboxUI";

        // Set profile image
        Transform profileTransform = inboxUI.transform.Find("Profile");
        if (profileTransform != null)
        {
            Image profileImg = profileTransform.GetComponent<Image>();
            if (profileImg != null) profileImg.sprite = data.profileImage;
        }

        // Set name
        Transform nameTransform = inboxUI.transform.Find("Name");
        if (nameTransform != null)
        {
            TextMeshProUGUI nameTMP = nameTransform.GetComponent<TextMeshProUGUI>();
            if (nameTMP != null) nameTMP.text = data.name;
        }

        // Set email preview
        Transform textTransform = inboxUI.transform.Find("Text");
        if (textTransform != null)
        {
            TextMeshProUGUI emailTMP = textTransform.GetComponent<TextMeshProUGUI>();
            if (emailTMP != null) emailTMP.text = data.emailPreview;
        }

        // Set unread indicator
        Transform unreadTransform = inboxUI.transform.Find("Unread");
        if (unreadTransform != null)
        {
            unreadTransform.gameObject.SetActive(data.isUnread);
        }

        // Add click listener
        Button inboxButton = inboxUI.GetComponent<Button>();
        if (inboxButton != null) inboxButton.onClick.AddListener(() => OnInboxSelected(inboxIndex));
    }

    private void PositionInbox(GameObject inboxUI)
    {
        RectTransform originalRT = inboxPrefab.GetComponent<RectTransform>();
        float elementHeight = originalRT.sizeDelta.y;
        RectTransform inboxRT = inboxUI.GetComponent<RectTransform>();
        inboxRT.anchoredPosition = new Vector2(originalRT.anchoredPosition.x, originalRT.anchoredPosition.y - totalHeight);

        totalHeight += elementHeight;
        UpdateContentSize();
    }

    private void UpdateContentSize()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }

    private void UpdateInboxUI(int inboxIndex)
    {
        GameObject inboxToUpdate = inbox[inboxIndex].contactUI;

        if (inboxToUpdate != null)
        {
            // Update the email preview text
            Transform emailTransform = inboxToUpdate.transform.Find("Text");
            if (emailTransform != null)
            {
                TextMeshProUGUI emailTMP = emailTransform.GetComponent<TextMeshProUGUI>();
                if (emailTMP != null)
                {
                    emailTMP.text = inbox[inboxIndex].emailPreview;
                    emailTMP.ForceMeshUpdate();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(emailTMP.rectTransform);
                    Canvas.ForceUpdateCanvases();
                }
            }

            // Update unread indicator
            UpdateUnreadIndicator(inboxIndex);
        }
    }

    public void UpdateUnreadIndicator(int inboxIndex)
    {
        GameObject inboxToUpdate = inbox[inboxIndex].contactUI;
        if (inboxToUpdate != null)
        {
            Transform unreadTransform = inboxToUpdate.transform.Find("Unread");
            if (unreadTransform != null)
            {
                unreadTransform.gameObject.SetActive(inbox[inboxIndex].isUnread);
            }
        }
    }

    private void OnInboxSelected(int inboxIndex)
    {
        if (emailRenderer == null || inboxIndex < 0 || inboxIndex >= inbox.Count) return;

        // Update the unread indicator for the previous current email to show it if unread
        if (currentEmailIndex >= 0 && currentEmailIndex != inboxIndex)
        {
            UpdateUnreadIndicator(currentEmailIndex);
        }

        currentEmailIndex = inboxIndex;

        // Mark as read when player opens the email
        inbox[inboxIndex].isUnread = false;
        UpdateUnreadIndicator(inboxIndex);

        emailRenderer.ClearEmail();
        ShowEmailThread();

        LoadEmailData(inboxIndex);
        RestoreEmailState(inboxIndex);

        // Update the top panel with inbox info
        UpdateTopPanel(inboxIndex);

        // Force update the inbox UI for the selected email
        UpdateInboxUI(inboxIndex);

        // Update the latest email on the inbox UI after switching back
        inbox[inboxIndex].UpdateLatestEmailOnUI();

        // After initial load, set flag to false
        if (isInitialLoad) isInitialLoad = false;
    }

    private void ShowEmailThread()
    {
        if (emailThreadPanel != null) emailThreadPanel.SetActive(true);
    }

    private void LoadEmailData(int inboxIndex)
    {
        var renderer = emailRenderer as EmailRenderer;
        if (renderer != null)
        {
            renderer.emailList = inbox[inboxIndex].emailList;
            renderer.startEmailList = new List<StartEmailData>(); // Reset start emails
        }
    }

    private void RestoreEmailState(int inboxIndex)
    {
        var renderer = emailRenderer as EmailRenderer;
        if (renderer == null) return;

        renderer.currentIndex = inbox[inboxIndex].currentIndex;

        // Render start emails (history) if any
        // For emails, start emails might not be needed, but if present

        // Render emails up to current index
        for (int i = 0; i <= renderer.currentIndex && i < renderer.emailList.Count; i++)
        {
            renderer.RenderEmail(renderer.emailList[i]);
        }
    }

    private void UpdateTopPanel(int inboxIndex)
    {
        if (emailThreadPanel == null || inboxIndex < 0 || inboxIndex >= inbox.Count) return;

        // Find the Top Panel GameObject
        Transform topPanel = emailThreadPanel.transform.Find("Top Panel");
        if (topPanel == null) return;

        // Update Name
        TextMeshProUGUI nameTMP = topPanel.Find("Name")?.GetComponent<TextMeshProUGUI>();
        if (nameTMP != null) nameTMP.text = inbox[inboxIndex].name;

        // Update Email Title
        TextMeshProUGUI titleTMP = topPanel.Find("Email Title")?.GetComponent<TextMeshProUGUI>();
        if (titleTMP != null) titleTMP.text = inbox[inboxIndex].emailTitle;
    }

    public void UpdateEmailPreview(int emailIndex)
    {
        // Update the email preview for the specified email index
        if (emailIndex >= 0 && emailIndex < inbox.Count)
        {
            InboxData updatedData = inbox[emailIndex];
            // Use the text from the first email in the EmailList
            if (updatedData.emailList != null && updatedData.emailList.Count > 0) updatedData.emailPreview = ShortenText(updatedData.emailList[0].emailText, 40);
            inbox[emailIndex] = updatedData;
            UpdateInboxUI(emailIndex);
        }
    }

    private string ShortenText(string text, int maxLength)
    {
        if (text.Length <= maxLength) return text;
        else return text.Substring(0, maxLength - 3) + "...";
    }

    public void ReloadEmailThread(int emailIndex)
    {
        if (currentEmailIndex == emailIndex && emailRenderer != null)
        {
            LoadEmailData(emailIndex);
            emailRenderer.ResetEmail();
        }
    }

    public void AddNewInbox(InboxData newInboxData)
    {
        inbox.Add(newInboxData);
        // Instantiate and configure the new inbox UI
        Transform parent = content.transform;
        GameObject newInboxUI = Instantiate(inboxPrefab, parent);
        DontDestroyOnLoad(newInboxUI);
        ConfigureInbox(newInboxUI, newInboxData, inbox.Count - 1);
        PositionInbox(newInboxUI);
        newInboxData.contactUI = newInboxUI;
    }

    public void RemoveInbox(int inboxIndex)
    {
        if (inboxIndex < 0 || inboxIndex >= inbox.Count) return;

        // Destroy the UI element
        if (inbox[inboxIndex].contactUI != null)
        {
            Destroy(inbox[inboxIndex].contactUI);
        }

        // Remove from list
        inbox.RemoveAt(inboxIndex);

        // Update indices for remaining inboxes
        for (int i = inboxIndex; i < inbox.Count; i++)
        {
            if (inbox[i].contactUI != null)
            {
                // Update the click listener with the new index
                Button inboxButton = inbox[i].contactUI.GetComponent<Button>();
                if (inboxButton != null)
                {
                    inboxButton.onClick.RemoveAllListeners();
                    inboxButton.onClick.AddListener(() => OnInboxSelected(i));
                }
            }
        }

        // Adjust currentEmailIndex if necessary
        if (currentEmailIndex >= inboxIndex)
        {
            currentEmailIndex--;
        }

        // Recalculate total height and reposition all inboxes
        RecalculateContentSize();
    }

    private void RecalculateContentSize()
    {
        totalHeight = 0;
        RectTransform originalRT = inboxPrefab.GetComponent<RectTransform>();
        float elementHeight = originalRT.sizeDelta.y;

        for (int i = 0; i < inbox.Count; i++)
        {
            if (inbox[i].contactUI != null)
            {
                RectTransform inboxRT = inbox[i].contactUI.GetComponent<RectTransform>();
                inboxRT.anchoredPosition = new Vector2(originalRT.anchoredPosition.x, originalRT.anchoredPosition.y - totalHeight);
                totalHeight += elementHeight;
            }
        }

        UpdateContentSize();
    }
}
