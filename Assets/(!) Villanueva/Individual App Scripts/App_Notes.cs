using UnityEngine;
using DG.Tweening;

public class App_Notes : MonoBehaviour
{
    [Header("Order Corner Page")]
    [SerializeField] private Transform notesClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform notesOpenedPosition; //= new Vector3(0, -175, 0);

    float TransitionTime = 0.5f; //Both has the same time transitions so...

    /// <summary>
    /// Order Corner Page
    /// </summary>

    public void OpenNotesPage(GameObject notesPage)
    {
        Debug.Log("Opening Mail Thread: " + notesPage.name);

        notesPage.transform.position = notesClosedPosition.position;
        notesPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); //Disabled this as it feels off setting the prev page inactive as the other goes in. - Nicaia

        notesPage.transform.DOMove(notesOpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    // Future Optimisation, Make it so that ViewNotes Page is just a single page 
    // instead of indivual Note#s. This is so that you can structure it similar to Gallery and ViewImage
    //hat you need to figure out is how to store the data for the Body without it being seen
    // on the button so it can retrieve the info when clicked
    // This is also so that back button isnt overloaded by a function per Note

    public void ReturnToNotesMain(GameObject notesPage)
    {
        Debug.Log("Returning to Inbox...");

        notesPage.gameObject.SetActive(true);

        transform.DOMove(notesClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
        
    //Vector3 ClosedPoint = new Vector3(0, -2500, 0);
    //Vector3 OpenedPoint = new Vector3(0, 0, 0);
    //float TransitionTime = 0.5f;

    //public void ViewNote(GameObject Note_Page)
    //{
    //    Debug.Log("Opening... " + Note_Page);

    //    Note_Page.transform.localPosition = ClosedPoint;
    //    Note_Page.gameObject.SetActive(true);

    //    this.gameObject.SetActive(false);

    //    Note_Page.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    //}


    //// Future Optimisation, Make it so that ViewNotes Page is just a single page 
    //// instead of indivual Note#s. This is so that you can structure it similar to Gallery and ViewImage
    //// What you need to figure out is how to store the data for the Body without it being seen
    //// on the button so it can retrieve the info when clicked
    //// This is also so that back button isnt overloaded by a function per Note

    //public void ReturnToMain(GameObject PrevPage)
    //{
    //    Debug.Log("Returning to Notes...");

    //    PrevPage.gameObject.SetActive(true);

    //    transform.DOLocalMove(ClosedPoint, TransitionTime).SetEase(Ease.OutCubic)
    //    .OnComplete(() =>
    //    {
    //        this.gameObject.SetActive(false);
    //    });
    //}
}
