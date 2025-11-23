using UnityEngine;
using System.Collections.Generic;

public class InboxDataModifier : MonoBehaviour
{
    [Header("Inbox List Manager Reference")]
    [SerializeField] private InboxListManager inboxListManager;

    [Header("Modification Settings")]
    [SerializeField] private int targetInboxIndex = -1; // -1 to add new InboxData, >=0 to append to existing
    [SerializeField] private InboxData newInboxData; // The InboxData to add or append from

    private bool hasApplied = false; // Flag to ensure it only applies once per activation

    private void OnEnable()
    {
        if (hasApplied) return; // Prevent multiple applications per activation

        if (inboxListManager == null)
        {
            return;
        }

        if (targetInboxIndex == -1)
        {
            // Check if key fields are empty to prevent invalid new additions
            if (string.IsNullOrEmpty(newInboxData.name) || string.IsNullOrEmpty(newInboxData.emailTitle) || newInboxData.profileImage == null) return;

            // Add new InboxData, but initialize state fields to defaults to avoid overwriting runtime data
            InboxData newEntry = new InboxData
            {
                profileImage = newInboxData.profileImage,
                name = newInboxData.name,
                emailTitle = newInboxData.emailTitle,
                emailPreview = newInboxData.emailPreview,
                emailList = new List<EmailData>(newInboxData.emailList ?? new List<EmailData>()),
                startEmailList = new List<StartEmailData>(newInboxData.startEmailList ?? new List<StartEmailData>()),
                currentIndex = 0, // Default
                isResponding = false, // Default
                isUnread = true, // New inboxes should be unread
                contactUI = null, // Default
            };
            inboxListManager.inbox.Add(newEntry);
        }
        else if (targetInboxIndex >= 0 && targetInboxIndex < inboxListManager.inbox.Count)
        {
            // If this is the currently open inbox, save its current state to ensure inbox has the latest startEmailList
            if (targetInboxIndex == inboxListManager.currentEmailIndex)
            {
                SaveCurrentEmailState();
            }
            // Append to existing InboxData, skipping state fields; allow as long as lists are not null (even if empty, but per user, append if not null)
            InboxData existingInbox = inboxListManager.inbox[targetInboxIndex];
            bool AddEmail = false;

            if (newInboxData.emailList != null && newInboxData.emailList.Count > 0)
            {
                List<EmailData> newEmails = newInboxData.emailList;
                int firstChoiceIndex = -1;
                for (int i = 0; i < newEmails.Count; i++)
                {
                    existingInbox.emailList.Add(newEmails[i]);
                    if (newEmails[i].Choices != null && newEmails[i].Choices.Length > 0)
                    {
                        if (firstChoiceIndex == -1) firstChoiceIndex = existingInbox.emailList.Count - newEmails.Count + i;
                    }
                }
                AddEmail = true;
            }

            if (newInboxData.startEmailList != null && newInboxData.startEmailList.Count > 0)
            {
                existingInbox.startEmailList.AddRange(newInboxData.startEmailList);
                AddEmail = true;
            }

            if (!AddEmail) { return; }

            // Set unread indicator since new emails were added
            existingInbox.isUnread = true;
            inboxListManager.UpdateUnreadIndicator(targetInboxIndex);

            // Update currentIndex with the additional emails
            int choiceIndex = -1;
            if (newInboxData.emailList != null && newInboxData.emailList.Count > 0)
            {
                for (int i = 0; i < newInboxData.emailList.Count; i++)
                {
                    if (newInboxData.emailList[i].Choices != null && newInboxData.emailList[i].Choices.Length > 0)
                    {
                        choiceIndex = existingInbox.emailList.Count - newInboxData.emailList.Count + i;
                        break; //STOP STOPPPP
                    }
                }
            }
            if (choiceIndex != -1) existingInbox.currentIndex = choiceIndex;
            else existingInbox.currentIndex = existingInbox.emailList.Count - 1;

            inboxListManager.inbox[targetInboxIndex] = existingInbox; // Update the list

            // Update the email preview for the modified inbox
            inboxListManager.UpdateEmailPreview(targetInboxIndex);

            // If this is the currently open inbox, reload the email thread to show the new emails
            inboxListManager.ReloadEmailThread(targetInboxIndex);
        }

        hasApplied = true;
    }

    private void SaveCurrentEmailState()
    {
        if (inboxListManager.currentEmailIndex >= 0 && inboxListManager.currentEmailIndex < inboxListManager.inbox.Count)
        {
            var renderer = inboxListManager.emailRenderer as EmailRenderer;
            if (renderer != null)
            {
                inboxListManager.inbox[inboxListManager.currentEmailIndex].currentIndex = renderer.currentIndex;
                inboxListManager.inbox[inboxListManager.currentEmailIndex].startEmailList = new List<StartEmailData>(renderer.startEmailList);
            }
        }
    }
}
