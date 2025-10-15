using System.Collections.Generic;
using UnityEngine;


public class Phone_Tasklist_General : MonoBehaviour
{
    [SerializeField] Event_Manager events;

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

            this.GetComponent<Phone_Tasklist_Manager>().Level(); // << Error Occuring Here
        }

        //Debug.Log("Num of Tasks Complete: " + numTasksComplete);
        //Debug.Log("Num of Tasks: " + Phone_Tasklist_Manager.currNumOfTasks);
    }

    public void CompleteTask(int taskNum)
    {
        if (taskNum <= taskIsComplete.Count)
        {
            Debug.Log("Task #" + taskNum + " is Complete!");
            taskIsComplete[taskNum - 1] = true;

            events.Run_Event();
        }

        else return; //Debug.Log("This task is not unlocked yet.");
        

        CheckCompletion();
    }
}
