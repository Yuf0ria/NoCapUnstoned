using System.Collections.Generic;

[System.Serializable]
public struct MessageData
{
    public string text; // The text
    public bool isSender; // If it's sender or not (replier)
    public string[] Choices; // If this is null then cont progression.
}

[System.Serializable]
public struct StartMessageData // The messages that appear before!
{
    public string text;
    public bool isSender;
}
