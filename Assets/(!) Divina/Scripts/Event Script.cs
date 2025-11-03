using UnityEngine; using System.Collections.Generic; using TMPro; using UnityEngine.UI; using System; using System.Collections;
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
        { text.text = string.Empty; }
    }

} //IVE BEEN LOOKING FOR TS FOR SO LONG BRUUHHHHH.

//Copy pasted this from tutorial progression. some of these comments do not make sense.

public class EventScript : MonoBehaviour
{
    [Header("Objects n Stuff!")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Progression")]
    [SerializeField] public int progression; //Current Progression
    [SerializeField] private List<Progression> p; //The list :3 //ALSO LIKE, NO WONDER I CAN'T SEE THE LIST IN THE INSPECTOR.
                                                  // ALL I NEED TO DO IS TO WRITE TS BRUH

    private bool isTyping = false;

    private void Start()
    {
        if (progression >= 0 && progression < p.Count)
        {
            p[progression].change(p[progression], text);
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
        if (prog < 0 || prog >= p.Count) return; //Verification that it's in the exact progression as god intended

        foreach (Button btn in p[prog].activeProgression)
        {
            if (btn != null)
            {
                btn.onClick.RemoveListener(clickProgression); //Remove first to avoid duplicates
                btn.onClick.AddListener(clickProgression);
            }
        }
    }

    private void unsetButtons(int prog) //To make sure that no buttons asides from the intented button progresses the thing lol
    {
        if (prog < 0 || prog >= p.Count) return;

        foreach (Button btn in p[prog].activeProgression)
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
            text.text = p[progression].dialogue;
            isTyping = false;
            return;
        }

        //Debug.Log("Progression clicked, incrementing to: " + (progression + 1));

        int previousProgression = progression;
        progression++;

        //Unset buttons for prev progression
        unsetButtons(previousProgression);

        //Set buttons for current progression
        setButtons(progression);

        //Apply changes for new progression
        if (progression >= 0 && progression < p.Count)
        {
            p[progression].change(p[progression], text);
        }

        isTyping = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() //Animation :3
    {
        isTyping = true;
        string dialogue = p[progression].dialogue;
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

        //For monologues or seallie talking
    }
}
