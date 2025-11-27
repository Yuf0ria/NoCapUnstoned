using DG.Tweening;
using UnityEngine;

public class App_Eduva : MonoBehaviour
{
    [Header("Eduva Page")]
    [SerializeField] private Transform eduvaClosedPosition; //= new Vector3(1250, -175, 0);
    [SerializeField] private Transform eduvaOpenedPosition; //= new Vector3(0, -175, 0);

    float TransitionTime = 0.5f; //Both has the same time transitions so...
    public float TransitionMult = 1f; //This is for the slowing down of the App

    /// <summary>
    /// Eduva Page
    /// </summary>

    
    public void OpenEduvaPage(GameObject EduvaPage)
    {
        Debug.Log("Opening Mail Thread: " + EduvaPage.name);

        EduvaPage.transform.position = eduvaClosedPosition.position;
        EduvaPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false); //Disabled this as it feels off setting the prev page inactive as the other goes in. - Nicaia

        EduvaPage.transform.DOMove(eduvaOpenedPosition.position, TransitionTime * TransitionMult).SetEase(Ease.OutCubic);
    }

    //I replaced the DOLocalMove to DOMove. I spent 3 hours figuring out what was going wrong. :P

    public void ReturnToEduvaMain(GameObject EduvaPage)
    {
        Debug.Log("Returning to Inbox...");

        EduvaPage.gameObject.SetActive(true);

        transform.DOMove(eduvaClosedPosition.position, TransitionTime * TransitionMult).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
    
}
