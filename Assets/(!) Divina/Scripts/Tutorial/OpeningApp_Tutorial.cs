using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningApp_Tutorial : MonoBehaviour
{
    [SerializeField] private Tutorial_Event tutorial;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject Box;
    [SerializeField] private Button reply;


    //[SerializeField] private int progress;

    // Update is called once per frame
    //public void OpeningApp()
    //{
    //    if (tutorial.isDefault && tutorial.openapp)
    //    {
    //        tutorial.progression++;
    //        tutorial.openapp = false;

    //        Debug.Log("Opening " + this.gameObject.name + " done. Objective Number");
    //    }
        
    //    else
    //    {
    //        Debug.Log("Okay, just opening the app! Nothing special yet!");
    //    }
    //}

    public void Messaging(TextMeshProUGUI text)
    {
        Box.SetActive(true);
        message.text = text.text;
        //te.clickProgression();
    }
}
