using TMPro;
using UnityEngine;

public class SecurityStatus : MonoBehaviour
{
    [Header("Security Status")]
    [SerializeField] private TextMeshProUGUI toggleStatusText;
    [SerializeField] private GameObject securityContent;
    [SerializeField] public bool hasSecurity;

    private void Start()
    {
        Change();
    }

    public void toggleStatus()
    {
        hasSecurity = !hasSecurity;
        Change();
    }

    private void Change()
    {
        if (hasSecurity)
        {
            toggleStatusText.text = "On";
            securityContent.SetActive(true);
        }
        if (!hasSecurity)
        {
            toggleStatusText.text = "Off";
            securityContent.SetActive(false);
        }
    }
}
