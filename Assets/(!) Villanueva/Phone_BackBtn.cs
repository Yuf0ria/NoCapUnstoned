using UnityEngine;
using DG.Tweening;

public class Phone_BackBtn : MonoBehaviour
{
    public void PrevPage()
    {
        GameObject PreviousPage = App_Pages.BackBtn_PrevPage.gameObject;
        GameObject CurrentPage = App_Pages.Curr_Page.gameObject;
        Vector3 ClosedPoint = App_Pages.BackBtnPage_ClosedPosition;
        float TransitionTime = 0.5f;
        
        PreviousPage.gameObject.SetActive(true);

        CurrentPage.transform.DOLocalMove(ClosedPoint, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            CurrentPage.gameObject.SetActive(false);
        });
    }
}
