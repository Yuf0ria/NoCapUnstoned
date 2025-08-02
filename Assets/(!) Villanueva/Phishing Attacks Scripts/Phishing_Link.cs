using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Phishing_Link : MonoBehaviour
{
    public void LowSeverity(GameObject GameOverPanel)
    {
        GameOverPanel.transform.localPosition = ClosedPoint;
        GameOverPanel.gameObject.SetActive(true);

        GameOverPanel.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }

    Vector3 ClosedPoint = new Vector3(0, 2500, 0);
    Vector3 OpenedPoint = new Vector3(0, 0, 0);
    float TransitionTime = 0.5f;

    public void HighSeverity(GameObject GameOverPanel)
    {
        GameOverPanel.transform.localPosition = ClosedPoint;
        GameOverPanel.gameObject.SetActive(true);

        GameOverPanel.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }
}
