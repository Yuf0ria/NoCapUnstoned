using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Event_Manager : MonoBehaviour
{
    // Notification Objects
    [SerializeField] RectTransform Notif_Rect;
    [SerializeField] GameObject Notif_Icon;
    [SerializeField] GameObject Notif_Name;
    [SerializeField] GameObject Notif_Desc;
    [SerializeField] Sprite[] Icons;

    // =========================================================== Caia, I hard coded these two values. You might need to adjust this
    Vector3 Notif_HideTransform;
    Vector3 Notif_ShowTransform;
    float TransitionTime = 1;
    float ShowTime = 3;


    // Number of Events per category
    static int numOfCommon = 5;
    static int numOfRare;
    static int numOfRandom;

    void Start()
    {
        Notif_HideTransform = new Vector3(0, 1100, 0);
        Notif_ShowTransform = new Vector3(0, 845, 0);

        Notif_Rect.localPosition = Notif_HideTransform;
    }
    
    void New_Notification(int icon_num, string name, string desc)
    {
        Notif_Icon.GetComponent<Image>().sprite = Icons[icon_num];
        Notif_Name.GetComponent<TextMeshProUGUI>().text = name;
        Notif_Desc.GetComponent<TextMeshProUGUI>().text = desc;

        Notif_Rect.transform.DOLocalMove(Notif_ShowTransform, TransitionTime)
        .OnComplete(() =>
        {
            Notif_Rect.transform.DOLocalMove(Notif_ShowTransform, ShowTime)
            .OnComplete(() =>
            {
                Notif_Rect.transform.DOLocalMove(Notif_HideTransform, TransitionTime);
            });
        });
    }

    public void Run_Event()
    {
        //Common Events
        if (Random.Range(0, 99) % 3 == 0) // Check if the remainder is 0, which is true about 1/3 of the time
        {
            switch (Random.Range(0, numOfCommon - 1))
            {
                case 1:
                    Common_Phishing_Postmail();
                    break;

                case 2:
                    Common_Fake_Friendlink();
                    break;

                case 3:
                    Common_Spam_Postmail();
                    break;

                case 4:
                    Common_Fake_Message();
                    break;

                default:
                    Common_Real_Postmail();
                    break;
            }

        }

        //Rare Events
        if (Random.Range(0, 99) % 10 == 0) // Check if the remainder is 0, which is true about 1/10 of the time
        {

        }

    }

    void Common_Real_Postmail()
    {
        //Debug.Log("You recieve a real email! :)");


        New_Notification(0, "Postmail", "You have a new Postmail! :D"); // Need Varying versions of postmails
    }

    void Common_Phishing_Postmail()
    {
        //Debug.Log("You recieve a phishing email! :(");


        New_Notification(0, "Postmail", "You recieve a phishing email! :(");
    }

    void Common_Fake_Friendlink()
    {
        //Debug.Log("A phished account posted on Friendlink.");


        New_Notification(0, "Postmail", "A phished account posted on Friendlink.");
    }

    void Common_Spam_Postmail()
    {
        //Debug.Log("You received SPAM."); //These are more SPAM than Phishing

        New_Notification(0, "Postmail", "You received SPAM.");
    }

    void Common_Fake_Message()
    {
        //Debug.Log("A phished account sent a Message.");

        New_Notification(0, "Postmail", "A phished account sent a Message.");
    }



    // Common Events, 33% Chance of occuring every time a task is completed OR when the story progresses
    // The Following are possible Events:

    // Player recieves a REAL postmail, this poses no danger.
    // Player recieves a FAKE postmail, this has a link
    // Friendlink Posts, Postmail or Messages from a Phished Accounts

    // IF the player has been compromised, Player will recieve a notifications for a Post they never posted themselves.
    // IF the player has been compromised, Player will have a new Photo in their Gallery 



    // Rare Events, 10% Chance of occuring every time a task is completed OR when the story progresses 
    // (Can Happen at the same time as Common)

    // Player will recieve SPAM Postmail or Messages, for the next minute, every 10 seconds.
    // IF the player has been compromised, Player Recieves REAL Notification of "Suspicious Activity" and a reminder to update security
    // IF the player has been compromised, Player gets locked out of their own account



    // Random Events, 100% Chance of occuring every time a task is completed:
    // Phone Lag??
    // WiFi Disconnects
    // New Eduva, Webb or Friendlink Post: Half are Normal, Half are about being Hacks or Phishing Warnings
    // 
    
    // EVENTS CAN STACK, so multiple events can happen in a single trigger
}
