using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject EventPanel;

    private AlterDialogueSO currentconv;
    private string currentspeaker;
    private Sprite currentportrait;
    private int step;

    public int conIndex = 0;

    private bool isdialogueOn;
    
    public ActorSO[] actorSO;
    public static bool freezeControl;

    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private TMP_Text content;
    private Image portrait;

    [SerializeField]
    private RectTransform optionPanel;
    [SerializeField]
    private RectTransform optionBtnTemplate;
    [SerializeField]
    private RectTransform optionContainer;

    [SerializeField]
    private float typingSpeed = .02f;
    private Coroutine typeWriterRoutine;
    private bool canContinue = true;

    private NpcMovement currentNPC;
    private ObjectInteraction currentObject;

    public string data;

    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("SpeakerText").GetComponent<TMP_Text>();
        content = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("PoitraitImage").GetComponent<Image>();

        dialogueCanvas.SetActive(false);
        EventPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isdialogueOn && Input.GetKeyDown(KeyCode.E) && canContinue)
        {
            if(step < currentconv.Actors.Length)
            {
                PlayDialogue();
                //if (step == currentconv.Dialogues.Length && currentconv.isEndDialogue == true)
                    //NextCon();
            }
            else
            {
                TurnOffDialogue();
                if (currentconv.isEventTrigger)
                {
                    data = currentconv.eventName;
                    EventPanel.SetActive(true);
                    freezeControl = true;
                }
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
            if (currentconv.HasResponses) 
            {
                ShowOptions(currentconv.Options);
            }
        }

        if(typeWriterRoutine != null)
        {
            StopCoroutine(typeWriterRoutine);
        }

        if (step < currentconv.Dialogues.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterFX(currentconv.Dialogues[step]));
        else
            optionPanel.gameObject.SetActive(true);

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

    public void ShowOptions(OptionResponse[] responses)
    {
        Debug.Log("It's running");

        foreach (OptionResponse option in responses)
        {
            GameObject optionBtn = Instantiate(optionBtnTemplate.gameObject, optionContainer);
            optionBtn.gameObject.SetActive(true);
            optionBtn.GetComponent<TMP_Text>().text = option.ResponseText;
            optionBtn.GetComponent<Button>().onClick.AddListener(() => OnPickedOption(option));
        }

        optionPanel.gameObject.SetActive(true);
    }

    public void OnPickedOption(OptionResponse response)
    {

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

    /*public void InitiateDialogue(NpcMovement dialogue)
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
    }*/

    public void TurnOffDialogue()
    {
        step = 0;
        isdialogueOn = false;
        freezeControl = false;
        if (optionPanel != null)
        {
            optionPanel.gameObject.SetActive(false);
        }
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        else Debug.Log("Dialogue is Destroyed");
    }

    /*public void NextCon()
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
    }*/
}
