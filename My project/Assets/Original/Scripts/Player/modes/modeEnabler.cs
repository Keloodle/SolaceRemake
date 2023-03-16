using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeEnabler : MonoBehaviour
{
    public GameObject ab01;
    public GameObject ab02;
    public GameObject ab03;
    public GameObject ab04;
    public GameObject r01;
    public GameObject r02;
    public GameObject r03;
    public GameObject r04;

    public int currentAbilities = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        switch (currentAbilities)
        {
            case 1:
                ab01.GetComponent<clickSelector>().enabled = true;
                r01.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case 2:
                ab02.GetComponent<clickSelector>().enabled = true;
                r02.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case 3:
                ab03.GetComponent<clickSelector>().enabled = true;
                r03.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case 4:
                ab04.GetComponent<clickSelector>().enabled = true;
                r04.GetComponent<SpriteRenderer>().enabled = true;
                break;
            default:
                break;
        }
    }
}
