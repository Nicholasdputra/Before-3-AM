using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public Sprite npcSprite;
    // public int favorability;
    public DialogueDataSO dialogueData;
    [SerializeField] public string firstTimeBaseDialogue;
    [SerializeField] public string nonFirstTimeBaseDialogue;
}