using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class Phone_Login : MonoBehaviour
{
    [Header("Fake Login Page")]
    [SerializeField] private bool isPhishingPage = false;
    [SerializeField] private float phishingLevel = 1;

    [Header("Prefab, GameObjects and Position")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField otpInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private RectTransform Page_Login;


    [SerializeField] private TMP_InputField oldPasswordInput;
    [SerializeField] private TMP_InputField newPasswordInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private RectTransform Page_Change;


    [SerializeField] private Transform openedPos;
    [SerializeField] private Transform closedPos;
    [SerializeField] private float transitionTime;


    [Header("Auto Fill Settings")]
    [SerializeField] private string Username = "kevin.student";
    [SerializeField] private string Password = "password123";
    [SerializeField] private string OTP = "";
    [SerializeField] private float autoTypeDelay = 0.2f; // delay between characters

    private bool usernameFilled = false;
    private bool passwordFilled = false;
    private bool otpFilled = false;
    private bool oldPasswordFilled = false;

    [SerializeField] private Event_Manager events;

    void Start() //Change this to OnEnable
    {
        usernameInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(usernameInput, Username, 1)));
        passwordInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(passwordInput, Password, 2)));
        otpInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(otpInput, OTP, 3)));

        oldPasswordInput.onSelect.AddListener(_ => StartCoroutine(AutoFill(oldPasswordInput, Password, 4)));
        newPasswordInput.onValueChanged.AddListener(_ => New_PasswordInput());
        //Login_Start();
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
        if (filledUp == 4) oldPasswordFilled = true;

        CheckLoginReady();
    }

    private void CheckLoginReady()
    {
        if (usernameFilled && passwordFilled)
        {
            loginButton.interactable = true;
        }
    }

    public void Login_Confirm()
    {
        if (Phone_Statistics.isTwoFactorAuthentication)
        {
            if (OTP == otpInput.text && otpInput.text != "")
            {
                if(!isPhishingPage)
                {
                    events.New_Notification(0, "App_Name", "OTP confirmed. Logging in...");
                }

                else
                {
                    if(phishingLevel > 1) 
                    {
                        Phone_Statistics.isStolenAccount = true; 
                        Debug.Log("Account Stolen!");
                        Phone_Statistics.numLowSeverity++;
                    }
                }

                loginButton.interactable = true;
                Login_Leave();
            }

            else
            {
                loginButton.interactable = false;
                otpInput.gameObject.SetActive(true);
                TwoFactorAuthentication();
            }
        }

        else
        {
            if(!isPhishingPage)
            {
                events.New_Notification(0, "App_Name", "Logging in...");
            }
        
            else
            {
                Phone_Statistics.isStolenAccount = true; 
                Debug.Log("Account Stolen!");
                Phone_Statistics.numLowSeverity++;
                Debug.Log(Phone_Statistics.numLowSeverity);
            }

            Login_Leave();
        }
    }

    void TwoFactorAuthentication()
    {
        OTP = Random.Range(100000, 999999).ToString();

        events.New_Notification(0, "App_Name", "Logging in? Here is your OTP: " + OTP);
    }

    public void Login_Start()
    {
        otpInput.gameObject.SetActive(false);
        Page_Login.transform.DOMove(openedPos.position, transitionTime).SetEase(Ease.OutCubic);
    }

    public void Login_Leave()
    {
        Page_Login.transform.DOMove(closedPos.position, transitionTime).SetEase(Ease.OutCubic);

        if (usernameInput != null)
        {
            usernameInput.text = "";
            var placeholder = usernameInput.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (placeholder != null) placeholder.gameObject.SetActive(true);
        }

        if (passwordInput != null)
        {
            passwordInput.text = "";
            var placeholder = passwordInput.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (placeholder != null) placeholder.gameObject.SetActive(true);
        }

        if (otpInput != null)
        {
            otpInput.text = "";
            var placeholder = otpInput.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (placeholder != null) placeholder.gameObject.SetActive(true);

            otpInput.gameObject.SetActive(false);
        }

        usernameFilled = false;
        passwordFilled = false;
        otpFilled = false;

        //StopAllCoroutines();

        if (loginButton != null) loginButton.interactable = false;
    }





    public void ChangePassword()
    {
        Login_Leave();

        Page_Change.transform.DOMove(openedPos.position, transitionTime).SetEase(Ease.OutCubic);
    }

    void New_PasswordInput()
    {
        var placeholder = newPasswordInput.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (newPasswordInput.text != "")
        {
            if (placeholder != null && placeholder.gameObject.name == "Placeholder") placeholder.gameObject.SetActive(false);
            
            confirmButton.interactable = true;
        }

        else
        {
            if (placeholder != null && placeholder.gameObject.name == "Placeholder") placeholder.gameObject.SetActive(true);           
        }
    }
    
    public void Confirm_NewPassword()
    {
        Password = newPasswordInput.text;

        if(Phone_Statistics.numLowSeverity > 0 && Phone_Statistics.isStolenAccount) // Check if the Account was actually Stolen
        {
            Phone_Statistics.numLowSeverity--;
            Phone_Statistics.isStolenAccount = false;
        }
        
        ChangePasword_Leave();
    }
    
    public void ChangePasword_Leave()
    {
        newPasswordInput.text = "";

        if (oldPasswordInput != null)
        {
            oldPasswordInput.text = "";
            var placeholder = oldPasswordInput.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (placeholder != null) placeholder.gameObject.SetActive(true);
        }

        if (newPasswordInput != null)
        {
            newPasswordInput.text = "";
            var placeholder = newPasswordInput.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (placeholder != null) placeholder.gameObject.SetActive(true);
        }

        oldPasswordFilled = false;

        //StopAllCoroutines();

        confirmButton.interactable = false;

        Page_Change.transform.DOMove(closedPos.position, transitionTime).SetEase(Ease.OutCubic);
        Login_Start();
    }


}
