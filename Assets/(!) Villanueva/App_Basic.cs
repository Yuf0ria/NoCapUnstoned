using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class App_Basic : MonoBehaviour
{
    [SerializeField] GameObject AppIcon;

    //Make a General App that contains general functions like Open and Close
    //Then make a specific App you want to Open and Close that you drop into the slot

    void Start() //Make All Panels InActive to save on Performance Slots
    {
        App_ClosedPoint = AppIcon.transform.position;

        transform.position = App_ClosedPoint;
        transform.localScale = ClosedScale;
    }


    //Vector3 App_ClosedPoint;
    Vector3 ClosedScale = new Vector3(0, 0, 0);
    Vector3 OpenedPoint = new Vector3(540, 960, 0);
    Vector3 OpenScale = new Vector3(1, 1, 1);
    float TransitionTime = 0.5f;

    public static GameObject CurrentApp;
    public static Vector3 App_ClosedPoint;

    public void OpenApp()
    {
        Debug.Log("Opening " + this.gameObject.name + "...");

        this.gameObject.SetActive(true);
        transform.DOMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
        transform.DOScale(OpenScale, TransitionTime).SetEase(Ease.OutCubic);

        CurrentApp = this.gameObject;
        App_ClosedPoint = AppIcon.transform.position;
    }

}
