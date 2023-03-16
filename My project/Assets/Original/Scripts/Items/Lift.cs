using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Vector2 heights;
    public bool canMove = false;
    public bool moveUp = true;
    public bool isMoving = false;
    public bool onTop = false;
    public bool onBottom = true;
    private AudioSource movingAudio;

    // Start is called before the first frame update
    void Start()
    {
        movingAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (moveUp)
            {
                onTop = false;
                onBottom = false;
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(transform.position.x, heights.x, 0), 3 * Time.deltaTime);
                if (transform.position.y >= heights.x)
                {
                    isMoving = false;
                    canMove = false;
                    moveUp = false;
                    onTop = true;
                }
            } else if (!moveUp)
            {
                onTop = false;
                onBottom = false;
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(transform.position.x, heights.y, 0), 3 * Time.deltaTime);
                if(transform.position.y <= heights.y)
                {
                    isMoving = false;
                    canMove = false;
                    moveUp = true;
                    onBottom = true;
                }
            }
        }

        if(isMoving == true)
        {
            movingAudio.volume = 0.3f;
        } else
        {
            movingAudio.volume = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "player")
        {
            isMoving = true;
            canMove = true;

            collision.gameObject.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {

            collision.gameObject.transform.parent = FindObjectOfType<GameManager>().transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.75f, 1, 0.5f);
        Gizmos.DrawCube(new Vector3(this.transform.position.x, heights.x, 0), new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(this.transform.position.x, heights.y, 0), new Vector3(1, 1, 1));
    }
}
