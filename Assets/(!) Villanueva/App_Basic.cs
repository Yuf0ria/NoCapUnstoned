using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class App_Basic : MonoBehaviour
{
    [SerializeField] GameObject AppIcon;
    [SerializeField] GameObject Position; //Place STANDARD POSITION GameObject here!

    //Make a General App that contains general functions like Open and Close
    //Then make a specific App you want to Open and Close that you drop into the slot

    void Start() //Make All Panels InActive to save on Performance Slots
    {
        App_ClosedPoint = AppIcon.transform.position;

        transform.position = App_ClosedPoint;
        transform.localScale = ClosedScale;
        OpenedPoint = Position.transform.position;
        // ^^^^
        //For the position of the app! (I MADE THIS SCRIPT - NICAIA)
        //MORE SIDE NOTES:
        //This only saves the position during the start which means if the player resizes-
        //the game window, it will mess up the position.
    }


    //Vector3 App_ClosedPoint;
    Vector3 ClosedScale = new Vector3(0, 0, 0);
    Vector3 OpenedPoint; //= new Vector3(540, 960, 0);
    Vector3 OpenScale = new Vector3(1, 1, 1);
    float TransitionTime = 0.5f;

    public static GameObject CurrentApp;
    public static Vector3 App_ClosedPoint;

    private void Update() //To update the position everytime the window is resized. I might find a better way idk but this will do :P - Nicaia
    {
        this.gameObject.transform.position = Position.transform.position;
        OpenedPoint = Position.transform.position;
    }

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
