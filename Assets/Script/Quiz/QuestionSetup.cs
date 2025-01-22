using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionSetup : MonoBehaviour
{
    [Header("Quiz Setup")]
    public string data = "";
    [SerializeField]
    private GameObject quizCanvas;
    [SerializeField]
    private List<QuestionData> questionPool;
    [SerializeField]
    private List<QuestionData> selectedQuestions;
    private QuestionData currentQuest;

    [Header("Quiz Panel Setup")]
    [SerializeField]
    private TextMeshProUGUI questionTxt;
    [SerializeField]
    private TextMeshProUGUI levelTxt;
    [SerializeField]
    private AnswerBtn[] answerBtns;

    [Header("Progress")]
    [SerializeField]
    private TextMeshProUGUI progressTxt;
    [SerializeField]
    private TextMeshProUGUI correctAnswerTxt;

    [SerializeField]
    private int correctChoice;
    [SerializeField]
    private int numOfQuestions = 5;
    [SerializeField]
    private int requirement = 3;

    private int correctCount = 0;
    private int currentQuestion = 0;

    private bool isAnswerCorrect;

    public bool isFinished = false;

    private EnlightenManager enlightenManager;
    private BetterDialManager dialManager;
    private QuizToObject q2Ob;

    private void Awake()
    {
        enlightenManager = FindObjectOfType<EnlightenManager>();
        dialManager = FindObjectOfType<BetterDialManager>();
        q2Ob = FindObjectOfType<QuizToObject>();
    }

    private void Update()
    {
        if (data != dialManager.data)
        {
            InitializeQuiz();
        }
    }

    public void InitializeQuiz()
    {
        GetQuestionAssets();
        GenerateQuestionSet();
        DisplayCurrentQuestion();
    }

    public void GetQuestionAssets()
    {
        switch (dialManager.data)
        {
            case "Act1":
                data = dialManager.data;
                break;
            case "Act2":
                data = dialManager.data;
                break;
            case "Act3":
                data = dialManager.data;
                break;
            case "English":
                data = dialManager.data;
                break;
            case "Quiz":
                data = dialManager.data;
                break;
            default:
                break;
        }
        questionPool = new List<QuestionData>(Resources.LoadAll<QuestionData>(data));
    }

    public void GenerateQuestionSet()
    {
        selectedQuestions = new List<QuestionData>();

        for (int i = 0; i < numOfQuestions; i++)
        {
            if (questionPool.Count == 0) 
            {
                Debug.Log("No More Question");
                break;
            }
            int randomIndex = Random.Range(0, questionPool.Count);
            selectedQuestions.Add(questionPool[randomIndex]);
            questionPool.RemoveAt(randomIndex);
        }
    }

    public void SetQuestionValue()
    {
        if (currentQuest != null)
        {
            questionTxt.text = currentQuest.question;
            levelTxt.text = currentQuest.level;
        }
        else
            Debug.Log("Question is null");
    }

    public void SetAnswerValue()
    {
        List<string> answer = RandomizeAnswers(new List<string>(currentQuest.answer));

        for (int i = 0; i < answerBtns.Length; i++)
        {
            bool isCorrect = false;
            if (i  == correctChoice)
                isCorrect = true;

            answerBtns[i].SetCorrect(isCorrect);
            answerBtns[i].SetAnswer(answer[i]);
        }
    }

    public void QuizProcess(int select)
    {
        isAnswerCorrect = (select == correctChoice);
        if (isAnswerCorrect)
        {
            correctCount++;
        }

        currentQuestion++;
        DisplayCurrentQuestion();
    }

    private void CheckCompletion()
    {
        if (correctCount >= requirement)
        {
            Debug.Log("Quiz Completed");
            quizCanvas.SetActive(false);
            BetterDialManager.freezeControl = false;
            isFinished = true;
            q2Ob.isQuizCompleted = true;
        }
        else
        {
            Debug.Log("Try again, not enough correct answers");
            enlightenManager.enliNum--;
            RetryQuiz();
        }
    }

    private void RetryQuiz()
    {
        correctCount = 0;
        currentQuestion = 0;
        DisplayCurrentQuestion();
    }

    public void DisplayCurrentQuestion()
    {
        if (currentQuestion == selectedQuestions.Count)
        {
            CheckCompletion();
            return;
        }

        currentQuest = selectedQuestions[currentQuestion];
        SetQuestionValue();
        SetAnswerValue();

        progressTxt.text = numOfQuestions.ToString();
        correctAnswerTxt.text = correctCount.ToString();
    }
    private List<string> RandomizeAnswers(List<string> origin)
    {
        bool correctChosen = false;
        List<string> newList = new List<string>();
        correctChoice = Random.Range(0, answerBtns.Length);

        for (int i = 0; i < answerBtns.Length; i++)
        {
            int random = Random.Range(0, origin.Count);

            if (random == 0 && !correctChosen)
            {
                correctChoice = i;
                correctChosen = true;
            }
                newList.Add(origin[random]);
                origin.RemoveAt(random);
        }
        return newList;
    }
}


