using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class checkpoint : MonoBehaviour
{

    public clickDialogue checkDialogue;
    public bool thisTriggered = false;

    [SerializeField] private bool lastLevel = false;

    // private float guardianEnergy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if (checkDialogue.isClose && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<damagePlayer>().hp = FindObjectOfType<damagePlayer>().maxHp;
            FindObjectOfType<energyController>().energy = FindObjectOfType<energyController>().maxEnergy;
            FindObjectOfType<character>().transform.GetChild(0).GetComponent<Animator>().SetBool("sitting", true);
        }
        if (!FindObjectOfType<DialogueManager>().isUp)
        {
            FindObjectOfType<character>().transform.GetChild(0).GetComponent<Animator>().SetBool("sitting", false);
        }
    }*/

    public void sitDown()
    {
        if (checkDialogue.isClose)
        {
            thisTriggered = true;
            FindObjectOfType<thisIsSoICanFindIt>().gameObject.GetComponent<Animator>().SetBool("checkFade", true);
            FindObjectOfType<damagePlayer>().hp = FindObjectOfType<damagePlayer>().maxHp;
            if(lastLevel == true)
            {
                FindObjectOfType<damagePlayer>().hp = 4;
            }
            // FindObjectOfType<energyController>().energy = FindObjectOfType<energyController>().maxEnergy;
            FindObjectOfType<character>().playerAnim.SetBool("sitting", true);
            FindObjectOfType<checkSaver>().lastCheckPoint = FindObjectOfType<character>().transform.position;
            FindObjectOfType<checkSaver>().lastCheckScene = SceneManager.GetActiveScene().buildIndex;
        }
        if (FindObjectOfType<DialogueManager>().isUp && thisTriggered)
        {
            //tempText.text = "This works!";
            /*FindObjectOfType<character>().playerAnim.SetBool("sitting", false);
            SceneManager.LoadScene(FindObjectOfType<checkSaver>().lastCheckScene);
            thisTriggered = false;
            StartCoroutine(delaySit());
            */
        }
    }

    private void Update()
    {
        if (!FindObjectOfType<DialogueManager>().isUp && thisTriggered)
        {
            FindObjectOfType<character>().playerAnim.SetBool("sitting", false);
            FindObjectOfType<thisIsSoICanFindIt>().gameObject.GetComponent<Animator>().SetBool("checkFade", false);
            SceneManager.LoadScene(FindObjectOfType<checkSaver>().lastCheckScene);
            FindObjectOfType<character>().transform.position = FindObjectOfType<checkSaver>().lastCheckPoint;
            thisTriggered = false;
            //StartCoroutine(delaySit());
        }
    }

    IEnumerator delaySit()
    {
        yield return new WaitForSeconds(0.02f);
    }

}
