using UnityEngine;

public class Phone_Statistics : MonoBehaviour
{
    public static bool isWifiConnected;
    public static bool isSecurityUpToDate;
    public static bool isTwoFactorAuthentication;

    public static float transitionTimeMultiplier;


    public static bool disablePhoneBackButton;

    public static bool isVulnerable;
    public static bool isCompromised;

    public static int numLowSeverity;
    public static int numHighSeverity;


    [Header("Starting Toggle Stats")]
    [SerializeField] bool isWifiConnectedInsp;
    [SerializeField] bool isSecurityUpToDateInsp;
    [SerializeField] bool isTwoFactorAuthenticationInsp;


    [Header("Starting Float Stats")]
    [SerializeField] float transitionTimeMultiplierInsp = 1;



    //[SerializeField] bool disablePhoneBackButtonInsp;


    [Header("Starting Stats")]
    [SerializeField] bool isVulnerableInsp;
    [SerializeField] bool isCompromisedInsp;

    [SerializeField] int numLowSeverityInsp;
    [SerializeField] int numHighSeverityInsp;

    void Start()
    {
        isWifiConnected = isWifiConnectedInsp;
        isSecurityUpToDate = isSecurityUpToDateInsp;
        isTwoFactorAuthentication = isTwoFactorAuthenticationInsp;

        transitionTimeMultiplier = transitionTimeMultiplierInsp;

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
