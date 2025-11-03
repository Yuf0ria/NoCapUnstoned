using UnityEngine;

public interface IMessageRenderer
{
    void RenderMessage(MessageData data, bool showChoices = true);
    void ClearMessages();
    void ResetMessaging();
}
