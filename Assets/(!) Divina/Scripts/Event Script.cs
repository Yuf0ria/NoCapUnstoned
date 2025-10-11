using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
//Weird way to format it like this, I know. - Nicaia

//How the hell do you do this thing???
[Serializable]
public struct Progression
{
    [Header("GameObjects")]
    [SerializeField] private GameObject[] toSetTrue;
    [SerializeField] private GameObject[] toSetFalse;
    [SerializeField] public Button[] activeProgression; //For other voids to access
                                                        //Task List[] and Messaging Inbox may be possible to do at to set true/false or some shit.


    [Header("Story/Dialogue")]
    [SerializeField] public string dialogue;


    public void change(Progression p, TextMeshProUGUI text)
    {
        foreach (var setTrue in toSetTrue)
        { setTrue.SetActive(true); }
        foreach (var setFalse in toSetFalse)
        { setFalse.SetActive(false); }
        if (text != null)
        {
            text.text = string.Empty;
        }
    }

} //IVE BEEN LOOKING FOR TS FOR SO LONG BRUUHHHHH.

public class EventScript : MonoBehaviour
{
    [Header("Objects n Stuff!")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Progression")]
    [SerializeField] public int progression; //Current Progression
    //[SerializeField] private int previousProgression = -1; //Previous Progression
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
        foreach (char c in tp[progression].dialogue.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(0.02f); //0.02f is just right
        }
        isTyping = false;

        //For monologues or seallie talking
    }
}
