using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BetterDialManager : MonoBehaviour
{
    [Tooltip("Choose event")]
    [SerializeField]
    private GameObject EventPanel;
    [SerializeField]
    private string eventScene;

    private AdvancedDialogueSO currentconv;
    private string currentspeaker;
    private Sprite currentportrait;
    private int step;

    public int conIndex = 0;

    private bool isdialogueOn;

    [Tooltip("Choose actors for this conversation")]
    public ActorSO[] actorSO;
    public static bool freezeControl;

    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private TMP_Text content;
    private Image portrait;

    [SerializeField]
    private GameObject optionBtnPrefab;
    private GameObject[] optionBtn;
    private GameObject optionsPanel;
    private TMP_Text[] optionText;


    [SerializeField]
    private List<GameObject> optionBtns = new List<GameObject>();

    [SerializeField]
    private GameObject goNotify;

    [SerializeField]
    private float typingSpeed = .02f;
    private bool canContinue = true;
    private Coroutine typeWriterRoutine;

    private NpcMovement currentNPC;
    private ObjectInteraction currentObject;

    [SerializeField]
    private MinigameCompletion minigameStat;

    public string data;

    // Start is called before the first frame update
    void Start()
    {
        optionBtn = GameObject.FindGameObjectsWithTag("OptionsBtn");
        optionsPanel = GameObject.Find("OptionContainer");
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        else
            Debug.Log("Cannot Find Option Container");

        optionText = new TMP_Text[optionBtn.Length];
        for (int i = 0; i < optionBtn.Length; i++)
        {
            optionText[i] = optionBtn[i].GetComponentInChildren<TMP_Text>();
        }

        for (int i = 0; i < optionBtn.Length; i++)
        {
            optionBtn[i].SetActive(false);
        }

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("SpeakerText").GetComponent<TMP_Text>();
        content = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("PoitraitImage").GetComponent<Image>();

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
        else Debug.Log("Dialogue Canvas hadn't set yet");

        if (EventPanel != null)
            EventPanel.SetActive(false);
        else Debug.Log("Event Object wasn't selected");

        if (goNotify != null)
            goNotify.SetActive(false);
        else Debug.Log("Notify did not set");
    }

    // Update is called once per frame
    void Update()
    {
        if(isdialogueOn && Input.GetKeyDown(KeyCode.E) && canContinue)
        {
            if(step < currentconv.Actors.Length)
            {
                PlayDialogue();
                if (step == currentconv.Dialogues.Length && currentconv.isEndDialogue == true)
                    NextCon();
            }
            else
            {
                TurnOffDialogue();
                if (currentconv.isEventTrigger)
                {
                    if (currentconv.eventName != "Minigame")
                    {
                        data = currentconv.eventName;
                        EventPanel.SetActive(true);
                    } else if (currentconv.eventName == "Minigame" && eventScene != null)
                    {
                        SceneManager.LoadScene(eventScene);
                    }
                    freezeControl = true;
                }

                if (currentconv.isEndEventTrigger == true)
                {
                    currentObject.gameObject.SetActive(false);
                    goNotify.SetActive(true);
                }
            }
        }
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionBtn)
        {
            button.SetActive(false);
        }

        switch (optionNum)
        {
            case 0:
                currentconv = currentconv.optionA;
                break;
            case 1:
                currentconv = currentconv.optionB;
                break;
            case 2:
                currentconv = currentconv.optionC;
                break;
            case 3:
                currentconv = currentconv.optionD;
                break;
        }

        step = 0;
    }

    public void OptionAuto(string[] option)
    {
        foreach (GameObject button in optionBtns)
            { Destroy(button); }
        optionBtns.Clear();

        for (int i = 0; i < option.Length; i++)
        {
            if (!string.IsNullOrEmpty(option[i]))
            {
                GameObject newBtn = Instantiate(optionBtnPrefab, optionsPanel.transform);
                TMP_Text btnText = newBtn.GetComponentInChildren<TMP_Text>();
                btnText.text = option[i];
                int optionIndex = i;
                newBtn.GetComponent<Button>().onClick.AddListener(() => Option(optionIndex));
                optionBtns.Add(newBtn);
            }
        }

    }

    public void PlayDialogue()
    {
        string currentEmo = "Default";
        if (currentconv.Emotions != null && currentconv.Emotions.Length > step)
            currentEmo = currentconv.Emotions[step];

        if (currentconv.Actors[step] == Actors.Random)
        {
            SetActorInfo(false, currentEmo);
        } else 
            SetActorInfo(true, currentEmo);

        actor.text = currentspeaker;
        portrait.sprite = currentportrait;

        if (currentconv.Actors[step] == Actors.Branch)
        {
            for (int i = 0; i < currentconv.dialogueOption.Length; i++)
            {
                if (currentconv.dialogueOption[i] == null)
                    optionBtn[i].SetActive(false);
                else
                {
                    optionText[i].text = currentconv.dialogueOption[i];
                    optionBtn[i].SetActive(true);
                }

                optionBtn[0].GetComponent<Button>().Select();
            }

            //OptionAuto(currentconv.dialogueOption);
        }

        if(typeWriterRoutine != null)
        {
            StopCoroutine(typeWriterRoutine);
        }

        if (step < currentconv.Dialogues.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterFX(currentconv.Dialogues[step]));
        else
            optionsPanel.SetActive(true);

        freezeControl = true;
        dialogueCanvas.SetActive(true);
        step++;
    }

    public void SetActorInfo(bool random, string emotion = "Default")
    {
        if (random)
        {
            for (int i = 0; i < actorSO.Length; i++)
            {
                if (actorSO[i].name == currentconv.Actors[step].ToString())
                {
                    currentspeaker = actorSO[i].actorName;
                    actorSO[i].InitializeDictionary();
                    currentportrait = actorSO[i].GetEmotionSprite(emotion);
                }
            }
        }
        else
        {
            currentspeaker = currentconv.randActor;
            currentportrait = currentconv.randActorPoit;
        }
    }

    IEnumerator TypeWriterFX(string line)
    {
        content.text = "";
        canContinue = false;
        bool addingRichText = false;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                content.text = line;
                break;
            }

            if (letter == '<' || addingRichText)
            {
                addingRichText = true;
                content.text += letter;
                if (letter == '>')
                    addingRichText = false;
            }
            else
            {
                content.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        canContinue = true;
    }

    public void InitiateDialogue(NpcMovement dialogue)
    {
        currentNPC = dialogue;
        currentconv = currentNPC.conv[conIndex];
        isdialogueOn = true;
    }

    public void InitiateDialogueOnObject(ObjectInteraction dialogue)
    {
        currentObject = dialogue;
        currentconv = currentObject.conv[conIndex];
        isdialogueOn = true;
    }

    public void TurnOffDialogue()
    {
        step = 0;
        isdialogueOn = false;
        freezeControl = false;
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        else Debug.Log("Dialogue is Destroyed");
    }

    public void NextCon()
    {
        if (currentNPC != null && conIndex < currentNPC.conv.Length - 1 && currentconv.isEndDialogue)
        {
            conIndex++;
            currentconv = currentNPC.conv[conIndex];
        }
        else if (currentObject != null && conIndex < currentObject.conv.Length - 1 && currentconv.isEndDialogue)
        {
            conIndex++;
            currentconv = currentObject.conv[conIndex];
        }           
        else
            Debug.Log(conIndex);
    }
}

public enum Actors
{
    Cat,
    Lap,
    Main,
    Mom,
    Te,
    Thang,
    Vinh,
    Random,
    Branch
};