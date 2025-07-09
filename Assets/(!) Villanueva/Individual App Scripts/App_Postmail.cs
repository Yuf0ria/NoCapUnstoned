using UnityEngine;
using DG.Tweening;

public class App_Postmail : MonoBehaviour
{
    Vector3 MailClosedPosition = new Vector3(1250, -175, 0);
    Vector3 MailOpenedPosition = new Vector3(0, -175, 0);
    float MailTransitionTime = 0.5f;

    public void OpenMailThread(GameObject MailThreadPage)
    {
        Debug.Log("Opening Mail Thread: " + MailThreadPage.name);

        MailThreadPage.transform.localPosition = MailClosedPosition;
        MailThreadPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        MailThreadPage.transform.DOLocalMove(MailOpenedPosition, MailTransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToInbox(GameObject InboxPage)
    {
        Debug.Log("Returning to Inbox...");

        InboxPage.gameObject.SetActive(true);

        transform.DOLocalMove(MailClosedPosition, MailTransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    //Future Features:
    // - Selecting a reply will update the reply box, besides just closing it.
    // - Send makes a copy of the Reponse Game Object and adds it to the Scroll View
    // - .

    Vector3 ReplyBoxClosedPosition = new Vector3(0, -1275, 0);
    Vector3 ReplyBoxOpenedPosition = new Vector3(0, -450, 0);
    float ReplyBoxTransitionTime = 0.5f;

    [SerializeField] GameObject ReplyBox;

    public void ShowReplyOptions(GameObject replyBox)
    {
        Debug.Log("Showing Reply Options...");
        ReplyBox = replyBox;
        ReplyBoxClosedPosition = ReplyBox.transform.localPosition;
        ReplyBox.transform.DOLocalMove(ReplyBoxOpenedPosition, ReplyBoxTransitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectReply(GameObject Reply)
    {
        Debug.Log("Hiding Reply Options...");
        ReplyBox.transform.DOLocalMove(ReplyBoxClosedPosition, ReplyBoxTransitionTime).SetEase(Ease.OutCubic);
    }

    public void SendReply(GameObject Reply)
    {
        //Nothing Here Yet
    }
}
