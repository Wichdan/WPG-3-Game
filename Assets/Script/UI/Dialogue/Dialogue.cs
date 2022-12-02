using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/New Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public List<Conversation> conversation;
}

[System.Serializable]
public class Conversation
{
    public string charName;
    public Sprite leftPortrait, rightPortrait;
    [TextArea(1,3)]
    public string sentence;
    public bool isNarrator;
}
