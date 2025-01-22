using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInstruction : MonoBehaviour
{
    [SerializeField]
    private GameObject instruction;

    public void ShowHelp()
    {
        instruction.SetActive(true);
    }

    public void HideHelp()
    {
        instruction.SetActive(false);
    }

}
