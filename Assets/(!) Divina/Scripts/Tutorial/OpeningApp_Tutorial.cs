using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * HEY READ THE READFIRST.CS SCRIPT INSIDE THE SAME
 * FOLDER AS THIS ONE BEFORE YOU DO ANYTHING THANKS <3
 */

public class OpeningApp_Tutorial : MonoBehaviour
{
    /// <summary>
    /// HEY ALSO, UH, THIS IS THE MESSAGING SCRIPT BTW THAT I RAWDOGGED... LOL
    /// </summary>


    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject Box;
    [SerializeField] private Button reply;

    public void Messaging(TextMeshProUGUI text)
    {
        Box.SetActive(true);
        message.text = text.text;
        reply.interactable = false;
    }
}
