using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class FoodPreparationSlider : MonoBehaviour
{
    public Slider preparationSlider;
    public float preparationTime = 10f; // Time in seconds for preparation

    public void StartPreparation()
    {
        StartCoroutine(FillSlider());
    }

    private IEnumerator FillSlider()
    {
        float elapsed = 0f;
        bool halfNotified = false;
        while (elapsed < preparationTime)
        {
            elapsed += Time.deltaTime;
            preparationSlider.value = elapsed / preparationTime;

            // Check for 50% completion
            if (!halfNotified && preparationSlider.value >= 0.5f)
            {
                halfNotified = true;
                ShowHalfwayNotification();
            }

            yield return null;
        }
        preparationSlider.value = 1f;
        // Trigger completion notification
        ShowCompletionNotification();
        Debug.Log("Food preparation complete!");
    }

    private void ShowHalfwayNotification()
    {
        GameObject notif = GameObject.FindWithTag("Notification");
        if (notif != null)
        {
            TextMeshProUGUI nameNotifTMP = notif.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descNotifTMP = notif.transform.Find("Desc").GetComponent<TextMeshProUGUI>();

            if (nameNotifTMP != null && descNotifTMP != null)
            {
                nameNotifTMP.text = "Order 50% done!";
                descNotifTMP.text = "Your order is now on the way there!";
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

    private void ShowCompletionNotification()
    {
        GameObject notif = GameObject.FindWithTag("Notification");
        if (notif != null)
        {
            TextMeshProUGUI nameNotifTMP = notif.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descNotifTMP = notif.transform.Find("Desc").GetComponent<TextMeshProUGUI>();

            if (nameNotifTMP != null && descNotifTMP != null)
            {
                nameNotifTMP.text = "Your order is here!";
                descNotifTMP.text = "Your driver is outside. Come and pick your food!";
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
