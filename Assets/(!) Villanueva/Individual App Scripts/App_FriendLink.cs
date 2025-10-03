using DG.Tweening;
using UnityEngine;
//using static System.TimeZoneInfo; HEY WHAT THE FUCK IS THIS?!

public class App_FriendLink : MonoBehaviour
{
    /// <summary>
    /// Hiii. I just copy pasted this from App_Webb.
    /// </summary>
    
    // Our naming conventions is so vastly different from each other it makes Sir Tala cry.

    [Header("Web Page")]
    [SerializeField] private Transform friendlinkPageClosedPosition; // = new Vector3(0, 2500, 0);
    [SerializeField] private Transform friendlinkPageOpenedPosition; // = new Vector3(0, 0, 0);

    [Header("Seach Box")]
    [SerializeField] private Transform messageBoxHidePosition; 
    [SerializeField] private Transform messageBoxClosedPosition; // = new Vector3(0, 250, 0);
    [SerializeField] private Transform messageBoxOpenedPosition; // = new Vector3(0, -210, 0);

    float transitionTime = 0.5f;

    [SerializeField] GameObject messageBox;
    private GameObject activepage;

    /// <summary>
    /// FriendLink Pages
    /// </summary>

    public void OpenFriendlinkPage(GameObject friendlinkPage)
    {
        Debug.Log("Opening Friendlink Page: " + friendlinkPage.name);

        friendlinkPage.transform.position = friendlinkPageClosedPosition.position;
        friendlinkPage.gameObject.SetActive(true);

        friendlinkPage.transform.DOMove(friendlinkPageOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic);
        messageBox.transform.DOMove(messageBoxHidePosition.position, transitionTime).SetEase(Ease.OutCubic);

        // Hi Aundee, I know you can do better than whatever this is.

        if (activepage != null) // Checks if the app was just opened or not. If the activepage is true, then it would move the prev page as a new one opens.
        {
            Debug.Log("Now moving: " + activepage);
            activepage.transform.DOMove(friendlinkPageClosedPosition.position, transitionTime).SetEase(Ease.OutCubic);
            activepage = friendlinkPage;
            Debug.Log("New active page: " + activepage);
        } else //If the app was just opened for the first time, or atleast empty, then it just sets whatever page you opened as a new one
        {
            activepage = friendlinkPage;
            Debug.Log("New active page: " + activepage);
        }
    }
    
    public void ReturnToFriendlinkPage(GameObject friendlinkPage)
    {
        Debug.Log("Returning to Browser Main Page...");

        friendlinkPage.gameObject.SetActive(true);

        transform.DOMove(friendlinkPageClosedPosition.position, transitionTime).SetEase(Ease.OutCubic);

        activepage = null;
    }

    public void OpenFriendlinkMessage(GameObject friendlinkMessage) //WHEN OPENING A MESSAGE BTW
    {
        Debug.Log("Opening Friendlink Page: " + friendlinkMessage.name);

        friendlinkMessage.transform.position = friendlinkPageClosedPosition.position;
        friendlinkMessage.gameObject.SetActive(true);

        friendlinkMessage.transform.DOMove(friendlinkPageOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic);

        //The Message Box when it's a message
        messageBox.transform.position = messageBoxHidePosition.position;
        messageBox.transform.DOMove(messageBoxClosedPosition.position, transitionTime).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// ReplyBox
    /// </summary>

    public void ShowReplyOptions(GameObject MessageBox)
    {
        Debug.Log("Showing Reply Options...");
        messageBox = MessageBox;
        messageBox.transform.position = messageBoxClosedPosition.transform.position;
        messageBox.transform.DOMove(messageBoxOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectReply(GameObject MessageBox) //Hey, why the fuck does this one not work. bruh
    {
        Debug.Log("Hiding Reply Options...");
        messageBox = MessageBox;
        messageBox.transform.position = messageBoxOpenedPosition.transform.position; //EDIT: I FIXED IT
        messageBox.transform.DOMove(messageBoxClosedPosition.position, transitionTime).SetEase(Ease.OutCubic);
    }

    public void HideReply(GameObject MessageBox) //Hey, why the fuck does this one not work. bruh
    {
        Debug.Log("Hiding Reply Options...");
        messageBox = MessageBox;
        messageBox.transform.DOMove(messageBoxHidePosition.position, transitionTime).SetEase(Ease.OutCubic);
    }

    public void SendReply(GameObject Reply)
    {
        //Nothing Here Yet
    }
}
