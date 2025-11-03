using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [Header("Timer")]
    //[SerializeField] private TextMeshProUGUI batteryRecord;
    [SerializeField] private float sec;
    [SerializeField] private int battery;
    [SerializeField] private Sprite[] batteryLogo; // Element 0 starts at 100%

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        if (battery != 1)
        {
            if (sec >= 180) 
            { sec = 0; battery--; }
        }        
        
        Image image = transform.GetComponentInChildren<Image>();

        TextMeshProUGUI batteryRecord = GetComponent<TextMeshProUGUI>();
        batteryRecord.text = battery.ToString() + "%";

        if (battery > 90) //Full battery
        { image.sprite = batteryLogo[0]; }

        if (battery > 70) //Okay battery
        { image.sprite = batteryLogo[1]; }

        else if (battery > 50) //Half battery
        { image.sprite = batteryLogo[2]; }

        else if (battery > 30) //Low battery
        { image.sprite = batteryLogo[3]; }

        else if (battery > 10) //Lower battery
        { image.sprite = batteryLogo[4]; }
    }
}
