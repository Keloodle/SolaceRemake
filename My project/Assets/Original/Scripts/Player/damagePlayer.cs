using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class damagePlayer : MonoBehaviour
{
    public float hp = 5;
    public TextMeshProUGUI text;
    public float maxHp;
    private Rigidbody2D rb;
    public GameObject hitEffect;

    public Animator deathAnim;
    public bool isDead = false;
    public bool isMortal = true;
    public bool shieldOn = false;
    private bool enterIn = false;
    private bool instantDeath = false;

    public healthBar hpBar;
    private dashMove dashScript;
    [SerializeField] private Transform[] healthOrbs;
    [SerializeField] private GameObject hpEffect;

    public Vector3 lastCheckPos;

    private IEnumerator pause;

    // Start is called before the first frame update
    void Start()
    {
        lastCheckPos = transform.position;
        maxHp = hp;
        rb = this.GetComponent<Rigidbody2D>();
        dashScript = FindObjectOfType<dashMove>();
    }

    public void enterDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            enterIn = true;
        }
        if (context.canceled)
        {
            enterIn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Old alpha bar that we don't use
        // hpBar.barSlider.value = hp;

        // Logic for which orbs appear
        for (int i = 0; i < healthOrbs.Length; i++)
        {
            if (hp > i)
            {
                if (healthOrbs[i].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                {
                    // print("new orb");
                    healthOrbs[i].gameObject.GetComponent<Animator>().SetTrigger("grow");
                }

                healthOrbs[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
            } else if (hp <= i)
            {
                if (healthOrbs[i].gameObject.GetComponent<SpriteRenderer>().enabled == true)
                {
                    var prefab = Instantiate(hpEffect, healthOrbs[i].gameObject.transform.position, healthOrbs[i].gameObject.transform.rotation);
                    prefab.transform.parent = healthOrbs[i].gameObject.transform;
                    Destroy(prefab, 1f);
                }
                healthOrbs[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        // Prototype text that we don't use
        // text.text = "HP: " + hp + "/" + maxHp;

        // Respawn
        if (isDead && enterIn)
        {
            // Destroy(FindObjectOfType<dontDestroy>().gameObject);
            SceneManager.LoadScene(FindObjectOfType<checkSaver>().lastCheckScene);
            transform.position = FindObjectOfType<checkSaver>().lastCheckPoint;
            GetComponent<energyController>().energy = GetComponent<energyController>().maxEnergy;
            hp = maxHp;
            deathAnim.SetBool("isDead", false);
            FindObjectOfType<character>().enabled = true;
            isDead = false;
        }

    }


    public void recieveDamage()
    {
        if (hp > 0 && isMortal == true && !shieldOn || instantDeath == true)
        {

            hp--;
            isMortal = false;
            Invoke("becomeMortal", 1f);
            FindObjectOfType<CameraScript>().gotHit();
            FindObjectOfType<modeSelector>().uiAnim.SetBool("isOn", false);
            FindObjectOfType<modeSelector>().wheelUp = false;
            FindObjectOfType<character>().playerAnim.SetTrigger("hurt");

            Time.timeScale = 0.2f;
            Invoke("fixTime", 0.2f/5f);

            var prefab = Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y, -5), transform.rotation);
            Destroy(prefab, 4);

            if(dashScript.dashTime < dashScript.startDashTime)
            {
                dashScript.enemyCollided();
            }

            if (hp == 0)
            {
                instantDeath = false;
                deathAnim.SetBool("isDead", true);
                isDead = true;
                FindObjectOfType<character>().enabled = false;
            }
        }

    }

    void fixTime()
    {
        Time.timeScale = 1;
    }
    

    public void recieveHealth()
    {
        hp++;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 14 || collision.gameObject.layer == 11 || collision.gameObject.layer == 19)
        {
            if (isMortal && !shieldOn)
            {
                recieveDamage();
                if (this.transform.position.x <= collision.transform.position.x)
                {
                    rb.velocity = new Vector2(-6, 6);
                }
                else if (this.transform.position.x > collision.transform.position.x)
                {
                    rb.velocity = new Vector2(6, 6);
                }
            } else if(isMortal && shieldOn)
            {
                if (this.transform.position.x <= collision.transform.position.x)
                {
                    rb.velocity = new Vector2(-4, 3);
                }
                else if (this.transform.position.x > collision.transform.position.x)
                {
                    rb.velocity = new Vector2(4, 3);
                }
            }
        }
        else if(collision.gameObject.layer == 18)
        {
            hp = 1;
            instantDeath = true;
            recieveDamage();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.layer == 11)
        {
            if (isMortal)
            {
                recieveDamage();
                if (this.transform.position.x < other.transform.position.x)
                {
                    rb.velocity = new Vector2(-6, 6);
                }
                else if (this.transform.position.x > other.transform.position.x)
                {
                    rb.velocity = new Vector2(6, 6);
                }
            }
        }
    }

    void becomeMortal()
    {
        isMortal = true;
    }
}
