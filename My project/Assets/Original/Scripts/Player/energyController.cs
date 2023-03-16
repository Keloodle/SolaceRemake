using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class energyController : MonoBehaviour
{
    public float energy = 10;
    public TextMeshProUGUI text;
    public float maxEnergy;

    // public healthBar energyBar;
    public SpriteRenderer energyBar;
    private float barEnergy;
    public Vector2 energyPosition;

    void Start()
    {
        maxEnergy = energy;
        barEnergy = energy;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate where to send the soules
        energyPosition = new Vector2((energy-5f)/2, 0) + new Vector2(energyBar.transform.position.x, energyBar.transform.position.y);

        // energyBar.barSlider.value = energy;
        // energyBar.material.SetFloat("_Health", energy / maxEnergy);
        barEnergy = Mathf.MoveTowards(barEnergy, energy, 10 * Time.deltaTime);
        energyBar.material.SetFloat("_Health", barEnergy / maxEnergy);


        // text.text = "Energy: " + energy + "/10";
    }

    public void reduceEnergy(float energyReduction)
    {
        energy -= energyReduction;
    }

    public void recieveEnergy()
    {
        //print("trying to charge");
        //print(energy);
        energy++;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }
}
