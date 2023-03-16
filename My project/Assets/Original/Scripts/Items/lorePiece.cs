using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lorePiece : MonoBehaviour
{
    public clickDialogue checkDialogue;
    private bool isWithin = false;
    [SerializeField] private int thisPiece;

    
    private void Update()
    {
        if (isWithin && FindObjectOfType<DialogueManager>().isUp && checkDialogue.hasTriggered)
        {
            FindObjectOfType<checkSaver>().loreFound[thisPiece] = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isWithin = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isWithin = false;
    }
}
