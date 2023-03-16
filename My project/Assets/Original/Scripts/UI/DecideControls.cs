using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideControls : MonoBehaviour
{
    [SerializeField] private GameObject keyboard;
    [SerializeField] private GameObject xbox;
    [SerializeField] private GameObject ps4;
    // [SerializeField] private GameObject switchCont;

    private controllerTypes ct;
    // Start is called before the first frame update
    void Start()
    {
        ct = FindObjectOfType<controllerTypes>();
    }

    // Update is called once per frame
    void Update()
    {
        keyboard.SetActive(ct.keyboard);
        xbox.SetActive(ct.Xbox_One_Controller);
        ps4.SetActive(ct.PS4_Controller);
    }
}
