using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * HEY READ THE READFIRST.CS SCRIPT INSIDE THE SAME
 * FOLDER AS THIS ONE BEFORE YOU DO ANYTHING THANKS <3
 */

/// <summary>
/// THE NOTIFICATION TRIGGERS EVERYTIME A GAME OBJECT IS OPEN OR SMTHIN YEAH!
/// </summary>

public class Notes_New_Task_Announcement : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button buttonTrigger;

    [Header("Icon")]
    [SerializeField] private Sprite notesIcon;
    [SerializeField] private Sprite defaultIcon;

    // To be called when the task is completed. But this would vary depending on the task,
    // but it should mostly just be buttons.

    private void OnEnable()
    {
        TextMeshProUGUI titleTMP = transform.Find("Title").GetComponent<TextMeshProUGUI>();

        //Notification gameobject
        GameObject notif = GameObject.FindWithTag("Notification");

        if (notif != null)
        {
            TextMeshProUGUI nameNotifTMP = notif.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descNotifTMP = notif.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
            Image iconNotif = notif.transform.Find("Icon").GetComponent<Image>();

            Button notifButton = notif.GetComponent<Button>();
            notifButton.interactable = true;            
            notifButton.onClick.AddListener(quickHide);

            //CHANGING TEXT BASED ON THE TASK
            if (titleTMP != null && nameNotifTMP != null && descNotifTMP != null)
            {
                nameNotifTMP.text = "Notes - New Task";
                descNotifTMP.text = titleTMP.text;
                iconNotif.sprite = notesIcon;
            }

            //NOTIF POP UP THANGGG
            Transform showPos = GameObject.Find("NOTIF SHOW POSITION").transform;
            Transform hidePos = GameObject.Find("NOTIF HIDE POSITION").transform;

            if (showPos != null && hidePos != null)
            {
                notif.transform.DOMove(showPos.position, 0.5f).OnComplete(() =>
                {
                    //Just waiting for a while
                    DOVirtual.DelayedCall(3f, () => notif.transform.DOMove(hidePos.position, 0.5f).OnComplete(() =>
                    {
                        iconNotif.sprite = defaultIcon;
                        notifButton.interactable = false;
                        notifButton.onClick.RemoveListener(quickHide);
                    }));
                });
            }
        }

        //Get the button of the notif game object
        
        
        if (buttonTrigger != null)
        buttonTrigger.onClick.AddListener(goalTriggered);
    }

    public void quickHide()
    {
        GameObject notif = GameObject.FindWithTag("Notification");
        Button notifButton = notif.GetComponent<Button>();

        Transform showPos = GameObject.Find("NOTIF SHOW POSITION").transform;
        Transform hidePos = GameObject.Find("NOTIF HIDE POSITION").transform;

        Image iconNotif = notif.transform.Find("Icon").GetComponent<Image>();
        iconNotif.sprite = notesIcon;

        // Hides automatically I think
        notif.transform.DOMove(hidePos.position, 0.5f).OnComplete(() =>
        {
            iconNotif.sprite = defaultIcon;
            notifButton.interactable = false;
            notifButton.onClick.RemoveListener(quickHide);
        });
    }

    public void goalTriggered()
    {
        //Changes the font style of the Notes preview to strikethrough to show that it's done
        TextMeshProUGUI noteTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI noteDesc = transform.Find("Description").GetComponent<TextMeshProUGUI>();

        noteTitle.fontStyle = FontStyles.Strikethrough;
        noteDesc.fontStyle = FontStyles.Strikethrough;
    }
}
