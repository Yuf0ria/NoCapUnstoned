using UnityEngine;

public class Event_Manager : MonoBehaviour
{

    static int RandomVal1;
    static int RandomVal2;


    // Number of Events per category
    static int numOfCommon = 5;
    static int numOfRare;
    static int numOfRandom;

    public static void Run_Event()
    {
        Debug.Log("Running Event Method");

        RandomVal1 = Random.Range(0, 99);

        //Common Events
        if (RandomVal1 % 3 == 0) // Check if the remainder is 0, which is true about 1/3 of the time
        {
            RandomVal2 = Random.Range(0, numOfCommon - 1);

            switch (RandomVal2)
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
        if (RandomVal1 % 10 == 0) // Check if the remainder is 0, which is true about 1/10 of the time
        {

        }

        Debug.Log(RandomVal1 + " : " + RandomVal2);

    }

    static void Common_Real_Postmail()
    {
        Debug.Log("You recieve a real email! :)");
    }

    static void Common_Phishing_Postmail()
    {
        Debug.Log("You recieve a phishing email! :(");
    }

    static void Common_Fake_Friendlink()
    {
        Debug.Log("A phished account posted on Friendlink.");
    }

    static void Common_Spam_Postmail()
    {
        Debug.Log("You received SPAM."); //These are more SPAM than Phishing
    }

    static void Common_Fake_Message()
    {
        Debug.Log("A phished account sent a Message.");
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
