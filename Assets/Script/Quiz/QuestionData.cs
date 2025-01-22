using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiz", menuName ="ScriptableObjects/Quiz")]
public class QuestionData : ScriptableObject
{
    [TextArea]
    public string question;
    public string level;
    [Tooltip("Correct answer should always be listed first")]
    public string[] answer;
}
