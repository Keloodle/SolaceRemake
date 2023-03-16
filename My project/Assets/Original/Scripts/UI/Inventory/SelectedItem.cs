using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour/*, IPointerEnterHandler*/
{
    private inventorySelector selector;
    [SerializeField] private int thisValue;
    [SerializeField] private string lore;
    [SerializeField] private string itemName;

    // Start is called before the first frame update
    void Start()
    {
        selector = FindObjectOfType<inventorySelector>();
    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        selector.selected = thisValue;
        selector.currentDescription = lore;
        selector.currentItem = itemName;
    }*/

    private void FixedUpdate()
    {
        for (int i = 0; i < selector.outlines.Length; i++)
        {
            if (i != thisValue)
            {
                selector.outlines[i].GetComponent<Image>().color = Color.white;
            }
        }
    }


    public void swapColour()
    {
        for (int i = 0; i < selector.outlines.Length; i++)
        {
            if (i != thisValue)
            {
                selector.outlines[i].GetComponent<Image>().color = Color.white;
            }
        }
        selector.outlines[thisValue].GetComponent<Image>().color = Color.cyan;
    }

    public void controllerSelected()
    {
        for (int i = 0; i < selector.outlines.Length; i++)
        {
            if (i != thisValue)
            {
                selector.outlines[i].GetComponent<Image>().color = Color.white;
            }
        }
        selector.outlines[thisValue].GetComponent<Image>().color = Color.cyan;

        if (FindObjectOfType<checkSaver>().loreFound[thisValue] == true)
        {
            selector.selected = thisValue;
            selector.currentDescription = lore;
            selector.currentItem = itemName;
        }
    }
}
