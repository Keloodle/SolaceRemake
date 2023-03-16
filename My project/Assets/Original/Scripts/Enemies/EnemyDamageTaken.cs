using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTaken : MonoBehaviour
{
    public Renderer hpRenderer;
    private float startingHealth;
    public float hp = 10;

    public bool isMortal = true;
    public bool isDead = false;

    public int hDir;
    public Rigidbody2D rb;

    public GameObject healingHeart;
    public Animator enemyAnimator;
    public GameObject enemyToDie;
    public BoxCollider2D damager;

    [SerializeField] private bool hasDeathAnim = false;
    [SerializeField] private float deathTime = 0;
    [SerializeField] private LayerMask deathLayer;

    // Start is called before the first frame update
    void Start()
    {
        startingHealth = hp;
    }

    // Update is called once per frame
    void Update()
    {
        hpRenderer.material.SetFloat("_Health", hp / startingHealth);

    }

    public void recieveDamage(float dmg)
    {

        if (hp > 0)
        {
            hp -= dmg;
            rb.velocity = new Vector2(3 * hDir, 3);

            // Checks to see if we have an animator.

            if (enemyAnimator != null)
            {
                // If we have an animator, we can trigger a hurt animation.
                enemyAnimator.SetTrigger("hurt");
            }

            // Death
            if (hp <= 0)
            {
                
                isDead = true;
                if(damager != null)
                {
                    damager.enabled = false;
                }
                if (hasDeathAnim)
                {
                    this.gameObject.layer = 23;
                    enemyAnimator.SetBool("dead", true);
                    Invoke("spawnHeart", deathTime);
                    Destroy(enemyToDie.gameObject, deathTime);
                } else
                {
                    Invoke("spawnHeart", 0);
                    Destroy(enemyToDie.gameObject);
                }
            }
        }
    }

    void spawnHeart()
    {
        // print("spawn?");
        // Checks to see if we have a prefab to spawn. If we have, do it.
        if (healingHeart != null)
        {
            var prefab = Instantiate(healingHeart, this.transform.position, this.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            recieveDamage(1);
        }
    }
}
