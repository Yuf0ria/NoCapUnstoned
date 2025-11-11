using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Phone_Login : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TMP_InputField otpInput;


    [Header("Auto Fill Settings")]
    [SerializeField] private string Username = "kevin.student";
    [SerializeField] private string Password = "password123";
    [SerializeField] private string OTP = "";
    [SerializeField] private float autoTypeDelay = 0.2f; // delay between characters

    private bool usernameFilled = false;
    private bool passwordFilled = false;
    private bool otpFilled = false;

    [SerializeField] private Event_Manager event_Manager;

    void Start() //Change this to OnEnable
    {
        loginButton.interactable = false;

        usernameInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(usernameInput, Username, 1)));
        passwordInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(passwordInput, Password, 2)));
    }

    private IEnumerator AutoFill(TMP_InputField inputField, string mockText, int filledUp)
    {
        Debug.Log("Running " + inputField.gameObject.name);

        // Hide placeholder
        var placeholder = inputField.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (placeholder != null) placeholder.gameObject.SetActive(false);

        inputField.text = "";

        // Simulate typing effect
        foreach (char c in mockText)
        {
            inputField.text += c;
            yield return new WaitForSeconds(autoTypeDelay);
        }

        if (filledUp == 1) usernameFilled = true;
        if (filledUp == 2) passwordFilled = true;
        if (filledUp == 3) otpFilled = true;

        CheckLoginReady();
    }
    
    void ResetDetails(TMP_InputField inputField)
    {
        var placeholder = inputField.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        placeholder.gameObject.SetActive(true);

        inputField.text = "";
    }

    private void CheckLoginReady()
    {
        if (usernameFilled && passwordFilled)
        {
            loginButton.interactable = true;
        }
    }

    public void Login()
    {
        if (Phone_Statistics.isTwoFactorAuthentication)
        {
            if (OTP == otpInput.text && otpInput.text != "")
            {
                // MOVE TO NEXT PAGE
                Debug.Log("Moving to Next Page");
                loginButton.interactable = true;
                event_Manager.New_Notification(0, "App_Name", "OTP confirmed. Logging in...");
            }

            else
            {
                loginButton.interactable = false;
                otpInput.gameObject.SetActive(true);
                TwoFactorAuthentication();
            }
        }
    }
    
    void TwoFactorAuthentication()
    {
        OTP = Random.Range(1000, 9999).ToString();
        
        event_Manager.New_Notification(0, "App_Name", "Logging in? Here is your OTP: " + OTP);
        
        otpInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(otpInput, OTP, 3)));
    }
}
