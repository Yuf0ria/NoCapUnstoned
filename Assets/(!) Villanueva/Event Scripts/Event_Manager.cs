using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Event_Manager : MonoBehaviour
{
    [Header("Notification")]
    [SerializeField] RectTransform Notif_Rect;
    [SerializeField] GameObject Notif_Icon;
    [SerializeField] GameObject Notif_Name;
    [SerializeField] GameObject Notif_Desc;
    [SerializeField] Sprite[] Icons;

    [Header("GAME OVER")]
    [SerializeField] RectTransform gameOverPanel;
    [SerializeField] TextMeshProUGUI gameOverTitle;
    [SerializeField] TextMeshProUGUI gameOverCause;
    [SerializeField] TextMeshProUGUI gameOverAdvice;
    [SerializeField] TextMeshProUGUI gameOverStats;
    [SerializeField] Transform revealPos;
    [SerializeField] Transform hidePos;

    [Header("GAME WIN")]
    [SerializeField] RectTransform gameWinPanel;
    [SerializeField] Slider gameTimer;
    [SerializeField] Chapter1Events chapter1Events;
    [SerializeField] EventScript progress;
    [SerializeField] int maxTasks = 10;
    


    Vector3 Notif_HideTransform;
    Vector3 Notif_ShowTransform;
    float TransitionTime = 1;
    float ShowTime = 5;



    // Number of Events per category
    [SerializeField] private int numOfRandom = 3;
    public float TransitionMult = 1f; //This is for the slowing down of the App

    void Start()
    {
        Notif_HideTransform = new Vector3(0, 1100, 0);
        Notif_ShowTransform = new Vector3(0, 845, 0);

        Notif_Rect.localPosition = Notif_HideTransform;

        gameOverPanel.position = hidePos.position;
        gameWinPanel.position = hidePos.position;
    }
    
    void Update()
    {
        SecurityStats();

        //Run_GameWin();
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
        switch (UnityEngine.Random.Range(0, numOfRandom - 1))
            {
                case 1: // Postmail SPAM
                    Common_Spam_Postmail();
                    break;

                case 3:
                    if (Phone_Statistics.isCompromised) StartCoroutine(Rare_ConstantSpam_Postmail());
                    break;

                case 4: // App Crashes (Exits App Slowly)
                    if (Phone_Statistics.isCompromised) StartCoroutine(Rare_CrashApp());
                    break;

                case 5: // SLOW THE APPS
                    if (Phone_Statistics.isCompromised) ChangeTransitionTime(2*Phone_Statistics.numLowSeverity);
                    break;

                default:
                    Common_DisconnectWiFi();
                    break;
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

                case 4: // SLOW THE APPS
                    if (!Phone_Statistics.isAntiVirus) ChangeTransitionTime(2*Phone_Statistics.numLowSeverity);
                    break;

                case 5: // For Button that will add One Low Severity Attack, that checks for | Security Up to Date
                    if (!Phone_Statistics.isSecurityUpToDate) Phone_Statistics.numLowSeverity++;
                    break;

                case 6: // For Button that will add One Low Severity Attack, that checks for | AntiVirus
                    if (!Phone_Statistics.isAntiVirus) Phone_Statistics.numLowSeverity++;
                    break;

                case 7: // For Button that will add One High Severity Attack, that checks for | Security Up to Date
                    if (!Phone_Statistics.isSecurityUpToDate) Phone_Statistics.numHighSeverity++;
                    break;

                case 8: // For Button that will add One High Severity Attack, that checks for | AntiVirus
                    if (!Phone_Statistics.isAntiVirus) Phone_Statistics.numHighSeverity++;
                    break;

                case 9: // Removes one Low Severity Attack
                    Phone_Statistics.numLowSeverity--;
                    break;

                case 10: // Removes one High Severity Attack
                    Phone_Statistics.numHighSeverity--;
                    break;

                case 11: // Adds one Low Severity Attack
                    Phone_Statistics.numLowSeverity++;
                    break;

                case 12: // Adds one High Severity Attack
                    Phone_Statistics.isTwoFactorAuthentication = !Phone_Statistics.isTwoFactorAuthentication;
                    break;

                case 13: // Turn On/Off AntiVirus 
                    Phone_Statistics.isAntiVirus = !Phone_Statistics.isAntiVirus;
                    break;

                case 14: // Turn On/Off AdBlocker
                    Phone_Statistics.isAdBlocker = !Phone_Statistics.isAdBlocker;
                break;

            default:
                    Common_DisconnectWiFi();
                    break;
            }
    }

    #region Statistics Management
    public void SecurityStats()
    {
        if (Phone_Statistics.numLowSeverity == 0 && Phone_Statistics.numHighSeverity == 0)
        {
            Phone_Statistics.isCompromised = false;

            if (Phone_Statistics.isAdBlocker && Phone_Statistics.isTwoFactorAuthentication && Phone_Statistics.isAntiVirus && Phone_Statistics.isSecurityUpToDate)
                Phone_Statistics.isVulnerable = false;

            else Phone_Statistics.isVulnerable = true;
        }

        else
        {
            Phone_Statistics.isCompromised = true; 
        }


        if (!Phone_Statistics.isCompromised && TransitionMult != 1)
        {
            ChangeTransitionTime(1);
        }

        /*
        if(!Phone_Statistics.isGameOverRunning)
        {
        //  ITS THE FINAL COUNTDOWN
            if(Phone_Statistics.numLowSeverity > 0) 
            {
                Debug.Log("Start the 45 second Countdown");
                StartCoroutine(Run_GameOver(45));
                Phone_Statistics.isGameOverRunning = true;
            }
        }


        if(Phone_Statistics.numHighSeverity > 0) 
        {
            Debug.Log("Start the 20 second Countdown");
            StartCoroutine(Run_GameOver(20));
            Phone_Statistics.isGameOverRunning = true;
        }

        if(Phone_Statistics.numLowSeverity >= 5)
        {
            Debug.Log("INSTANT GAME OVER");
            StartCoroutine(Run_GameOver(5));
            Phone_Statistics.isGameOverRunning = true;
        }
        */
    }

    /*
    IEnumerator Run_GameOver(float TimeToGameOver)
    {
        yield return new WaitForSeconds(TimeToGameOver);
        if(Phone_Statistics.isCompromised || gameTimer.value >= 0.9990f)
        {
            // ADD GAME OVER STATISTICS HERE!!!
            // You will likely need different if statements here or lets just
            // use a general one
            int sum = Phone_Statistics.numLowSeverity + Phone_Statistics.numHighSeverity;

            gameOverCause.text = cause;

            gameOverAdvice.text = advice;

            gameOverStats.text = "You were hit with " + sum + " Phishing Attacks Before Game Over";

            //Debug.Log(Phone_Statistics.numLowSeverity + ", " + Phone_Statistics.numLowSeverity);

            gameOverPanel.transform.DOMove(revealPos.position, TransitionTime).SetEase(Ease.OutCubic);
        }

        else Debug.Log("Game Over Cancelled");
    }
    */


    public void Run_GameWin()
    {
        if(!Phone_Statistics.isGameOverRunning)
        { 
            int attackSum = Phone_Statistics.numLowSeverity + Phone_Statistics.numHighSeverity;

            if (progress.progression >= maxTasks && attackSum > 0) // Tasks Complete but was phished
            {
                    gameOverTitle.text = "Congratulations!";

                    gameOverCause.text = "Your final score is " + gameScore() + "!";

                    gameOverAdvice.text = "You completed " + progress.progression + " out of " + maxTasks + " tasks and were hit with " + attackSum + " Phishing Attacks Before Game Over.";

                    gameOverStats.text = advice;

                    Phone_Statistics.isGameOverRunning = true;
                    gameOverPanel.transform.DOMove(revealPos.position, TransitionTime).SetEase(Ease.OutCubic);

            }

            else if (progress.progression >= maxTasks && attackSum <= 0) // Tasks Complete wasn't phished
            {
                    gameOverPanel.gameObject.GetComponent<Image>().color = new Color(0f, 138f / 255f, 27f / 255f, 1f);

                    gameOverTitle.text = "Congratulations!";

                    gameOverCause.text = "Your final score is " + gameScore() + "!";

                    gameOverAdvice.text = "You completed " + progress.progression + " out of " + maxTasks + " tasks and were hit with " + attackSum + " Phishing Attacks Before Game Over.";

                    gameOverStats.text = "You successfully avoided all Phishing Attacks and completed all tasks! \n\n\n Thank you for playing!";

                    Phone_Statistics.isGameOverRunning = true;
                    gameOverPanel.transform.DOMove(revealPos.position, TransitionTime).SetEase(Ease.OutCubic);


            }
            else if (gameTimer.value >= 0.9990f)
            {

                    gameOverCause.text = "Your final score is " + gameScore() + "!";

                    gameOverAdvice.text = "You completed " + progress.progression + " out of " + maxTasks + " tasks and were hit with " + attackSum + " Phishing Attacks Before Game Over.";

                    gameOverStats.text = advice;

                    Phone_Statistics.isGameOverRunning = true; 
                    gameOverPanel.transform.DOMove(revealPos.position, TransitionTime).SetEase(Ease.OutCubic);
            }
        }
 
    }

    int gameScore()
    {
        int attacksScore = 2*Phone_Statistics.numLowSeverity + 5*Phone_Statistics.numHighSeverity;
        Debug.Log(attacksScore);

        int tasksScore = progress.progression;
        Debug.Log(progress.progression);

        int safetyScore = 0;
        if (Phone_Statistics.isSecurityUpToDate) safetyScore += 5;
        if (Phone_Statistics.isTwoFactorAuthentication) safetyScore += 5;
        if (Phone_Statistics.isAntiVirus) safetyScore += 5;
        if (Phone_Statistics.isAdBlocker) safetyScore += 5;
        if (!Phone_Statistics.isVulnerable) safetyScore += 10;

        
        Debug.Log(Phone_Statistics.isSecurityUpToDate + ", " + Phone_Statistics.isTwoFactorAuthentication + ", " + Phone_Statistics.isAntiVirus + ", " + Phone_Statistics.isAdBlocker + ", " + Phone_Statistics.isVulnerable);

        int finalScore = tasksScore + safetyScore - attacksScore;

        return finalScore;
    }

    #endregion

    string cause;
    string advice;
    string stats;

    public void SetGameOverCause(string c)
    {
        cause = c;
    }

    public void SetGameOverAdvice(string a)
    {
        advice = a;
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

    #region Slows or Crashes

    public void ChangeTransitionTime(float newTransitionMult)
    {

        App_Eduva[] Eduva = FindObjectsByType<App_Eduva>(FindObjectsSortMode.None);
        for(int i = 0; i < Eduva.Length; i++)
        {
            Eduva[i].TransitionMult = newTransitionMult;
        }


        App_FriendLink[] FriendLink = FindObjectsByType<App_FriendLink>(FindObjectsSortMode.None); 
        for(int i = 0; i < FriendLink.Length; i++)
        {
            FriendLink[i].TransitionMult = newTransitionMult;
        }


        App_Gallery[] Gallery = FindObjectsByType<App_Gallery>(FindObjectsSortMode.None); 
        for(int i = 0; i < Gallery.Length; i++)
        {
            Gallery[i].TransitionMult = newTransitionMult;
        }


        App_Messages[] Messages = FindObjectsByType<App_Messages>(FindObjectsSortMode.None); 
        for(int i = 0; i < Messages.Length; i++)
        {
            Messages[i].TransitionMult = newTransitionMult;
        }


        App_Notes[] Notes = FindObjectsByType<App_Notes>(FindObjectsSortMode.None); 
        for(int i = 0; i < Notes.Length; i++)
        {
            Notes[i].TransitionMult = newTransitionMult;
        }


        App_OrderCorner[] OrderCorner = FindObjectsByType<App_OrderCorner>(FindObjectsSortMode.None); 
        for(int i = 0; i < OrderCorner.Length; i++)
        {
            OrderCorner[i].TransitionMult = newTransitionMult;
        }


        App_Settings[] Settings = FindObjectsByType<App_Settings>(FindObjectsSortMode.None); 
        for(int i = 0; i < Settings.Length; i++)
        {
            Settings[i].TransitionMult = newTransitionMult;
        }


        App_Webb[] Webb = FindObjectsByType<App_Webb>(FindObjectsSortMode.None); 
        for(int i = 0; i < Webb.Length; i++)
        {
            Webb[i].TransitionMult = newTransitionMult;
        }

        TransitionMult = newTransitionMult;


    }

    float appCrashTime = 2.5f;
    IEnumerator Rare_CrashApp()
    {
        GameObject CurrentApp = App_Basic.CurrentApp.Pop();
        Vector3 App_ClosedPoint = App_Basic.App_ClosedPoint;
        float TransitionTime = 0.5f;

        CurrentApp.transform.DOMove(App_ClosedPoint, TransitionTime * TransitionMult).SetEase(Ease.OutCubic);
        CurrentApp.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), TransitionTime * TransitionMult).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(appCrashTime);

        CurrentApp.transform.DOScale(Vector3.zero, TransitionTime * TransitionMult).SetEase(Ease.OutCubic)
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
