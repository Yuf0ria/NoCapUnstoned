using UnityEngine;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour
{
    [Header("Home Button")]
    [SerializeField] private Button Home;
    [SerializeField] private GameObject[] AllApps;

    [Header("Gallery")]
    [SerializeField] private Button Gallery;
    [SerializeField] private GameObject onGallery;

    [Header("Notes")]
    [SerializeField] private Button Notes;
    [SerializeField] private GameObject onNotes;

    [Header("Messages")]
    [SerializeField] private Button Messages;
    [SerializeField] private GameObject onMessages;

    [Header("Settings")]
    [SerializeField] private Button Settings;
    [SerializeField] private GameObject onSettings;

    [Header("FriendLink")]
    [SerializeField] private Button FriendLink;
    [SerializeField] private GameObject onFriendLink;

    [Header("Webb")]
    [SerializeField] private Button Webb;
    [SerializeField] private GameObject onWebb;

    [Header("Eduva")]
    [SerializeField] private Button Eduva;
    [SerializeField] private GameObject onEduva;

    [Header("OrderCorner")]
    [SerializeField] private Button OrderCorner;
    [SerializeField] private GameObject onOrderCorner;

    [Header("Postmail")]
    [SerializeField] private Button Postmail;
    [SerializeField] private GameObject onPostmail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject app in AllApps)
        {
            app.gameObject.SetActive(false);
        }

        Home.onClick.AddListener(() =>
        {
            foreach (GameObject app in AllApps)
            {
                app.gameObject.SetActive(false);
            }
        });

        Gallery.onClick.AddListener(() =>
        {
            onGallery.SetActive(true);
        });

        Notes.onClick.AddListener(() =>
        {
            onNotes.SetActive(true);
        });

        Messages.onClick.AddListener(() =>
        {
            onMessages.SetActive(true);
        });

        Settings.onClick.AddListener(() =>
        {
            onSettings.SetActive(true);
        });

        FriendLink.onClick.AddListener(() =>
        {
            onFriendLink.SetActive(true);
        });

        Webb.onClick.AddListener(() =>
        {
            onWebb.SetActive(true);
        });

        Eduva.onClick.AddListener(() =>
        {
            onEduva.SetActive(true);
        });

        OrderCorner.onClick.AddListener(() =>
        {
            onOrderCorner.SetActive(true);
        });

        Postmail.onClick.AddListener(() =>
        {
            onPostmail.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
