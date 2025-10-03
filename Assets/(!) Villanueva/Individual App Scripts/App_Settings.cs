using UnityEngine;
using DG.Tweening;
using TMPro;

public class App_Settings : MonoBehaviour
{

    [SerializeField] private Transform orderCornerClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform orderCornerOpenedPosition; //= new Vector3(0, -175, 0);
    float TransitionTime = 0.5f;

    public void OpenSettingsCategory(GameObject SettingsCategoryPage)
    {
        Debug.Log("Opening Settings Category: " + SettingsCategoryPage.name);

        SettingsCategoryPage.transform.position = orderCornerClosedPosition.position;
        SettingsCategoryPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false);

        SettingsCategoryPage.transform.DOMove(orderCornerOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToSettingsMain(GameObject SettingsMainPage)
    {
        Debug.Log("Returning to Main Settings Page...");

        SettingsMainPage.gameObject.SetActive(true);

        transform.DOMove(orderCornerClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });
    }

    public TMP_Text CurrentWiFi;
    public void ConnectToWifi(TMP_Text NewWiFi)
    {
        CurrentWiFi.text = NewWiFi.GetComponentInChildren<TMP_Text>().text;
    }
}
