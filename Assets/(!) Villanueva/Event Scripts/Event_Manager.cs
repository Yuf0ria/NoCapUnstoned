using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Event_Manager : MonoBehaviour
{
    // Notification Objects
    [SerializeField] RectTransform Notif_Rect;
    [SerializeField] GameObject Notif_Icon;
    [SerializeField] GameObject Notif_Name;
    [SerializeField] GameObject Notif_Desc;
    [SerializeField] Sprite[] Icons;

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
    
    public void New_Notification(int icon_num, string name, string desc)
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

    public void Run_RandomEvent()
    {
        //Common Events
        if (Random.Range(0, 99) % 3 == 0) // Check if the remainder is 0, which is true about 1/3 of the time
        {
            switch (Random.Range(0, numOfCommon - 1))
            {
                case 1:
                    Common_Spam_Postmail();
                    break;

                default:
                    Common_DisconnectWiFi();
                    break;
            }

        }

        //Rare Events
        if (Random.Range(0, 99) % 10 == 0) // Check if the remainder is 0, which is true about 1/10 of the time
        {

        }

    }

    public void Run_SpecificEvent(int eventID)
    {
        Debug.Log("Running Event " + eventID + "...");
        switch (eventID)
            {
                case 1: // Postmail SPAM
                    Common_Spam_Postmail();
                    break;
                    
                case 2: // Multiple Postmail SPAM
                    if (Phone_Statistics.isCompromised) StartCoroutine(Rare_ConstantSpam_Postmail());
                    break;
                    
                case 3: // App Crashes (Exits App Slowly)
                    if (Phone_Statistics.isCompromised) StartCoroutine(Rare_CrashApp());
                    break;

                case 4: // App Slows (Longer Transition Times)

                    break;

                case 5: // Shows a Random Ad

                    break;

                case 6: // App Slows (Longer Transition Times)

                    break;

                case 7: // App Slows (Longer Transition Times)

                    break;

                default:
                    Common_DisconnectWiFi();
                    break;
            }
    }

    #region Postmail Events
    void Common_Spam_Postmail()
    {
        //Debug.Log("You received SPAM."); //These are more SPAM than Phishing

        New_Notification(0, "Postmail", "You received SPAM."); 
    }

    int spamAmount = 10;

    float spamInterval = 10;
    IEnumerator Rare_ConstantSpam_Postmail()
    {
        for (int i = 0; i < spamAmount; i++)
        {
            New_Notification(0, "Postmail", "You received SPAM.");

            yield return new WaitForSeconds(spamInterval);
        }

    }

    #endregion

    #region 

    float appCrashTime = 2.5f;
    IEnumerator Rare_CrashApp()
    {
        //Debug.Log("Crashing " + App_Basic.CurrentApp.gameObject.name + "...");

        GameObject CurrentApp = App_Basic.CurrentApp.Pop();
        Vector3 App_ClosedPoint = App_Basic.App_ClosedPoint;
        float TransitionTime = 0.5f;

        CurrentApp.transform.DOMove(App_ClosedPoint, TransitionTime).SetEase(Ease.OutCubic);
        CurrentApp.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), TransitionTime).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(appCrashTime);

        CurrentApp.transform.DOScale(Vector3.zero, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            CurrentApp.gameObject.SetActive(false);
                
            New_Notification(0, "Oops!", "The app you were using crashed."); 
        });
    }

    #endregion
    void Common_DisconnectWiFi()
    {
        Phone_Statistics.isWifiConnected = false;
        App_Settings.DisconnectToWifi();
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
    
    // EVENTS CAN STACK, so multiple events can happen in a single trigger
}
