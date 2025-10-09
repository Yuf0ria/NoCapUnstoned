using TMPro;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI batteryRecord;
    [SerializeField] private float sec;
    [SerializeField] private int battery;

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;

        if (sec >= 180) { sec = 0; battery--; }

        batteryRecord.text = battery.ToString() + "%";
    }
}
