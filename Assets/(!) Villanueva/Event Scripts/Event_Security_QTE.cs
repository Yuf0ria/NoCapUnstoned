using UnityEngine;
using DG.Tweening;

public class Event_Security_QTE : MonoBehaviour
{
    public GameObject shrink;
    float ShrinkScale = 1;
    float ShrinkTime = 2.5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Shrink()
    {
        transform.DOScale(ShrinkScale, ShrinkTime).SetEase(Ease.OutCubic);
    }

    // First IEnumerate to trigger the wait time before the player can "Succeed" the QTE
    
    // Second IEnumerate Brief Window of Successful QTE
}
