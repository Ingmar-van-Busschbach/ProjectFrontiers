using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class DialogueWriter : MonoBehaviour
{
    public static DialogueWriter Instance { get; private set; }
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float waitTillAutoNextDialogue = 3;
    [SerializeField] private UnityEvent endScene;
    private PlayerInputs playerInputs;
    private InputAction interact;
    private DialogueData currentDialogue;
    private AudioSource audioSource;
    private int currentDialogueIndex;
    private Coroutine routine;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        playerInputs = new PlayerInputs();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        Instance = null; // This is technically not needed as on scene loading it should automatically delete the Instance reference, but it is a precaution.
    }

    private void OnEnable()
    {
        interact = playerInputs.Player.Interact;
        interact.Enable();
    }
    private void OnDisable()
    {
        interact.Disable();
    }
    public void InitializeDialogue(DialogueData dialogueData)
    {
        currentDialogue = dialogueData;
        currentDialogueIndex = 0;
        WriteDialogue(currentDialogue.dialogue[currentDialogueIndex]);
    }

    private void Update()
    {
        if (currentDialogue == null)
        {
            return;
        }
        if (interact.WasPressedThisFrame())
        {
            NextDialogue();
        }
    }

    private void WriteDialogue(StructLibrary.Struct_DialogueEntry dialogueEntry)
    {
        nameText.text = dialogueEntry.speakerName;
        audioSource.clip = dialogueEntry.dialogueVoice;
        audioSource.Play();
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(PrintText(dialogueEntry));
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex >= currentDialogue.dialogue.Length)
        {
            StopCoroutine(routine);
            nameText.text = "";
            dialogueText.text = "";
            audioSource.Stop();
            if (currentDialogue.endScene)
            {
                endScene?.Invoke();
            }
            return;
        }
        WriteDialogue(currentDialogue.dialogue[currentDialogueIndex]);
    }



    private IEnumerator PrintText(StructLibrary.Struct_DialogueEntry dialogueEntry)
    {
        char[] letters = dialogueEntry.dialogue.ToCharArray();
        string displayText = "";
        foreach(char letter in letters)
        {
            yield return new WaitForSeconds(dialogueEntry.printDuration);
            displayText += letter;
            dialogueText.text = displayText;
        }
        yield return new WaitForSeconds(waitTillAutoNextDialogue);
        NextDialogue();
    }
}
