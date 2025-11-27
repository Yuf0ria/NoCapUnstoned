using UnityEngine;

[System.Serializable]
public class MessageData
{
    public string text; // The text
    public string name; // The name
    public bool isSender; // If it's sender or not (replier)
    public string[] Choices; // If this is null then cont progression.
    public GameObject linkBox; // Optional hyperlink prefab for this message

    public GameObject Notes; // For tasks done or some shit.
}

[System.Serializable]
public struct StartMessageData // The messages that appear before!
{
    public string text;
    public string name;
    public bool isSender;
}
