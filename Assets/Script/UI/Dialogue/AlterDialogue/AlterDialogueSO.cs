using UnityEngine;

[CreateAssetMenu]
public class AlterDialogueSO : ScriptableObject
{
    [SerializeField]
    private Actors[] actors;

    [SerializeField]
    [TextArea]
    private string[] dialogue;

    [SerializeField]
    private string[] emotions;

    [SerializeField]
    private OptionResponse[] options;

    [Tooltip("Random Purpose Only")]
    [Header("Random Actor Info")]
    public string randActor;
    public Sprite randActorPoit;

    public Actors[] Actors => actors;

    public string[] Dialogues => dialogue;

    public bool HasResponses => Options != null && Options.Length > 0;

    public string[] Emotions => emotions;

    public OptionResponse[] Options => options;

    [Header("ToggleEvent")]
    public bool isEndDialogue;
    public bool isEventTrigger;
    public string eventName;
}
