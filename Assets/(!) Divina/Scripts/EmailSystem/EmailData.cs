using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmailData
{
    public string emailText; // The text
    public string emailTitle; // The title
    public string name; // The name
    public Sprite profile; // The profile sprite
    public string[] Choices; // If this is null then cont progression.
    public GameObject linkBox; // Optional hyperlink prefab for this message
}
