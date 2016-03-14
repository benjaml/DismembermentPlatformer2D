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

        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("Lol");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jumpSpecial");
            jumpImpulsion = jumpForce;
            isGrounded = false;
            jumpStart = Time.time;
            currentState = state.Separate;

        }
    }

    void applyMovement()
    {
        if (currentState == state.FullBody)
        {
            if (!isGrounded)
            {
                gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime, 2f);
            }
            movement.y += gravityVelocity;
            if ((Time.time - jumpStart) < jumpLength)
            {
                movement.y += jumpForce * Time.deltaTime;
            }
            transform.position += (Vector3)movement;
        }
        else
        {
            if (!isGrounded)
            {
                gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime, 2f);
            }
            movement.y += gravityVelocity;
            if ((Time.time - jumpStart) < jumpLength)
            {
                movement.y += jumpForce * Time.deltaTime;
            }
            upBody.transform.position += (Vector3)movement;
            downBody.enabled = true;
            downBody.SetInertie(movement.x);
        }
    }


}
