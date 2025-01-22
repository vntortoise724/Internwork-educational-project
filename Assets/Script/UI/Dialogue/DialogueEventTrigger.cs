using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject quizPanel;

    private readonly QuestionSetup questionSetup;
    private readonly AdvancedDialogueSO advancedDialogue;

    private void Start()
    {
        quizPanel.SetActive(false);
    }

    private void Update()
    {
        if (!quizPanel)
        {
            Debug.Log("Is Null");
            if (advancedDialogue.isEventTrigger == true)
            {
                quizPanel.SetActive(true);
            }
            else quizPanel.SetActive(false);
        }
    }
}
