using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueSymbol;

    public bool isClose = false;

    private bool isWithin = false;
    public bool hasTriggered = false;

    private bool justTriggered = false;
    [SerializeField] private AudioSource npcSound;

    private void Update()
    {
        if(!isWithin && hasTriggered && !FindObjectOfType<DialogueManager>().isUp)
        {
            hasTriggered = false;
        }
        /*
        if (Input.GetKeyDown(KeyCode.E) && isWithin && !FindObjectOfType<DialogueManager>().isUp)
        {
            dialogueSymbol.SetActive(false);
            TriggerDialogue();
            //this.gameObject.SetActive(false);
            isWithin = false;
            hasTriggered = true;
        } else if(Input.GetKeyDown(KeyCode.E) && hasTriggered && FindObjectOfType<DialogueManager>().isUp)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
        */
    }

    public void startDialogue()
    {
        // print(gameObject.name + isClose + hasTriggered);
        if (isWithin && !FindObjectOfType<DialogueManager>().isUp && justTriggered == false)
        {
            dialogueSymbol.SetActive(false);
            TriggerDialogue();
            //this.gameObject.SetActive(false);
            isWithin = false;
            hasTriggered = true;
            justTriggered = true;
            if(npcSound != null)
            {
                npcSound.Play();
            }
            StartCoroutine(activateAgain());
        }
        else if (hasTriggered && FindObjectOfType<DialogueManager>().isUp && justTriggered == false)
        {
            // print("next!");
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            justTriggered = true;
            StartCoroutine(activateAgain());
        }
    }

    IEnumerator activateAgain()
    {
        yield return new WaitForSeconds(0.1f);
        justTriggered = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueSymbol.SetActive(true);
        isWithin = true;
        isClose = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueSymbol.SetActive(false);
        isWithin = false;
        isClose = false;
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
