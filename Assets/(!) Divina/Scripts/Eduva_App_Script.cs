using DG.Tweening;
using UnityEngine;

public class Eduva_App_Script : MonoBehaviour
{
    Vector3 EduvaClosedPosition = new Vector3(0, -2500, 0);
    Vector3 EduvaOpenedPosition = new Vector3(0, 0, 0);
    float EduvaTransitionDuration = 0.5f;

    public void OpenEduvaCategory(GameObject EduvaCategoryPage)
    {
        Debug.Log("Opening Eduva Category: " + EduvaCategoryPage.name);

        EduvaCategoryPage.transform.localPosition = EduvaClosedPosition;
        EduvaCategoryPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        EduvaCategoryPage.transform.DOLocalMove(EduvaOpenedPosition, EduvaTransitionDuration).SetEase(Ease.OutCubic);
    }

    public void ReturnToEduvaMain(GameObject EduvaMainPage)
    {
        Debug.Log("Returning to Main Eduva Page...");

        EduvaMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(EduvaClosedPosition, EduvaTransitionDuration).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
