using DG.Tweening;
using TMPro;
using UnityEngine;

/*
 * HEY READ THE READFIRST.CS SCRIPT INSIDE THE SAME
 * FOLDER AS THIS ONE BEFORE YOU DO ANYTHING THANKS <3
 */

/// <summary>
/// THE NOTIFICATION TRIGGERS EVERYTIME A GAME OBJECT IS OPEN OR SMTHIN YEAH!
/// </summary>

public class Notes_New_Task_Announcement : MonoBehaviour
{
    private void OnEnable()
    {
        TextMeshProUGUI titleTMP = transform.Find("Title").GetComponent<TextMeshProUGUI>();

        GameObject notif = GameObject.FindWithTag("Notification");
        if (notif != null)
        {
            TextMeshProUGUI nameNotifTMP = notif.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descNotifTMP = notif.transform.Find("Desc").GetComponent<TextMeshProUGUI>();

            if (titleTMP != null && nameNotifTMP != null && descNotifTMP != null)
            {
                nameNotifTMP.text = "Notes - New Task";
                descNotifTMP.text = titleTMP.text;
            }

            //NOTIF POP UP THANGGG
            Transform showPos = GameObject.Find("NOTIF SHOW POSITION").transform;
            Transform hidePos = GameObject.Find("NOTIF HIDE POSITION").transform;

            if (showPos != null && hidePos != null)
            {
                notif.transform.DOMove(showPos.position, 0.5f).OnComplete(() =>
                {
                    //Just waiting for a while
                    DOVirtual.DelayedCall(3f, () => notif.transform.DOMove(hidePos.position, 0.5f));
                });
            }
        }
    }
}
