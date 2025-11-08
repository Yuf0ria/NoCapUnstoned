using System;
using TMPro;
using UnityEngine;

public class TimeScript : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timeRecord;
    private float sec;
    [SerializeField] private float min;
    [SerializeField] private int hour;

    public string hourText;
    public string minText;

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 30) { sec = 0; min++; }

        if (min <= 9)
        {
            minText = "0" + Convert.ToInt32(min);
        }
        else
        {
            minText = "" + Convert.ToInt32(min);
        }

        if (min >= 60) { min = 0; hour++; }

        if (hour > 24) { hour = 1; }

        hourText = "" + Convert.ToInt32(hour);

        timeRecord.text = hourText + ":" + minText;
    }

    public string GetCurrentTimeString()
    {
        return hourText + ":" + minText;
    }
}
