using TMPro;
using UnityEngine;

public class WiFiStatus : MonoBehaviour
{
    [Header("WiFi Status")]
    [SerializeField] private TextMeshProUGUI toggleStatusText;
    [SerializeField] private GameObject wifiContent;
    [SerializeField] private GameObject wifiStatusSymbol;
    [SerializeField] public bool hasWifi;

    private void Start()
    {
        Change();
    }

    public void toggleStatus()
    { 
        hasWifi = !hasWifi;
        Change();        
    }

    private void Change()
    {
        if (hasWifi)
        {
            toggleStatusText.text = "On";
            wifiContent.SetActive(true);
            wifiStatusSymbol.SetActive(true);
        }
        if (!hasWifi)
        {
            toggleStatusText.text = "Off";
            wifiContent.SetActive(false);
            wifiStatusSymbol.SetActive(false);
        }
    }
}
