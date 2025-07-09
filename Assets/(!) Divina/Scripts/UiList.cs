using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiList : MonoBehaviour
{
    [Header("Bottom Buttons")]
    [SerializeField] private Button Settings;
    [SerializeField] private Button Home;
    [SerializeField] private Button Back;

    [Header("Homepage: Buttons")]
    [SerializeField] public Button webbButton;
    [SerializeField] public Button postmailButton;
    [SerializeField] public Button friendlinkButton;
    [SerializeField] public Button eduvaButton;
    [SerializeField] public Button ordercornerButton;
    [SerializeField] public Button messagesButton;
    [SerializeField] public Button notesButton;
    [SerializeField] public Button galleryButton;
    [SerializeField] public Button settingsButton;

    [Header("APP: All Apps")]
    [SerializeField] public GameObject[] allApps;

    /// <summary>
    /// WEBB
    /// </summary>

    [Header("APP: Webb")]
    [SerializeField] public GameObject webbApp;
    //Pages
    [SerializeField] public GameObject webbHomePage;
    [SerializeField] public GameObject webbSearchResultsPage;
    [SerializeField] public GameObject webbWebsite1Page;
    [SerializeField] public GameObject webbSettingsPage;

    [Header("Webb: Tab Buttons")]
    [SerializeField] public Button homePageButton;
    [SerializeField] public Button searchTabButton;
    [SerializeField] public Button settingsPageButton;

    [Header("Webb: Websites")]
    [SerializeField] public GameObject websitePage;
    [SerializeField] public GameObject fakeWebsitePage;

    [Header("Webb: Home Page Buttons")]
    [SerializeField] public Button site1Shortcut;
    [SerializeField] public Button site2Shortcut;
    [SerializeField] public Button site3Shortcut;

    [Header("Webb: Search Results Buttons")]
    [SerializeField] public Button site1Redirect;
    [SerializeField] public Button site2Redirect;
    [SerializeField] public Button site3Redirect;
    [SerializeField] public Button site4Redirect;

    /// <summary>
    /// POSTMAIL
    /// </summary>

    [Header("APP: Postmail")]
    [SerializeField] public GameObject postmailApp;
    //Pages
    [SerializeField] public GameObject postmailHomePage;
    [SerializeField] public GameObject postmailEmailPage;

    [Header("Postmail: Buttons")]
    [SerializeField] public Button backToHomePage;

    [Header("Postmail: Inbox")]
    [SerializeField] public Button email1;
    [SerializeField] public Button email2;
    [SerializeField] public Button email3;
    [SerializeField] public Button email4;

    [Header("Postmail: Email")]
    [SerializeField] public Button email1Page;
    [SerializeField] public Button email2Page;
    [SerializeField] public Button email3Page;
    [SerializeField] public Button email4Page;

    [Header("APP: FriendLink")]
    [SerializeField] public GameObject friendlinkApp;
    //Pages
    [SerializeField] public GameObject friendlinkHomePage;
    [SerializeField] public GameObject friendlinkContactListPage;
    [SerializeField] public GameObject friendlinkMessagesPage;
    [SerializeField] public GameObject friendlinkSettingsPage;


    //[Header("APP: Eduva (WIP)")]
    //[SerializeField] public GameObject eduvaApp;
    //Pages



    //[Header("APP: Order Corner (WIP)")]
    //[SerializeField] public GameObject ordercornerApp;
    //Pages



    [Header("APP: Messages")]
    [SerializeField] public GameObject messagesApp;
    //Pages
    [SerializeField] public GameObject messagesContactListPage;
    [SerializeField] public GameObject messagesMessagePage;


    [Header("APP: Notes")]
    [SerializeField] public GameObject notesApp;
    //Pages... Wait...



    [Header("APP: Gallery")]
    [SerializeField] public GameObject galleryApp;
    //Pages
    [SerializeField] public GameObject galleryHomePage;
    [SerializeField] public GameObject galleryImagePage;


    [Header("APP: Settings")]
    [SerializeField] public GameObject settingsApp;
    //Pages
    [SerializeField] public GameObject settingsHomePage;
    [SerializeField] public GameObject settingsWifiPage;
    [SerializeField] public GameObject settingsSecurityPage;
    [SerializeField] public GameObject settingsSoftwareUpdatePage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* //To start everything
        foreach (GameObject app in allApps)
        {
            app.gameObject.SetActive(false);
        }

        Home.onClick.AddListener(() =>
        {
            foreach (GameObject app in allApps)
            {
                app.gameObject.SetActive(false);
            }
        });
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
