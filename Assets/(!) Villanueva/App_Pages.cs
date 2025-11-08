using UnityEngine;
using DG.Tweening;

public class App_Pages : MonoBehaviour
{
    public static Vector3 BackBtnPage_ClosedPosition;
    [SerializeField] Vector3 Page_ClosedPosition = new Vector3(0, -2500, 0);
    [SerializeField] float TransitionTime = 0.5f;


    public static GameObject Curr_Page;
    public static GameObject BackBtn_PrevPage;
    public GameObject Prev_Page;
    public void NextPage(GameObject Next_Page)
    {
        Curr_Page = Next_Page.gameObject;
        BackBtn_PrevPage = this.gameObject;

        BackBtnPage_ClosedPosition = Page_ClosedPosition;

        Next_Page.transform.localPosition = Page_ClosedPosition;
        Next_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        Next_Page.transform.DOLocalMove(Vector3.zero, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void PrevPage()
    {
        Curr_Page = Prev_Page;
        
        Prev_Page.gameObject.SetActive(true);

        transform.DOLocalMove(Page_ClosedPosition, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
