using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Phone_Tasklist_General : MonoBehaviour
{
    [SerializeField] Event_Manager events;

    [SerializeField] TextMeshProUGUI Task1;
    [SerializeField] TextMeshProUGUI Task2;
    [SerializeField] TextMeshProUGUI Task3;

    public static List<bool> taskIsComplete = new List<bool>();
    void CheckCompletion()
    {
        int numTasksComplete = 0;

        for (int i = 0; i <= taskIsComplete.Count - 1; i++)
        {
            if (taskIsComplete[i]) numTasksComplete++;
        }

        if (numTasksComplete == taskIsComplete.Count)
        {
            //Debug.Log("ALL TASKS COMPLETE!!! Moving to next tasklist...");
            Phone_Tasklist_Manager.tasklist++;

            Task1.fontStyle = FontStyles.Normal;
            Task2.fontStyle = FontStyles.Normal;
            Task3.fontStyle = FontStyles.Normal;


            this.GetComponent<Phone_Tasklist_Manager>().Level(); // << Error Occuring Here
        }

        //Debug.Log("Num of Tasks Complete: " + numTasksComplete);
        //Debug.Log("Num of Tasks: " + Phone_Tasklist_Manager.currNumOfTasks);
    }

    public void CompleteTask(int taskNum)
    {
        if (taskNum <= taskIsComplete.Count)
        {
            //Debug.Log("Task #" + taskNum + " is Complete!");
            taskIsComplete[taskNum - 1] = true;

            int dispNum = Phone_Tasklist_Manager.taskID_taskDisplay[taskNum];

            switch(dispNum)
            {
                case 1:
                    Task1.fontStyle = FontStyles.Strikethrough;
                    break;

                case 2:
                    Task2.fontStyle = FontStyles.Strikethrough;
                    break;

                default: 
                    Task3.fontStyle = FontStyles.Strikethrough;
                    break;
            }

            events.Run_RandomEvent();
        }

        else return; //Debug.Log("This task is not unlocked yet.");
        

        CheckCompletion();
    }
}
