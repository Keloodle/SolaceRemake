using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemy : MonoBehaviour
{
    public float dmgTaken = 0;

    public GameObject projectile;
    public GameObject player;
    public float frequency = 2f;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("shooting", frequency, frequency);
    }

    public void shooting()
    {
        GameObject prefab = Instantiate(projectile, this.transform.position, this.transform.rotation);
        prefab.transform.right = player.transform.position - prefab.transform.position;
        //prefab.transform.LookAt(player);
    }

    void Update()
    {
        
        //this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = dmgTaken.ToString();
    }

    public void increaseDmg()
    {
        dmgTaken++;
        if(dmgTaken == 10)
        {
            Destroy(this.gameObject);
        }
    }

}
