using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;


public class App_Pages : MonoBehaviour
{
    
    public static Vector3 BackBtnPage_ClosedPosition;
    public static Vector3 BackBtnPage_OpenedPosition;

    [SerializeField] private bool isStartingPage;
    [SerializeField] private bool inheritParentTransform;

    [SerializeField] private bool disablePhoneBackButton;

    [SerializeField] private float phoneTopOffset = -1251.3f;
    [SerializeField] private float headerTopOffset = -430f;
    [SerializeField] private float pivotOffset = 0.5f;
    [SerializeField] private Transform CurrPage_ClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform NextPage_ClosedPosition; //= new Vector3(0, -175, 0);
    [SerializeField] private Transform NextPage_OpenedPosition; //= new Vector3(0, -175, 0);
    [SerializeField] float TransitionTime = 0.5f;

    /*
    void Start()
    {
        if (!isStartingPage)
        {
            this.gameObject.transform.position = CurrPage_ClosedPosition.position;
            this.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if(this.isActiveAndEnabled && inheritParentTransform) //This is to center certain "Page Children" onto their Parents
        {
            
            RectTransform childRect = GetComponent<RectTransform>();

            childRect.offsetMin = new Vector2(0, phoneTopOffset);
            childRect.offsetMax = new Vector2(0, phoneTopOffset + headerTopOffset);
            childRect.pivot = new Vector2(0.5f, pivotOffset);
            childRect.anchoredPosition = Vector2.zero;
            
        }
    }
    */
    public static GameObject Curr_Page;
    public static GameObject BackBtn_PrevPage;
    public GameObject Prev_Page;
    public static Stack<GameObject> Page_History = new Stack<GameObject>();

    /*
    public void NextPage(GameObject Next_Page)
    {
        Curr_Page = Next_Page.gameObject;
        BackBtn_PrevPage = this.gameObject;
        Page_History.Push(BackBtn_PrevPage);
        Debug.Log("Added " + BackBtn_PrevPage.name + " to history.");

        BackBtnPage_ClosedPosition = NextPage_ClosedPosition.position;
        BackBtnPage_OpenedPosition = this.gameObject.transform.position;

        Next_Page.transform.position = NextPage_ClosedPosition.position;
        Next_Page.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        Next_Page.transform.DOMove(NextPage_OpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);

        Phone_Statistics.disablePhoneBackButton = disablePhoneBackButton;
    }

    public void PrevPage()
    {
        if (Page_History.Count > 0)
        {
            GameObject PreviousPage = Page_History.Pop();
            GameObject CurrentPage = Curr_Page.gameObject;
            Vector3 ClosedPoint = BackBtnPage_ClosedPosition;
            Vector3 OpenedPoint = BackBtnPage_OpenedPosition;
            float TransitionTime = 0.5f;

            Debug.Log(PreviousPage.name);
            PreviousPage.gameObject.SetActive(true);
            PreviousPage.transform.DOMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
            PreviousPage.transform.SetAsFirstSibling();

            CurrentPage.transform.DOMove(ClosedPoint, TransitionTime).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                CurrentPage.gameObject.SetActive(false);
                //PreviousPage.transform.SetAsFirstSibling();
                Curr_Page = PreviousPage;
            });
        }
    }
    */
}
