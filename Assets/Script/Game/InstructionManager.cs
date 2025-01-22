using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    private TextMeshPro instrucText;

    void Start()
    {
        instrucText = gameObject.GetComponent<TextMeshPro>();    
        instrucText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instrucText.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instrucText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            instrucText.enabled = false;
        }
    }

}
