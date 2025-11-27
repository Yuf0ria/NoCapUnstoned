using TMPro;
using UnityEngine;

public class Chapter1Events : MonoBehaviour
{
    [Header("Tasks")]
    [SerializeField] private bool first_OrderCorner;
    [SerializeField] private bool second_Quiz;
    [SerializeField] private bool third_Faculty;
    [SerializeField] private bool fourth_Seminar;
    [SerializeField] private bool fifth_Research;
    [SerializeField] private bool sixth_Internship;
    [SerializeField] private bool seventh_Counseling;

    [Header("GameObjects")]
    [SerializeField] private TextMeshProUGUI goFirst_OrderCorner;
    [SerializeField] private TextMeshProUGUI goSecond_Quiz;
    [SerializeField] private TextMeshProUGUI goThird_Faculty;
    [SerializeField] private TextMeshProUGUI goFourth_Seminar;
    [SerializeField] private TextMeshProUGUI goFifth_Research;
    [SerializeField] private TextMeshProUGUI goSixth_Internship;
    [SerializeField] private TextMeshProUGUI goSeventh_Counseling;

    [Header("Event Manager")]
    [SerializeField] private Event_Manager eventManager;

    private bool wasCompleted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checking();
    }

    // Update is called once per frame
    void Update()
    {
        checking();

        if (AllTasksCompleted() && !wasCompleted)
        {
            if (eventManager != null)
            {
                eventManager.Run_GameWin();
                wasCompleted = true;
            }
        }
    }

    void checking()
    {
        // Check and update the task flags if text has strikethrough
        first_OrderCorner = HasStrikethrough(goFirst_OrderCorner);
        second_Quiz = HasStrikethrough(goSecond_Quiz);
        third_Faculty = HasStrikethrough(goThird_Faculty);
        fourth_Seminar = HasStrikethrough(goFourth_Seminar);
        fifth_Research = HasStrikethrough(goFifth_Research);
        sixth_Internship = HasStrikethrough(goSixth_Internship);
        seventh_Counseling = HasStrikethrough(goSeventh_Counseling); 
    }

    bool HasStrikethrough(TextMeshProUGUI tmp)
    {
        return (tmp.fontStyle & FontStyles.Strikethrough) == FontStyles.Strikethrough;
    }

    public bool AllTasksCompleted()
    {
        return first_OrderCorner && second_Quiz && third_Faculty && fourth_Seminar && fifth_Research && sixth_Internship && seventh_Counseling;
    }
}
