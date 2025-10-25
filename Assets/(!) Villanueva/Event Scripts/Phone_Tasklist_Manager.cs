using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class Phone_Tasklist_Manager : MonoBehaviour
{
    //PUT INTO GAMEOBJECT TASKLIST MANAGER
    [SerializeField] TextMeshProUGUI Task1;
    [SerializeField] TextMeshProUGUI Task2;
    [SerializeField] TextMeshProUGUI Task3;

    public static int currLevel;
    //This value is modified specifically by the Level/Scene Manager

    int currNumOfTasks = 0; // Number of tasks for the current tasklist
    public static int tasklist; // What tasklist we're on.

    public static Dictionary<int, int> taskID_taskDisplay = new Dictionary<int, int>();
     
    //This is to change which tasklist is currently active
    void Start()
    {
        currLevel = 1;
        tasklist = 1;

        Level();
    }

    public void Level()
    {
        Debug.Log("Running Level()");
        if (currLevel == 1) Level1();
    }

    void Level1()
    {
        switch (tasklist)
        {
            case 1:

                Task1.text = "Please open the most recent mail in Postmail";
                Task2.text = "Open Gallery";
                Task3.text = "Open Settings";

                taskID_taskDisplay.Add(1, 1);
                taskID_taskDisplay.Add(2, 2);
                taskID_taskDisplay.Add(3, 3);

                currNumOfTasks = 3;
                break;

            case 2:

                Task1.text = "Place an Order in Order Corner";
                Task2.text = "View your Eduva Quiz";
                Task3.text = "";

                taskID_taskDisplay.Add(4, 1);
                taskID_taskDisplay.Add(5, 2);

                currNumOfTasks = 2;
                break;

            default:
                Debug.Log("All tasklists completed.");
                break;
        }

        for (int i = 0; i <= currNumOfTasks - 1; i++)
        {
            Phone_Tasklist_General.taskIsComplete.Add(false);
        }

        //Debug.Log(tasklist);
    }

}
