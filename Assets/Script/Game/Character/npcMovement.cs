using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcMovement : MonoBehaviour
{
    [Header("Conversation")]
    public AdvancedDialogueSO[] conv;

    [Header("Manager")]
    public string manager = "DialogueManager";

    [SerializeField]
    private GameObject Notify;
    
    private Transform player;
    private BetterDialManager dialManager;
    private bool dialogueInitiated;

    private void Start()
    {
        GameObject dialogueManagerObject = GameObject.Find(manager);

        if (dialogueManagerObject != null)
        {
            dialManager = dialogueManagerObject.GetComponent<BetterDialManager>();
            if (dialManager == null) 
                Debug.LogError("dialManagaer is not to be found on DialogueManager.");
        }
        else 
            Debug.LogError("DialogueManager is gone !?");

        Notify.SetActive(false);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Notify.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && dialogueInitiated == false)
        {
            player = collision.gameObject.GetComponent<Transform>();

            if (player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                FlipObject();
            }
            else if (player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                FlipObject();
            }

            dialManager.InitiateDialogue(this);
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
        }     
    }

    private void FlipObject()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
