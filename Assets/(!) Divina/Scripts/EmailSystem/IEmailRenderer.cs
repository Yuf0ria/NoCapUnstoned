using UnityEngine;

public interface IEmailRenderer
{
    void RenderEmail(EmailData data, bool showChoices = true);
    void ClearEmail();
    void ResetEmail();
}
