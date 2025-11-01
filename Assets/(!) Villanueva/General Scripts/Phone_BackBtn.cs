using UnityEngine;
using DG.Tweening;

public class Phone_BackBtn : MonoBehaviour
{
    public void PrevPage()
    {
        if (App_Pages.Page_History.Count > 0)
        {
            GameObject PreviousPage = App_Pages.Page_History.Pop();
            GameObject CurrentPage = App_Pages.Curr_Page.gameObject;
            Vector3 ClosedPoint = App_Pages.BackBtnPage_ClosedPosition;
            Vector3 OpenedPoint = App_Pages.BackBtnPage_OpenedPosition;
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
                App_Pages.Curr_Page = PreviousPage;
            });
        }
        
    }
}
