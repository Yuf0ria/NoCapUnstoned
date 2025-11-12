using UnityEngine;

public class Phone_Statistics : MonoBehaviour
{
    public static bool isWifiConnected;
    public static bool isSecurityUpToDate;


    public static bool disablePhoneBackButton;

    public static bool isVulnerable;
    public static bool isCompromised;

    public static int numLowSeverity;
    public static int numHighSeverity;


    [SerializeField] bool isWifiConnectedInsp;
    [SerializeField] bool isSecurityUpToDateInsp;


    [SerializeField] bool disablePhoneBackButtonInsp;

    [SerializeField] bool isVulnerableInsp;
    [SerializeField] bool isCompromisedInsp;

    [SerializeField] int numLowSeverityInsp;
    [SerializeField] int numHighSeverityInsp;

    void Start()
    {
        isWifiConnected = isWifiConnectedInsp;
        isSecurityUpToDate = isSecurityUpToDateInsp;

        disablePhoneBackButton = disablePhoneBackButtonInsp;

        isVulnerable = isVulnerableInsp;
        isCompromised = isCompromisedInsp;

        numLowSeverity = numLowSeverityInsp;
        numHighSeverity = numHighSeverityInsp;
    }

    void Update()
    {
        if (isWifiConnected != isWifiConnectedInsp) isWifiConnectedInsp = isWifiConnected;
    }
}
