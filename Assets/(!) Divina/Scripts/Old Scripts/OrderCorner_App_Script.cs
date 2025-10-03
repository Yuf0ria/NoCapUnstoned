using DG.Tweening;
using UnityEngine;

public class OrderCorner_App_Script : MonoBehaviour
{
    Vector3 OrderCornerClosedPosition = new Vector3(0, -2500, 0);
    Vector3 OrderCornerOpenedPosition = new Vector3(0, 0, 0);
    float OrderCornerTransitionDuration = 0.5f;

    public void OpenOrderCornerCategory(GameObject OrderCornerCategoryPage)
    {
        Debug.Log("Opening OrderCorner Category: " + OrderCornerCategoryPage.name);

        OrderCornerCategoryPage.transform.localPosition = OrderCornerClosedPosition;
        OrderCornerCategoryPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        OrderCornerCategoryPage.transform.DOLocalMove(OrderCornerOpenedPosition, OrderCornerTransitionDuration).SetEase(Ease.OutCubic);
    }

    public void ReturnToOrderCornerMain(GameObject OrderCornerMainPage)
    {
        Debug.Log("Returning to Main OrderCorner Page...");

        OrderCornerMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(OrderCornerClosedPosition, OrderCornerTransitionDuration).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
