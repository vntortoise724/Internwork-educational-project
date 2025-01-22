using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Conversation")]
    public AdvancedDialogueSO[] conv;

    [Header("Manager")]
    public string manager = "DialogueManager";

    [SerializeField]
    private GameObject Notify;

    private BetterDialManager dialManager;
    private QuizToObject q2Ob;
    private bool dialogueInitiated;

    // Start is called before the first frame update
    void Start()
    {
        GameObject dialogueManagerObject = GameObject.Find(manager);

        if (dialogueManagerObject != null)
        {
            dialManager = dialogueManagerObject.GetComponent<BetterDialManager>();
            if (dialManager == null)
                Debug.LogError("dialManager is not to be found on DialogueManager.");
        }
        else
            Debug.LogError("DialogueManager is gone !?");

        q2Ob = FindObjectOfType<QuizToObject>();

        Notify.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Notify.SetActive(true);
            if (q2Ob.isQuizCompleted == true)
            {
                dialManager.conIndex = 1;
            }
            else
                dialManager.conIndex = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && dialogueInitiated == false)
        {
            dialManager.InitiateDialogueOnObject(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Notify.SetActive(false);
            dialManager.TurnOffDialogue();
            dialogueInitiated = false;
            dialManager.conIndex = 0;
        }
    }
}
