using UnityEngine;
using DG.Tweening;
using UnityEditorInternal;
using System.Collections.Generic;

public class App_Basic : MonoBehaviour
{
    [SerializeField] bool ChildActiveOnStart;
    [SerializeField] bool ChildOfApp;
    [SerializeField] GameObject AppIcon;

    //Make a General App that contains general functions like Open and Close
    //Then make a specific App you want to Open and Close that you drop into the slot

    void Start() //Make All Panels InActive to save on Performance Slots
    {
        ClosedPoint = AppIcon.transform.position;

        if (!ChildOfApp)
        {
            transform.position = ClosedPoint;
            transform.localScale = ClosedScale;
        }

        if (!ChildActiveOnStart) this.gameObject.SetActive(false);
    }



    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            BackButtonPressed = false;
        }
    }





    Vector3 ClosedPoint;
    Vector3 ClosedScale = new Vector3(0, 0, 0);
    Vector3 OpenedPoint = new Vector3(540, 960, 0);
    Vector3 OpenScale = new Vector3(1, 1, 1);
    float TransitionTime = 0.5f;

    public void OpenApp()
    {
        Debug.Log("Opening " + this.gameObject.name + "...");

        this.gameObject.SetActive(true);
        transform.DOMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
        transform.DOScale(OpenScale, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void CloseApp()
    {
        Debug.Log("Closing " + this.gameObject.name + "...");

        transform.DOMove(ClosedPoint, TransitionTime).SetEase(Ease.OutCubic);
        transform.DOScale(ClosedScale, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }



    // Note: This section is going to be transferred to a different Script. 
    // App_Basic, handles the main common funtions such as opening and closing the App
    // App_Delivery.cs or App_Variants.cs, will hold their own PanelHistory functions and their own
    // Unique static PanelHistory, ex: App_Delivery_PanelHistory

    // If it remains in App_Basic, it will merge all app histories into one Stack.
    // Aundee will fix soon. Ty! ^o(Y)o^ <--- I made a lil cat.
    static Stack<GameObject> PanelHistory = new Stack<GameObject>();
    static int PanelHsitoryCountAtPress = -1;
    static bool BackButtonPressed = false;

    [SerializeField] GameObject NextPanel;
    
    public void BackToPrevPanel()
    {

        if (!ChildOfApp || !this.gameObject.activeSelf) return;

        // Prevents multiple runs of BackToPrevPanel per button press :D CODING IS FUCKEN HARD

        if (BackButtonPressed) return; // It checks if it is TRUE, but if its the first time, its false
        BackButtonPressed = true;      // Then immediately makes it TRUE to prevent multiple runs.

        // It only lets through once, and it can be reset by releasing the mouse button, detected by Void Update

        if (PanelHsitoryCountAtPress != PanelHistory.Count)
        {
            PanelHsitoryCountAtPress = PanelHistory.Count;

            if (PanelHistory.Count > 0)
            {
                GameObject PrevPanel = PanelHistory.Pop();
                Debug.Log("Switched to: " + PrevPanel.name + " | Stack count now: " + PanelHistory.Count);

                this.gameObject.SetActive(false);
                PrevPanel.SetActive(true);
            }
            else
            {
                Debug.Log("Panel History is empty!");
            }
        }
    }

    public void GoToNextPanel()
    {
        PanelHistory.Push(this.gameObject);

        Debug.Log(PanelHistory.Peek() + " , " + PanelHistory.Count);

        NextPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
