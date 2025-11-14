using System.ComponentModel;
using UnityEngine;

public class Phone_Statistics : MonoBehaviour
{
    public static bool isWifiConnected;
    public static bool isSecurityUpToDate;
    public static bool isTwoFactorAuthentication;
    public static bool isAntiVirus;
    public static bool isAdBlocker;

    public static bool disablePhoneBackButton;

    public static bool isVulnerable;
    public static bool isCompromised;
    public static bool isStolenAccount;
    public static bool isGameOverRunning = false;

    public static int numLowSeverity;
    public static int numHighSeverity;


    [Header("Starting Toggle Stats")]
    [SerializeField] bool isWifiConnectedInsp;
    [SerializeField] bool isSecurityUpToDateInsp;
    [SerializeField] bool isTwoFactorAuthenticationInsp;
    [SerializeField] bool isAntiVirusInsp;
    [SerializeField] bool isAdBlockerInsp;


    [Header("Starting Stats")]
    [SerializeField] bool isVulnerableInsp;
    [SerializeField] bool isCompromisedInsp;
    [SerializeField] bool isStolenAccountInsp;

    [SerializeField] int numLowSeverityInsp;
    [SerializeField] int numHighSeverityInsp;


    void Start()
    {
        isWifiConnected = isWifiConnectedInsp;
        isSecurityUpToDate = isSecurityUpToDateInsp;
        isTwoFactorAuthentication = isTwoFactorAuthenticationInsp;
        isAntiVirus = isAntiVirusInsp;
        isAdBlocker = isAdBlockerInsp;
        isStolenAccount = isStolenAccountInsp;

        //disablePhoneBackButton = disablePhoneBackButtonInsp;

        isVulnerable = isVulnerableInsp;
        isCompromised = isCompromisedInsp;

        numLowSeverity = numLowSeverityInsp;
        numHighSeverity = numHighSeverityInsp;
    }

    void Update()
    {
        //if (isWifiConnected != isWifiConnectedInsp) isWifiConnectedInsp = isWifiConnected;
    }


}
