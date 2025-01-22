using UnityEngine;
using System;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField]
    private AdvancedDialogueSO adDialogue;
    [SerializeField]
    private ResponseEvent[] responseEvents;

    public ResponseEvent[] Events => responseEvents;

    public void OnValidate()
    {
        if (adDialogue == null) return;
        if (responseEvents == null) return;
        if (responseEvents != null && responseEvents.Length == adDialogue.dialogueOption.Length) return;

        for (int i = 0; i < responseEvents.Length; i++) 
        {
            if (responseEvents[i] != null)
                responseEvents[i].name = adDialogue.dialogueOption[i];
            responseEvents[i] = gameObject.AddComponent<ResponseEvent>();
        }
    }   
}
