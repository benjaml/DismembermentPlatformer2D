using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public enum state  
    {
        FullBody,
        Separate,
    }

    public state currentState = state.FullBody;
    [Header("Body Parts")]
    public UpBody upBody;
    public DownBody downBody;
    public GameObject[] fists = new GameObject[2];

    [Header("Movement variables")]
    public float speed;
    public float gravity;
    public bool isGrounded = false;
    public float jumpForce;
    public float jumpLength = 1f;
    private float jumpStart;
    private bool jumping;

    // Private variables
    Vector2 movement = Vector2.zero;
    public float gravityVelocity = 0.0f;
    public float jumpImpulsion = 0f;
    private Vector3 vel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // reset movement variable
        movement = Vector2.zero;

        checkGravity();
        checkInput();
        applyMovement();
	}

    void checkGravity()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.up,1.5f);
        Debug.DrawLine(transform.position, transform.position - transform.up * 1.5f);
        if(hit.transform != null)
        {
            Debug.Log(hit.transform.name);
            isGrounded = true;
            gravityVelocity = 0f;
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if(!Input.GetKeyDown(KeyCode.LeftShift)  && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jump");
            jumpImpulsion = jumpForce;
            isGrounded = false;
            jumpStart = Time.time;
            jumping = true;
            gravityVelocity += jumpForce*Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jumpSpecial");
            jumpImpulsion = jumpForce;
            isGrounded = false;
            jumping = true;
            jumpStart = Time.time;
            currentState = state.Separate;
            downBody.SetInertie(movement.x);

        }
    }

    void applyMovement()
    {
        if (currentState == state.FullBody)
        {
            if (!isGrounded)
            {
                gravityVelocity -= gravity * Time.deltaTime;
            }
            movement.y = gravityVelocity;   
            RaycastHit2D hit = Physics2D.Raycast(transform.position+(Vector3)movement, -Vector3.up, 1.5f);
            if (hit.transform != null)
            {
                float impactY = hit.point.y;
                movement.y = impactY - transform.position.y + 1.5f;
            }
            transform.position += (Vector3)movement;
        }
        else
        {
            if (!isGrounded)
            {
                gravityVelocity -= (gravity/2f) * Time.deltaTime;
            }
            movement.y = gravityVelocity;
            RaycastHit2D hit = Physics2D.Raycast(upBody.transform.position, -Vector3.up, 1f);
            if (hit && hit.transform.tag == "down")
            {
                Debug.Log("Fusion !"); 
                upBody.transform.position = hit.transform.position + Vector3.up;
                currentState = state.FullBody;
                downBody.StopMovement();
                return;
            }
            upBody.transform.position += (Vector3)movement;
            downBody.enabled = true;
            
        }
    }


}
