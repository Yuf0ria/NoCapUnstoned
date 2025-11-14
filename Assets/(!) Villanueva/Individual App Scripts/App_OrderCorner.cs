using DG.Tweening;
using UnityEngine;

public class App_OrderCorner : MonoBehaviour
{
    [Header("Order Corner Page")]
    [SerializeField] private Transform orderCornerClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform orderCornerOpenedPosition; //= new Vector3(0, -175, 0);

    float TransitionTime = 0.5f; //Both has the same time transitions so...
    public float TransitionMult = 1f; //This is for the slowing down of the App

    /// <summary>
    /// Order Corner Page
    /// </summary>

    public void OpenOrderCornerPage(GameObject OrderCornerPage)
    {
        Debug.Log("Opening Mail Thread: " + OrderCornerPage.name);

        OrderCornerPage.transform.position = orderCornerClosedPosition.position;
        OrderCornerPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); //Disabled this as it feels off setting the prev page inactive as the other goes in. - Nicaia

        OrderCornerPage.transform.DOMove(orderCornerOpenedPosition.position, TransitionTime * TransitionMult).SetEase(Ease.OutCubic);
    }

    //I replaced the DOLocalMove to DOMove. I spent 3 hours figuring out what was going wrong. :P

    public void ReturnToOrderCornerMain(GameObject OrderCornerPage)
    {
        Debug.Log("Returning to Inbox...");

        OrderCornerPage.gameObject.SetActive(true);

        transform.DOMove(orderCornerClosedPosition.position, TransitionTime * TransitionMult).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
