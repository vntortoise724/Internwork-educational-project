using UnityEngine;

[CreateAssetMenu]
public class AdvancedDialogueSO : ScriptableObject
{
    [SerializeField]
    private Actors[] actors;

    [SerializeField]
    [TextArea]
    private string[] dialogue;

    [SerializeField]
    private string[] emotions;

    [Tooltip("Random Purpose Only")]
    [Header("Random Actor Info")]
    public string randActor;
    public Sprite randActorPoit;

    public Actors[] Actors => actors;

    public string[] Dialogues => dialogue;

    public string[] Emotions => emotions;

    [Header("Options")]
    [TextArea]
    public string[] dialogueOption;

    public AdvancedDialogueSO optionA;
    public AdvancedDialogueSO optionB;
    public AdvancedDialogueSO optionC;
    public AdvancedDialogueSO optionD;

    [Header("ToggleEvent")]
    public bool isEndDialogue;
    public bool isEventTrigger;
    public bool isEndEventTrigger;
    public string eventName;
}
