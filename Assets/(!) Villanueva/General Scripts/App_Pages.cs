using UnityEngine;
using DG.Tweening;

public class App_Pages : MonoBehaviour
{
    public static Vector3 BackBtnPage_ClosedPosition;

    [SerializeField] bool isStartingPage;
    [SerializeField] private Transform Page_ClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform Page_OpenedPosition; //= new Vector3(0, -175, 0);
    [SerializeField] float TransitionTime = 0.5f;

    void Start()
    {
        if (!isStartingPage)
        {
            this.gameObject.transform.position = Page_ClosedPosition.position;
            this.gameObject.SetActive(false);
        }

    }

    public static GameObject Curr_Page;
    public static GameObject BackBtn_PrevPage;
    public GameObject Prev_Page;
    public void NextPage(GameObject Next_Page)
    {
        Curr_Page = Next_Page.gameObject;
        BackBtn_PrevPage = this.gameObject;

        BackBtnPage_ClosedPosition = Page_ClosedPosition.position;

        Next_Page.transform.localPosition = Page_ClosedPosition.position;
        Next_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        Next_Page.transform.DOMove(Page_OpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void PrevPage()
    {
        Curr_Page = Prev_Page;
        
        Prev_Page.gameObject.SetActive(true);

        transform.DOMove(Page_ClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
