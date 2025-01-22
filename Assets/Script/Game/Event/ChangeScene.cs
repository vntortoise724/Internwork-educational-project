using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeScene : MonoBehaviour
{
    public UnityEvent eventHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("He is in");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    eventHandler.Invoke();
                }
            }
        }
    }
}
