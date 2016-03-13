using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    [Header("Body Parts")]
    public GameObject upBody;
    public GameObject downBody;
    public GameObject[] fists = new GameObject[2];

    [Header("Movement variables")]
    public float speed;
    public float gravity;
    public bool isGrounded = false;
    public float jumpForce;


    // Private variables
    Vector2 movement = Vector2.zero;
    float gravityVelocity = 0.0f;
    float jumpImpulsion = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // reset movement variable
        movement = Vector2.zero;

        checkGravity();
        checkInput();
        checkCollision();
        applyMovement();
	}

    void checkGravity()
    {
        if (jumpImpulsion > 0f)
        {
            gravityVelocity += jumpForce * Time.deltaTime;
            jumpImpulsion -= jumpForce * Time.deltaTime;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.up,1.5f);
        Debug.DrawLine(transform.position, transform.position - transform.up * 1.5f);
        if(hit.transform != null)
        {
            Debug.Log(hit.transform.name);
            isGrounded = true;
            gravityVelocity = 0f;
        }
        else
        {
            gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime,2f);
            movement += (Vector2)transform.up * gravityVelocity;
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jump");
            jumpImpulsion = jumpForce;
        }
    }

    void applyMovement()
    {
        transform.position += (Vector3)movement;
    }

    void checkCollision()
    {
        Vector2 nextFramePosition = (Vector2)transform.position + movement;
        RaycastHit2D hit;
        // check down Collision
        if(!isGrounded)
        {
            hit = Physics2D.Raycast(nextFramePosition, -transform.up, 1.5f);
       
            if (hit.transform != null)
            {
                movement.y = (hit.point.y - nextFramePosition.y )+1.5f;
                isGrounded = true;
            }

        }
    }
}
