using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaDialogue : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        this.gameObject.SetActive(false);
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
