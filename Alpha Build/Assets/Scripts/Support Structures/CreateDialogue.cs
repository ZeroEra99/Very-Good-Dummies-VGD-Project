using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateDialogue
{
    public string name;
    [HideInInspector]
    public bool interacted = false;
    [TextArea(3, 10)]
    public string[] sentences;
    [TextArea(3, 10)]
    public string[] repeatedSentences;
}
