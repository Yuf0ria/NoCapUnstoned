using UnityEngine;
using System.Collections.Generic;

public class ChatDataModifier : MonoBehaviour
{
    [Header("Contact List Manager Reference")]
    [SerializeField] private ContactListManager contactListManager;

    [Header("Modification Settings")]
    [SerializeField] private int targetChatIndex = -1; // -1 to add new ChatData, >=0 to append to existing
    [SerializeField] private ChatData newChatData; // The ChatData to add or append from

    private bool hasApplied = false; // Flag to ensure it only applies once per activation

    private void OnEnable()
    { 
        if (hasApplied) return; // Prevent multiple applications per activation
         
        if (contactListManager == null)
        {
            return;
        }
         
        if (targetChatIndex == -1)
        { 
            // Check if key fields are empty to prevent invalid new additions
            if (string.IsNullOrEmpty(newChatData.name) || string.IsNullOrEmpty(newChatData.phoneNumber) || newChatData.profileImage == null) return;

            // Add new ChatData, but initialize state fields to defaults to avoid overwriting runtime data
            ChatData newEntry = new ChatData
            {
                profileImage = newChatData.profileImage,
                name = newChatData.name,
                phoneNumber = newChatData.phoneNumber,
                chat = newChatData.chat,
                messageList = new List<MessageData>(newChatData.messageList ?? new List<MessageData>()),
                startMessageList = new List<StartMessageData>(newChatData.startMessageList ?? new List<StartMessageData>()),
                currentIndex = 0, // Default
                isResponding = false, // Default
                isAutoProgressing = false, // Default
                autoProgressTimer = 0f, // Default
                isUnread = true, // New chats should be unread
                contactUI = null // Default
            };
            contactListManager.contacts.Add(newEntry);
        }
        else if (targetChatIndex >= 0 && targetChatIndex < contactListManager.contacts.Count)
        { 
            // If this is the currently open chat, save its current state to ensure contacts has the latest startMessageList
            if (targetChatIndex == contactListManager.currentChatIndex)
            {
                contactListManager.SaveCurrentChatState();
            }
            // Append to existing ChatData, skipping state fields; allow as long as lists are not null (even if empty, but per user, append if not null)
            ChatData existingChat = contactListManager.contacts[targetChatIndex];
            bool AddMessage = false;
             
            if (newChatData.messageList != null && newChatData.messageList.Count > 0)
            {
                List<MessageData> newMessages = newChatData.messageList;
                int firstChoiceIndex = -1;
                for (int i = 0; i < newMessages.Count; i++)
                {
                    existingChat.messageList.Add(newMessages[i]);
                    if (newMessages[i].Choices != null && newMessages[i].Choices.Length > 0)
                    {
                        if (firstChoiceIndex == -1) firstChoiceIndex = existingChat.messageList.Count - newMessages.Count + i;
                    }
                }
                AddMessage = true;
            }
             
            if (newChatData.startMessageList != null && newChatData.startMessageList.Count > 0)
            {
                existingChat.startMessageList.AddRange(newChatData.startMessageList);
                AddMessage = true;
            }

            if (!AddMessage) { return; }

            // Set unread indicator since new messages were added
            existingChat.isUnread = true;
            contactListManager.UpdateUnreadIndicator(targetChatIndex);

            // Update currentIndex with the additional messages
            int choiceIndex = -1;
            if (newChatData.messageList != null && newChatData.messageList.Count > 0)
            {
                for (int i = 0; i < newChatData.messageList.Count; i++)
                {
                    if (newChatData.messageList[i].Choices != null && newChatData.messageList[i].Choices.Length > 0)
                    {
                        choiceIndex = existingChat.messageList.Count - newChatData.messageList.Count + i;
                        break; //STOP STOPPPP
                    }
                }
            }
            if (choiceIndex != -1) existingChat.currentIndex = choiceIndex;
            else existingChat.currentIndex = existingChat.messageList.Count - 1;

            contactListManager.contacts[targetChatIndex] = existingChat; // Update the list

            // Update the chat preview for the modified contact
            contactListManager.UpdateChatPreview(targetChatIndex);

            // If this is the currently open chat, reload the message thread to show the new messages
            contactListManager.ReloadMessageThread(targetChatIndex);
        } 

        hasApplied = true;
    }
}
