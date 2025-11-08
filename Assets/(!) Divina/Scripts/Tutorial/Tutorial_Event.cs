using UnityEngine; 
using System.Collections.Generic; 
using TMPro; 
using UnityEngine.UI; 
using System;
using System.Collections;
//Weird way to format it like this, I know. - Nicaia

/* 1.11.2025 update:
 * HEY READ THE READFIRST.CS SCRIPT INSIDE THE SAME
 * FOLDER AS THIS ONE BEFORE YOU DO ANYTHING THANKS <3
 * 
 * DO NOT DO ANYTHING DRASTIC HERE
 */

//How the hell do you do this thing???
[Serializable]
public struct tutorialProgression
{
    [Header("GameObjects")]
    [SerializeField] private GameObject[] toSetTrue;
    [SerializeField] private GameObject[] toSetFalse;
    [SerializeField] public Button[] activeProgression; //For other voids to access    

    [Header("Story/Dialogue")]
    [SerializeField] public string dialogue;

    public void change(tutorialProgression tp, TextMeshProUGUI text)
    {
        foreach (var setTrue in toSetTrue)
        { setTrue.SetActive(true); }
        foreach (var setFalse in toSetFalse)
        { setFalse.SetActive(false); }
        if (text != null)
        { text.text = string.Empty; }
    }
} //IVE BEEN LOOKING FOR TS FOR SO LONG BRUUHHHHH.

public class Tutorial_Event : MonoBehaviour
{
    [Header("Objects n Stuff!")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Progression")]
    [SerializeField] public int progression; //Current Progression
    [SerializeField] private List<tutorialProgression> tp; //The list :3

    private bool isTyping = false;

    private void Start()
    {
        if (progression >= 0 && progression < tp.Count)
        {
            tp[progression].change(tp[progression], text);
        }
        setButtons(progression);

        isTyping = true;
        StartCoroutine(TypeLine());
    }

    private void OnDestroy()
    {
        unsetButtons(progression);
    }

    private void setButtons(int prog) //Just incase I get lost. It's in setButtons(progression);
    {
        if (prog < 0 || prog >= tp.Count) return; //Verification that it's in the exact progression as god intended

        foreach (Button btn in tp[prog].activeProgression)
        {
            if (btn != null)
            {
                btn.onClick.RemoveListener(clickProgression); // Remove first to avoid duplicates
                btn.onClick.AddListener(clickProgression);
            }
        }
    }

    private void unsetButtons(int prog)
    {
        if (prog < 0 || prog >= tp.Count) return;

        foreach (Button btn in tp[prog].activeProgression)
        {
            if (btn != null)
            {
                btn.onClick.RemoveListener(clickProgression);
            }
        }
    }

    public void clickProgression() //Clicking
    {
        if (isTyping)
        {
            StopAllCoroutines();
            text.text = tp[progression].dialogue;
            isTyping = false;
            return;
        }

        Debug.Log("Progression clicked, incrementing to: " + (progression + 1));

        int previousProgression = progression;
        progression++;

        //Unset buttons for prev progression
        unsetButtons(previousProgression);

        //Set buttons for current progression
        setButtons(progression);

        //Apply changes for new progression
        if (progression >= 0 && progression < tp.Count)
        {
            tp[progression].change(tp[progression], text);
        }

        isTyping = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() //Animation :3
    {
        isTyping = true;
        string dialogue = tp[progression].dialogue;
        int i = 0;
        while (i < dialogue.Length)
        {
            if (dialogue[i] == '<') //This is to hide the <color> thing
            {
                int closeIndex = dialogue.IndexOf('>', i); //End
                if (closeIndex != -1)
                {
                    text.text += dialogue.Substring(i, closeIndex - i + 1);
                    i = closeIndex + 1;
                }
                else
                {
                    //The animation part that actually shows
                    text.text += dialogue[i];
                    i++;
                    yield return new WaitForSeconds(0.02f);
                }
            }
            else
            {
                text.text += dialogue[i];
                i++;
                yield return new WaitForSeconds(0.02f);
            }
        }
        isTyping = false;
    }
}
