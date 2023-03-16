using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class inventorySelector : MonoBehaviour
{
    public int selected;

    public string currentDescription;
    public string currentItem;

    [SerializeField] private TextMeshProUGUI loreText;
    [SerializeField] private TextMeshProUGUI nameText;


    public GameObject[] loreImages;
    public GameObject[] outlines;

    [SerializeField] private GameObject[] pageTurners;
    [SerializeField] private string[] pages;
    public int pageNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void turnPageUp()
    {
        if(pageNumber < pages.Length-1)
        {
            pageNumber++;
        }
        currentDescription = pages[pageNumber];
    }

    public void turnPageDown()
    {
        if (pageNumber > 0)
        {
            pageNumber--;
        }
        currentDescription = pages[pageNumber];
    }

    public void Update()
    {
        if(selected == 2)
        {
            for (int i = 0; i < pageTurners.Length; i++)
            {
                pageTurners[i].SetActive(true);
            }
        } else if(selected != 2)
        {
            for (int i = 0; i < pageTurners.Length; i++)
            {
                pageTurners[i].SetActive(false);
            }
        }

        loreText.text = currentDescription;
        nameText.text = currentItem;
        // print(selected);

        for (int i = 0; i < loreImages.Length; i++)
        {
            loreImages[i].SetActive(!FindObjectOfType<checkSaver>().loreFound[i]);
        }

        //pageNumber = 0;
    }
}
