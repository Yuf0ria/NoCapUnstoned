using UnityEngine;
using DG.Tweening;

public class App_Messages : MonoBehaviour
{
    Vector3 ChatClosedPoint = new Vector3(1250, 185, 0);
    Vector3 ChatOpenedPoint = new Vector3(-5, 185, 0);
    float ChatTransitionTime = 0.5f;
    public void OpenChat(GameObject Chat_Page)
    {
        Debug.Log("Opening... " + Chat_Page);

        Chat_Page.transform.localPosition = ChatClosedPoint;
        Chat_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        Chat_Page.transform.DOLocalMove(ChatOpenedPoint, ChatTransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToMain(GameObject PrevPage)
    {
        Debug.Log("Returning to Contacts...");

        PrevPage.gameObject.SetActive(true);

        transform.DOLocalMove(ChatClosedPoint, ChatTransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }


    //Future Features:
    // - Selecting a reply will update the reply box, besides just closing it.
    // - Send makes a copy of the Reponse Game Object and adds it to the Scroll View
    
    Vector3 ResponseClosedPoint = new Vector3(0, -1275, 0);
    Vector3 ResponseOpenedPoint = new Vector3(0, -750, 0);
    float ResponseTransitionTime = 0.5f;
    float ReplyTransitionTime = 0.25f;
    [SerializeField] GameObject ResponseBox;

    public void ShowResponses(GameObject responseBox)
    {
        Debug.Log("Showing Responses");
        ResponseBox = responseBox;
        ResponseBox.transform.DOLocalMove(ResponseOpenedPoint, ResponseTransitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectResponse(GameObject Reply)
    {
        Debug.Log("Hiding Responses");
        ResponseBox.transform.DOLocalMove(ResponseClosedPoint, ResponseTransitionTime).SetEase(Ease.OutCubic);
    }

    public void SendReply(GameObject Reply)
    {
        GameObject Chatbox = this.gameObject;

        GameObject clone = Instantiate(Reply);
        clone.transform.SetParent(Chatbox.transform, true);

        clone.transform.localPosition = new Vector3(750, -50, 0);

        clone.transform.DOLocalMove(new Vector3(232, -50, 0), ReplyTransitionTime).SetEase(Ease.OutCubic);

    }
}
