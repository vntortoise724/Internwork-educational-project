using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameCompletionSO")]
public class MinigameCompletion : ScriptableObject
{
    public bool isPlayingMinigame = false;
    public bool completion = false;
}
