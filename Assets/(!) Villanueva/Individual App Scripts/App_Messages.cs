using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class App_Messages : MonoBehaviour
{
    [Header("Mail")]
    [SerializeField] private Transform messageClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform messageOpenedPosition; //= new Vector3(0, -175, 0);

    [Header("Reply Box")]
    [SerializeField] private Transform ReplyBoxHidePosition; //To hide the reply box - Nicaia
    [SerializeField] private Transform ReplyBoxClosedPosition; //= new Vector3(0, -1275, 0);
    [SerializeField] private Transform ReplyBoxOpenedPosition; //= new Vector3(0, -450, 0);

    float TransitionTime = 0.5f;

    [SerializeField] GameObject ReplyBox;
    //[SerializeField] Button ReplyOpenBox;
    //[SerializeField] private bool disableReplyBox;

    /// <summary>
    /// Mail Thread Page
    /// </summary>

    public void OpenMessageThread(GameObject messageThreadPage)
    {
        Debug.Log("Opening Mail Thread: " + messageThreadPage.name);

        messageThreadPage.transform.position = messageClosedPosition.position;
        messageThreadPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); //Disabled this as it feels off setting the prev page inactive as the other goes in. - Nicaia

        messageThreadPage.transform.DOMove(messageOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
        ReplyBox.transform.DOMove(ReplyBoxClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic); //Fancy appearing of reply box - Nicaia

        //if (disableReplyBox) 
        //{
        //    ReplyOpenBox.interactable = false;
        //} else 
        //{ 
        //    ReplyOpenBox.interactable = true;
        //}
    }

    //I replaced the DOLocalMove to DOMove. I spent 3 hours figuring out what was going wrong. :P

    public void ReturnToContacts(GameObject InboxPage)
    {
        Debug.Log("Returning to Inbox...");

        InboxPage.gameObject.SetActive(true);

        ReplyBox.transform.DOMove(ReplyBoxHidePosition.position, TransitionTime).SetEase(Ease.OutCubic); //Fancy hiding of reply box - Nicaia
        transform.DOMove(messageClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });

        //ReplyOpenBox.interactable = true;
    }

    /// <summary>
    /// ReplyBox
    /// </summary>

    //Future Features:
    // - Selecting a reply will update the reply box, besides just closing it.
    // - Send makes a copy of the Reponse Game Object and adds it to the Scroll View

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






    //Vector3 ChatClosedPoint = new Vector3(1250, 185, 0);
    //Vector3 ChatOpenedPoint = new Vector3(-5, 185, 0);
    //float ChatTransitionTime = 0.5f;

    //public void OpenChat(GameObject Chat_Page)
    //{
    //    Debug.Log("Opening... " + Chat_Page);

    //    Chat_Page.transform.position = chatClosedPosition;
    //    Chat_Page.gameObject.SetActive(true);

    //    this.gameObject.SetActive(false);

    //    Chat_Page.transform.DOLocalMove(ChatOpenedPoint, ChatTransitionTime).SetEase(Ease.OutCubic);
    //}

    //public void ReturnToMain(GameObject PrevPage)
    //{
    //    Debug.Log("Returning to Contacts...");

    //    PrevPage.gameObject.SetActive(true);

    //    transform.DOLocalMove(ChatClosedPoint, ChatTransitionTime).SetEase(Ease.OutCubic)
    //    .OnComplete(() =>
    //    {
    //        this.gameObject.SetActive(false);
    //    });
    //}

    //Future Features:
    // - Selecting a reply will update the reply box, besides just closing it.
    // - Send makes a copy of the Reponse Game Object and adds it to the Scroll View

    //Vector3 ResponseClosedPoint = new Vector3(0, -1275, 0);
    //Vector3 ResponseOpenedPoint = new Vector3(0, -750, 0);
    //float ResponseTransitionTime = 0.5f;
    //float ReplyTransitionTime = 0.25f;
    //[SerializeField] GameObject ResponseBox;

    //public void ShowResponses(GameObject responseBox)
    //{
    //    Debug.Log("Showing Responses");
    //    ResponseBox = responseBox;
    //    ResponseBox.transform.DOLocalMove(ResponseOpenedPoint, ResponseTransitionTime).SetEase(Ease.OutCubic);
    //}

    //public void SelectResponse(GameObject Reply)
    //{
    //    Debug.Log("Hiding Responses");
    //    ResponseBox.transform.DOLocalMove(ResponseClosedPoint, ResponseTransitionTime).SetEase(Ease.OutCubic);
    //}

    //public void SendReply(GameObject Reply)
    //{
    //    GameObject Chatbox = this.gameObject;

    //    GameObject clone = Instantiate(Reply);
    //    clone.transform.SetParent(Chatbox.transform, true);

    //    clone.transform.localPosition = new Vector3(750, -50, 0);

    //    clone.transform.DOLocalMove(new Vector3(232, -50, 0), ReplyTransitionTime).SetEase(Ease.OutCubic);

    //}
}
