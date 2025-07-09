using UnityEngine;
using DG.Tweening;

public class App_Settings : MonoBehaviour
{
    Vector3 SettingsClosedPosition = new Vector3(0, -2500, 0);
    Vector3 SettingsOpenedPosition = new Vector3(0, 0, 0);
    float SettingsTransitionDuration = 0.5f;

    public void OpenSettingsCategory(GameObject SettingsCategoryPage)
    {
        Debug.Log("Opening Settings Category: " + SettingsCategoryPage.name);

        SettingsCategoryPage.transform.localPosition = SettingsClosedPosition;
        SettingsCategoryPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        SettingsCategoryPage.transform.DOLocalMove(SettingsOpenedPosition, SettingsTransitionDuration).SetEase(Ease.OutCubic);
    }

    public void ReturnToSettingsMain(GameObject SettingsMainPage)
    {
        Debug.Log("Returning to Main Settings Page...");

        SettingsMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(SettingsClosedPosition, SettingsTransitionDuration).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
