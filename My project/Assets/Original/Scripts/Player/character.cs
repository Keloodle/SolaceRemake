using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class character : MonoBehaviour
{
    public float speed = 0.1f;
    [SerializeField] private float m_JumpForce = 400f;

    const float k_GroundedRadius = .01f;
    private Rigidbody2D rb;

    public float groundedHeight = 0.51f;
    public float checkRate = 1.0f;
    public bool grounded = false;
    public bool onWall = false;
    public LayerMask groundLayer;
    private bool shockShieldOn;
    public float heightOffset = 0.25f;
    public float looking = 1;

    private float startHeight = 10000;
    public float jumpHeight = 4;
    
    public SpriteRenderer stickRender;
    public Animator playerAnim;
    public bool isSitting = false;
    [SerializeField] private float wallDistance = 0.325f;

    private IEnumerator coroutine;


    private void Start()
    {
        // - After 0 seconds, prints "Starting 0.0"
        // - After 0 seconds, prints "Before WaitAndPrint Finishes 0.0"
        // - After 2 seconds, prints "WaitAndPrint 2.0"
        //print("Starting " + Time.time);

        // Start function WaitAndPrint as a coroutine.

        coroutine = WaitAndPrint(2.0f);
        StartCoroutine(coroutine);

        //print("Before WaitAndPrint Finishes " + Time.time);
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //print("WaitAndPrint " + Time.time);
        }
    }

    void Update()
    {
        playerAnim.SetBool("grounded", grounded);

        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("sit"))
        {
            isSitting = true;
        } else
        {
            isSitting = false;
        }

        playerAnim.SetFloat("velocity", rb.velocity.y);
        GroundCheck();

        // Checks if shield is on
        if(FindObjectOfType<shockShield>().enabled == true && FindObjectOfType<shockShield>().shieldOn == true)
        {
            shockShieldOn = true;
        } else
        {
            shockShieldOn = false;
        }

        // Jumping
        /*
        if (!shockShieldOn && grounded && !isSitting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                startHeight = this.transform.position.y;
                rb.velocity = new Vector2(rb.velocity.x, m_JumpForce);
            }
        }
        */
        // Moving left
        if (!onWall && FindObjectOfType<dashMove>().isDashing == false && !isSitting && FindObjectOfType<PauseMenu>().paused == false && FindObjectOfType<PauseMenu>().inventoryUp == false)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                stickRender.flipX = true;
                looking = -1;
                playerAnim.SetBool("running", true);
            }
            // Moving right
            if (Input.GetAxis("Horizontal") > 0)
            {
                stickRender.flipX = false;
                looking = 1;
                playerAnim.SetBool("running", true);
            }
        }

        if(Input.GetAxis("Horizontal") == 0)
        {
            playerAnim.SetBool("running", false);
        }

        // Wall jump

        wallCheck();
        if (onWall)
        {

            playerAnim.SetBool("onWall", true);

            //rb.velocity = new Vector2(rb.velocity.x, 0);

            /*if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.AddForce(new Vector2(300*-looking, 600));
                StartCoroutine(waitWall());
            }*/

        } else if (!onWall)
        {
            playerAnim.SetBool("onWall", false);
        }

        if (this.transform.position.y > startHeight + jumpHeight)
        {
            //rb.velocity = new Vector2(rb.velocity.x, 0);
            startHeight = 10000;
        }

    }

    public void jump(InputAction.CallbackContext context)
    {
        //print("jump");
        // Jumping
        if (context.performed && !shockShieldOn && grounded && !isSitting && FindObjectOfType<PauseMenu>().paused == false && FindObjectOfType<PauseMenu>().inventoryUp == false && FindObjectOfType<DialogueManager>().isUp == false)
        {
            startHeight = this.transform.position.y;
            rb.velocity = new Vector2(rb.velocity.x, m_JumpForce);
            playerAnim.SetTrigger("jump");
        }
    }

    IEnumerator waitWall()
    {
        yield return new WaitForSeconds(0.02f);

        //After we have waited 5 seconds print the time again.
        //rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    void wallCheck()
    {
        if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -Vector3.right, wallDistance, groundLayer) && !grounded)
        {
            onWall = true;
            stickRender.flipX = false;
            looking = 1;
        }
        else if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right, wallDistance, groundLayer) && !grounded)
        {
            onWall = true;
            stickRender.flipX = true;
            looking = -1;
        }
        else
        {
            onWall = false;
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), -Vector3.right * wallDistance, Color.yellow);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.right * wallDistance, Color.yellow);
        }
    }
    
        // public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, groundLayer, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);

    void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + heightOffset), groundedHeight, groundLayer);

        /*if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z), Vector3.down, groundedHeight + heightOffset, groundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z), groundedHeight);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (!isSitting && FindObjectOfType<DialogueManager>().isUp == false)
        {
            transform.position += Vector3.right * Input.GetAxis("Horizontal") * speed;
            playerAnim.SetFloat("speed", Input.GetAxis("Horizontal"));
        }
    }
}