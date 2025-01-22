using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform destination;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isLocked", false);
    }
    public Transform GetDestination()
    {
        return destination;
    }

    public void EnableAnimation()
    {
        animator.SetBool("isLocked", false);
    }
}
