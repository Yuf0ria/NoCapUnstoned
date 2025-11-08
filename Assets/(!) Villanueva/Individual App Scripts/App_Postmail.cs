using UnityEngine;
using DG.Tweening;

public class App_Postmail : MonoBehaviour
{
    [Header("Mail")]
    [SerializeField] private Transform MailClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform MailOpenedPosition; //= new Vector3(0, -175, 0);

    [Header("Reply Box")]
    [SerializeField] private Transform ReplyBoxHidePosition; //To hide the reply box - Nicaia
    [SerializeField] private Transform ReplyBoxClosedPosition; //= new Vector3(0, -1275, 0);
    [SerializeField] private Transform ReplyBoxOpenedPosition; //= new Vector3(0, -450, 0);

    float TransitionTime = 0.5f; //Both has the same time transitions so...

    [SerializeField] GameObject ReplyBox;

    /// <summary>
    /// Mail Thread Page
    /// </summary>

    public void OpenMailThread(GameObject MailThreadPage)
    {
        Debug.Log("Opening Mail Thread: " + MailThreadPage.name);

        MailThreadPage.transform.position = MailClosedPosition.position;
        MailThreadPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); //Disabled this as it feels off setting the prev page inactive as the other goes in. - Nicaia

        MailThreadPage.transform.DOMove(MailOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
        ReplyBox.transform.DOMove(ReplyBoxClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic); //Fancy appearing of reply box - Nicaia
    }

    //I replaced the DOLocalMove to DOMove. I spent 3 hours figuring out what was going wrong. :P

    public void ReturnToInbox(GameObject InboxPage)
    {
        Debug.Log("Returning to Inbox...");

        InboxPage.gameObject.SetActive(true);

        ReplyBox.transform.DOMove(ReplyBoxHidePosition.position, TransitionTime).SetEase(Ease.OutCubic); //Fancy hiding of reply box - Nicaia
        transform.DOMove(MailClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    //Future Features:
    // - Selecting a reply will update the reply box, besides just closing it.
    // - Send makes a copy of the Reponse Game Object and adds it to the Scroll View
    // - .

    /// <summary>
    /// ReplyBox
    /// </summary>

    public void ShowReplyOptions(GameObject replyBox)
    {
        Debug.Log("Showing Reply Options...");
        ReplyBox = replyBox;
        ReplyBox.transform.position = ReplyBoxClosedPosition.transform.position;
        ReplyBox.transform.DOMove(ReplyBoxOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectReply(GameObject replyBox) //Hey, why the fuck does this one not work. bruh
    {
        Debug.Log("Hiding Reply Options...");
        ReplyBox = replyBox;
        ReplyBox.transform.position = ReplyBoxOpenedPosition.transform.position; //EDIT: I FIXED IT
        ReplyBox.transform.DOMove(ReplyBoxClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void SendReply(GameObject Reply)
    {
        //Nothing Here Yet
    }
}
