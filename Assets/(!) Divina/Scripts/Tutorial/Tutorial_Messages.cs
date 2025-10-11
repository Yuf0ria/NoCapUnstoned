using TMPro;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Messages : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Second Message")]
    [SerializeField] private GameObject secondMessage;

    public void secondMessageProgression()
    {
        secondMessage.SetActive(true);
        Tutorial_Event te;
        te = secondMessage.GetComponent<Tutorial_Event>();
        te.enabled = true;
    }

    [Header("Phishing Link")]
    [SerializeField] private GameObject notif;
    [SerializeField] private Transform notifHide;
    [SerializeField] private Transform notifShow;

    public void showNotifPhishingLink(Software_Update_Function suf)
    {
        suf.hasUpdate = true;
        notif.transform.DOMove(notifShow.position, 0.5f);
    }

    public void hideNotifPhishingLink()
    {
        notif.transform.DOMove(notifHide.position, 0.5f);
    }
}
