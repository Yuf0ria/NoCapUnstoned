using UnityEngine;
using DG.Tweening;

public class Phone_ExitBtn : MonoBehaviour
{
    public void CloseApp()
    {
        Debug.Log("Closing " + App_Basic.CurrentApp.gameObject.name + "...");

        GameObject CurrentApp = App_Basic.CurrentApp.gameObject;
        Vector3 App_ClosedPoint = App_Basic.App_ClosedPoint;
        float TransitionTime = 0.5f;

        CurrentApp.transform.DOMove(App_ClosedPoint, TransitionTime).SetEase(Ease.OutCubic);
        CurrentApp.transform.DOScale(Vector3.zero, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
                //CurrentApp.gameObject.SetActive(false);
                //Had to disable this cause it won't active the notifs everytime a new task is made
        });
        
    }
}
