using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue SO")]
public class DialogueSO : ScriptableObject
{
    [Multiline]
    public List<string> dialogue;
}
