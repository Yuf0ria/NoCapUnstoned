using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class App_Gallery : MonoBehaviour
{

    [SerializeField] GameObject ImageView_Page;
    [SerializeField] Image ImageView;


    Vector3 ClosedPoint = new Vector3(0, -2500, 0);
    Vector3 OpenedPoint = new Vector3(0, 0, 0);
    float TransitionTime = 0.5f;

    public void ViewImage(GameObject Button)
    {
        Debug.Log("Viewing Image... " + Button.GetComponent<Image>().sprite);

        ImageView.sprite = Button.GetComponent<Image>().sprite;
        ImageView_Page.transform.localPosition = ClosedPoint;
        ImageView_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        ImageView_Page.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToMain(GameObject PrevPage)
    {
        Debug.Log("Returning to Gallery...");

        PrevPage.gameObject.SetActive(true);

        transform.DOLocalMove(ClosedPoint, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
