using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;

public class modeSelector : MonoBehaviour
{

    public GameObject uiSelect;
    public Animator uiAnim;
    [SerializeField] private SpriteRenderer characterSprite;

    private floatingOrb ab1;
    private parryAttack ab2;
    private shockShield ab3;
    private heal ab4;

    public GameObject shieldCircle;
    public GameObject flyingOrb;
    public GameObject floatDistance;
    public GameObject shieldBox;

    public damagePlayer dP;
    public float characterSpeed;
    public GameObject playerLight;

    public bool wheelUp = false;
    public bool psDpadWheel = false;

    private DualShockGamepad gamepad;

    private void Start()
    {
        ab1 = this.transform.GetComponent<floatingOrb>();
        ab2 = this.transform.GetComponent<parryAttack>();
        ab3 = this.transform.GetComponent<shockShield>();
        ab4 = this.transform.GetComponent<heal>();

        characterSpeed = FindObjectOfType<character>().speed;
    }

    public void openWheel(InputAction.CallbackContext context)
    {
        if (context.started && FindObjectOfType<PauseMenu>().paused == false)
        {
            Time.timeScale = 0.05f;
            uiSelect.GetComponent<ModeUI>().canSwap = true;
            uiAnim.SetBool("isOn", true);
            wheelUp = true;
        }
        else if (context.canceled && FindObjectOfType<PauseMenu>().paused == false)
        {
            wheelUp = false;
            Time.timeScale = 1f;
            uiAnim.SetBool("isOn", false);
        }
    }

    public void controllerSwap(InputAction.CallbackContext context)
    {
        if (context.started && wheelUp)
        {
            psDpadWheel = true;
            // print(Gamepad.current.name);
            if (Gamepad.current.name == "DualShock4GamepadHID")
            {
                gamepad = (DualShockGamepad)Gamepad.all[0];
            }
            var value = context.ReadValue<Vector2>();
            if (value.y == 1 && FindObjectOfType<ModeUI>().currentAbilities >= 3)
            {
                // print("top");
                combatMode(3);
                if (gamepad != null)
                {
                    gamepad.SetLightBarColor(Color.yellow);
                }
                FindObjectOfType<ModeUI>().selectedIcons[2].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (value.x == 1 && FindObjectOfType<ModeUI>().currentAbilities >= 1)
            {
                // print("right");
                combatMode(1);
                if (gamepad != null)
                {
                    gamepad.SetLightBarColor(Color.blue);
                }
                FindObjectOfType<ModeUI>().selectedIcons[0].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (value.y == -1 && FindObjectOfType<ModeUI>().currentAbilities >= 2)
            {
                // print("bottom");
                combatMode(2);
                if (gamepad != null)
                {
                    gamepad.SetLightBarColor(Color.magenta);
                }
                FindObjectOfType<ModeUI>().selectedIcons[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (value.x == -1 && FindObjectOfType<ModeUI>().currentAbilities >= 4)
            {
                // print("left");
                combatMode(4);
                if (gamepad != null)
                {
                    gamepad.SetLightBarColor(Color.red);
                }
                FindObjectOfType<ModeUI>().selectedIcons[3].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else if (context.canceled)
        {
            psDpadWheel = false;
            for (int i = 0; i < 4; i++)
            {
                FindObjectOfType<ModeUI>().selectedIcons[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Tab) && FindObjectOfType<PauseMenu>().paused == false)
        {
            // Time.timeScale = 0.05f;
            uiSelect.GetComponent<ModeUI>().canSwap = true;
            uiAnim.SetBool("isOn", true);

        } else if(Input.GetKeyUp(KeyCode.Tab)  && FindObjectOfType<PauseMenu>().paused == false)
        {
            Time.timeScale = 1f;
            uiAnim.SetBool("isOn", false);
        }
        if (Input.GetKey(KeyCode.Tab) && FindObjectOfType<PauseMenu>().paused == false)
        {
            Time.timeScale = 0.05f;
        } else
        {
            //Time.timeScale = 1f;
        }
        */
    }

    public void combatMode(int number)
    {
        disableEverything();
        if (number == 3)
        {
            // print("Floating mode");
            ab1.enabled = true;
            floatDistance.SetActive(true);
            flyingOrb.SetActive(true);
            flyingOrb.transform.position = transform.position;
            characterSprite.color = new Color32(255, 240, 190, 255);

        }
        else if (number == 1)
        {
            ab2.enabled = true;
            // print("Combat mode");
            //shieldBox.SetActive(true);
            playerLight.SetActive(true);
            characterSprite.color = new Color32(190, 255, 255, 255);
        }
        else if (number == 2)
        {
            // print("Shield mode");
            ab3.enabled = true;
            shieldCircle.GetComponent<Animator>().SetBool("isOn", true);
            shieldCircle.GetComponent<CircleCollider2D>().enabled = true;
            //dP.isMortal = true;
            playerLight.SetActive(true);
            characterSprite.color = new Color32(215, 190, 255, 255);
        }
        else if (number == 4)
        {   
            // print("Health mode");
            //InvokeRepeating("healing", 4f, 4f);
            //InvokeRepeating("replenish", 4f, 4f);
            ab4.enabled = true;
            playerLight.SetActive(true);
            characterSprite.color = new Color32(255, 190, 200, 255);
        }
    }

    public void replenish()
    {
        FindObjectOfType<energyController>().recieveEnergy();
    }

    public void healing()
    {
        FindObjectOfType<damagePlayer>().recieveHealth();
    }

    public void disableEverything()
    {
        ab4.disableCharge();

        ab1.enabled = false;
        ab2.enabled = false;
        ab3.enabled = false;
        ab4.enabled = false;

        //dP.isMortal = true;
        ab3.shieldOn = false;
        FindObjectOfType<damagePlayer>().shieldOn = false;
        
        shieldCircle.GetComponent<Animator>().SetBool("isOn", false);
        shieldCircle.GetComponent<CircleCollider2D>().enabled = false;
        flyingOrb.SetActive(false);
        floatDistance.SetActive(false);
        shieldBox.SetActive(false);

        playerLight.SetActive(false);
        CancelInvoke("healing");
        this.GetComponent<dashMove>().enabled = true;
        FindObjectOfType<character>().speed = characterSpeed;
    }

}
