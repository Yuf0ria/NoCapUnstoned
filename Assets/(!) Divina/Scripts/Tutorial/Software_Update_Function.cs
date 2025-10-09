using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Software_Update_Function : MonoBehaviour
{
    [Header("Event Script")]
    [SerializeField] private Tutorial_Event te;
    [SerializeField] private Tutorial_Messages me;

    [Header("UI")]
    //Software Status
    [SerializeField] private TextMeshProUGUI softwareUpdateStatus;
    [SerializeField] private TextMeshProUGUI softwareUpdateDetails;
    [SerializeField] private string softwareUpdateDetails_text;

    [Header("Slider")]
    [SerializeField] private Slider softwareUpdateProgressSlider;

    [Header("Button")]
    //Button
    [SerializeField] private Button installUpdate;
    [SerializeField] private TextMeshProUGUI softwareUpdateProgress;

    [Header("Updating")]
    [SerializeField] public bool hasUpdate;
    [SerializeField] private bool isUpdating;
    [SerializeField] private bool isDone;
    private float inputProgress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        installUpdate.onClick.AddListener(updateSoftware);

        softwareUpdateProgressSlider.maxValue = 10;

        hasUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasUpdate)
        {
            softwareUpdateStatus.text = "Software Update Available";
            softwareUpdateDetails.text = softwareUpdateDetails_text;

            //Install Button
            installUpdate.interactable = true;
            softwareUpdateProgress.text = "Install Update";
        }

        if (!hasUpdate)
        {
            softwareUpdateStatus.text = "No Software Updates Available";
            softwareUpdateDetails.text = "";

            //Install Button
            installUpdate.interactable = false;
            softwareUpdateProgress.text = "Update Installed";
        }

        if (isUpdating)
        {
            Updating();
            softwareUpdateProgress.text = "Installing...";
            softwareUpdateProgressSlider.value = inputProgress;
        }

        if (inputProgress >= softwareUpdateProgressSlider.maxValue && isUpdating) //When the update has been installed
        {            
            installUpdate.interactable = false;

            //Text
            softwareUpdateProgress.text = "Update Installed";
            me.hideNotifPhishingLink();
            //te.progress
            te.progression = 22;
            te.clickProgression();

            //isDone = true;
            isUpdating = false;
        }

        //if (isDone)
        //{
            
        //    isDone = false;
        //}
    }

    private void Updating()
    {
        if (inputProgress >= 0)
        {
            inputProgress -= 0.5f * Time.deltaTime;
        }        
    }

    public void updateSoftware()
    {
        if (!isUpdating)
        {
            isUpdating = true;            
        } 
        else
        {
            inputProgress += 0.5f;
        }
    }
}
