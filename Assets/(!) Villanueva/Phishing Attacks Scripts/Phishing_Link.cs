using UnityEngine;
using DG.Tweening;

public class Phishing_Link : MonoBehaviour
{
    public void LowSeverity(GameObject GameOverPanel)
    {   // Add the 3 minute Delay before checking the isCompromised is unchecked or rather there is No Low Severity Attacks Left
        // IF they fail the Check Game Over, if they succeed in one check but still compromised, add 1min 30sec.

        GameOverPanel.transform.localPosition = ClosedPoint;
        GameOverPanel.gameObject.SetActive(true);

        GameOverPanel.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }

    Vector3 ClosedPoint = new Vector3(0, 2500, 0);
    Vector3 OpenedPoint = new Vector3(0, 0, 0);
    float TransitionTime = 0.5f;

    public void HighSeverity(GameObject GameOverPanel)
    {   // Add the 10 Second Delay before Game Over
        GameOverPanel.transform.localPosition = ClosedPoint;
        GameOverPanel.gameObject.SetActive(true);

        GameOverPanel.transform.DOLocalMove(OpenedPoint, TransitionTime).SetEase(Ease.OutCubic);
    }
}
