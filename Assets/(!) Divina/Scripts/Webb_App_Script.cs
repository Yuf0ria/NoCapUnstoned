using DG.Tweening;
using UnityEngine;

public class Webb_App_Script : MonoBehaviour
{
    Vector3 WebbClosedPosition = new Vector3(0, -2500, 0);
    Vector3 WebbOpenedPosition = new Vector3(0, 0, 0);
    float WebbTransitionDuration = 0.5f;

    public void OpenWebbCategory(GameObject WebbCategoryPage)
    {
        Debug.Log("Opening Webb Category: " + WebbCategoryPage.name);

        WebbCategoryPage.transform.localPosition = WebbClosedPosition;
        WebbCategoryPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        WebbCategoryPage.transform.DOLocalMove(WebbOpenedPosition, WebbTransitionDuration).SetEase(Ease.OutCubic);
    }

    public void ReturnToWebbMain(GameObject WebbMainPage)
    {
        Debug.Log("Returning to Main Webb Page...");

        WebbMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(WebbClosedPosition, WebbTransitionDuration).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
