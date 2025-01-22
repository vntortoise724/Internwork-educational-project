using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerBtn : MonoBehaviour
{
    private bool isCorrect;

    [SerializeField]
    private TextMeshProUGUI answerText;

    public void SetAnswer(string newText)
    {
        answerText.text = newText;
    }

    public void SetCorrect(bool correct)
    {
        isCorrect = correct;
    }

    public void OnClick()
    {
        if (isCorrect)
        {
            Debug.Log("Tada");
        }
        else
        {
            Debug.Log("Boo");
        }
    }
}
