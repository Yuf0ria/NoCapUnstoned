using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class App_Gallery : MonoBehaviour
{

    [SerializeField] GameObject ImageView_Page;
    [SerializeField] Image ImageView;


    [SerializeField] private Transform gallleryClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform galleryOpenedPosition; //= new Vector3(0, -175, 0);
    float TransitionTime = 0.5f;

    public void ViewImage(GameObject Button)
    {
        Debug.Log("Viewing Image... " + Button.GetComponent<Image>().sprite);

        ImageView.sprite = Button.GetComponent<Image>().sprite;
        ImageView_Page.transform.position = galleryOpenedPosition.position;
        ImageView_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        ImageView_Page.transform.DOMove(galleryOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToMain(GameObject PrevPage)
    {
        Debug.Log("Returning to Gallery...");

        PrevPage.gameObject.SetActive(true);

        transform.DOMove(gallleryClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
