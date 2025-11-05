using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;
using System.Xml.Serialization;
using UnityEditor.ShaderGraph.Internal;


public class App_Settings : MonoBehaviour
{

    float autoScanTime = 60;
    float manualScanTime = 5;

    [SerializeField] Event_Manager events;

    //[SerializeField] private Transform ClosedPosition; //= new Vector3(1250, -175, 0);
    //[SerializeField] private Transform OpenedPosition; //= new Vector3(0, -175, 0);
    //float TransitionTime = 0.5f;

    /*
    public void OpenSettingsCategory(GameObject SettingsCategoryPage)
    {
        Debug.Log("Opening Settings Category: " + SettingsCategoryPage.name);

        SettingsCategoryPage.transform.position = ClosedPosition.position;
        SettingsCategoryPage.gameObject.SetActive(true);

        //this.gameObject.SetActive(false);

        SettingsCategoryPage.transform.DOMove(OpenedPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    public void ReturnToSettingsMain(GameObject SettingsMainPage)
    {
        Debug.Log("Returning to Main Settings Page...");

        SettingsMainPage.gameObject.SetActive(true);

        transform.DOMove(ClosedPosition.position, TransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });
    }
    */

    void Start()
    {
        if (events != null) StartCoroutine(AutoScan());

        currentWiFi = CurrentWiFi;
        wiFiStatus = WiFiStatus;
        Phone_Statistics.isWifiConnected = true;
    }

    #region

    [SerializeField] TextMeshProUGUI CurrentWiFi;
    [SerializeField] TextMeshProUGUI WiFiStatus;
    public static TextMeshProUGUI currentWiFi;
    public static TextMeshProUGUI wiFiStatus;
    public void ConnectToWifi(TextMeshProUGUI NewWiFi)
    {
        currentWiFi.text = NewWiFi.GetComponentInChildren<TextMeshProUGUI>().text;
        wiFiStatus.text = "Connected";
        Phone_Statistics.isWifiConnected = true;
    }
    
    public static void DisconnectToWifi()
    {
        currentWiFi.text = " ";
        wiFiStatus.text = "Disconnected";
        Phone_Statistics.isWifiConnected = false;
    }
    
    #endregion

    #region Security Scans
    IEnumerator AutoScan()
    {
        Debug.Log("AUTO SCANNING");

        yield return new WaitForSeconds(autoScanTime);

        if (Phone_Statistics.isWifiConnected && Phone_Statistics.isSecurityUpToDate)
        {
            if (Phone_Statistics.isCompromised)
                events.New_Notification(0, "Settings", "Your phone may be compromised.");
        }

        StartCoroutine(AutoScan());
    }

    public void ManualScan()
    {
        if (Phone_Statistics.numLowSeverity > 0 || Phone_Statistics.numHighSeverity > 0)
            Phone_Statistics.isCompromised = true;

        StartCoroutine(ManualScanning());
    }

    IEnumerator ManualScanning()
    {
        Debug.Log("MANUAL SCANNING");

        events.New_Notification(0, "Settings", "Starting security scan...");

        yield return new WaitForSeconds(manualScanTime);

        if (Phone_Statistics.numLowSeverity > 0 || Phone_Statistics.numHighSeverity > 0)
        {
            float totalAttacks = Phone_Statistics.numLowSeverity + Phone_Statistics.numHighSeverity;
            events.New_Notification(0, "Settings", "There have been " + totalAttacks + " successful phishing attacks.");
        }


        else if (Phone_Statistics.isVulnerable && !Phone_Statistics.isCompromised)
            events.New_Notification(0, "Settings", "Your phone is vulnerable. Stay cautious.");


        else
            events.New_Notification(0, "Settings", "Nothing came up on scans.");

    }

    #endregion
}
