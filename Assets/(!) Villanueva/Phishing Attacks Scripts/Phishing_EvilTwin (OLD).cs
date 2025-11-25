using UnityEngine;
using DG.Tweening;

public class Phishing_EvilTwin : MonoBehaviour
{
    
    Vector3 ClosedPoint = new Vector3(0, 2500, 0);
    Vector3 OpenedPoint = new Vector3(0, 0, 0);
    float TransitionTime = 0.5f;
    public void SuccefulConnection(GameObject GameOverPanel)
    {
        GameOverPanel.transform.localPosition = ClosedPoint;
        GameOverPanel.gameObject.SetActive(true);

        GameOverPanel.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }
}
