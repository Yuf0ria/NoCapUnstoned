using UnityEngine;

public class Phone_Tasklist_Manager : MonoBehaviour
{
    //PUT INTO GAMEOBJECT TASKLIST MANAGER

    public static int currLevel;
    //This value is modified specifically by the Level/Scene Manager

    int currNumOfTasks = 0; // Number of tasks for the current tasklist
    public static int tasklist; // What tasklist we're on.


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
                Debug.Log("Displaying Tasklist 1 of Level 1");
                Debug.Log("(1) Please open the most recent email in Postmail");
                Debug.Log("(2) Open Gallery");
                Debug.Log("(3) Open Settings");

                currNumOfTasks = 3;
                break;

            case 2:
                // << Insert Event or Cutscene Void/Method befor the Tasks are "Displayed"

                Debug.Log("Displaying Tasklist 2 of Level 1");
                Debug.Log("(4) Look at the Shrek Picture");
                currNumOfTasks = 1;
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
