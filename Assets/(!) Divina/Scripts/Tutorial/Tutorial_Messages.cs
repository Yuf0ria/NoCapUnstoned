using TMPro;
using UnityEngine;
using DG.Tweening;

/*
 * HEY READ THE READFIRST.CS SCRIPT INSIDE THE SAME
 * FOLDER AS THIS ONE BEFORE YOU DO ANYTHING THANKS <3
 */

public class Tutorial_Messages : MonoBehaviour
{
    //WHEN YOU GET PHISHED
    [Header("Phishing Link")] //Aundee, I ended up doing what you did. //Update 1.11.2025: i wanna do smthing else better than this lol
    [SerializeField] private GameObject notif;
    [SerializeField] private Transform notifHide;
    [SerializeField] private Transform notifShow;

    public void showNotifPhishingLink(Software_Update_Function suf) //When you click on the phishing link that seallie tells you to do. Oopsies.
    {
        suf.hasUpdate = true;
        notif.transform.DOMove(notifShow.position, 0.5f);
    }

    public void hideNotifPhishingLink() //The thing Software Update gets when the update is complete
    {
        notif.transform.DOMove(notifHide.position, 0.5f); //It hides it lol
    }
}
