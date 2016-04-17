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
    private float speed = 7f;
    private float gravity = 10.0f ;
    public bool isGrounded = true;
    private float jumpForce = 0.1f;
    private float jumpLength = 1f;
    private float jumpStart;
    private bool jumping;

    // Private variables
    Vector2 movement = Vector2.zero;
    public float gravityVelocity = 0.0f;
    public float gravityOnlyVelocity = 0.0f;
    public float jumpImpulsion = 0f;
    private Vector3 vel;

    bool appliedForce = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // reset movement variable
        appliedForce = false;
        movement = Vector2.zero;

        checkGravity();
        checkInput();
        applyMovement();
	}

    void checkGravity()
    {
        if(currentState == state.FullBody)
        {
            RaycastHit2D hit = Physics2D.Raycast(downBody.transform.position, -transform.up, 0.5f);
            Debug.DrawLine(downBody.transform.position, downBody.transform.position - transform.up * 0.5f);
            if (hit.transform != null)
            {
                isGrounded = true;
                gravityVelocity = 0f;
                //downBody.transform.position = (Vector3)hit.point + Vector3.up * 0.5f;
                //upBody.transform.position = (Vector3)hit.point + Vector3.up * 1.5f;
            }

        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(upBody.transform.position, -transform.up, 0.5f);
            Debug.DrawLine(upBody.transform.position, upBody.transform.position - transform.up * 0.5f);
            if (hit.transform != null)
            {
                upBody.isGrounded = true;
                gravityVelocity = 0f;
                upBody.transform.position = (Vector3)hit.point + Vector3.up*0.5f;
            }
            hit = Physics2D.Raycast(downBody.transform.position, -transform.up, 0.5f);
            Debug.DrawLine(downBody.transform.position, downBody.transform.position - transform.up * 0.5f);
            if (hit.transform != null)
            {
                downBody.isGrounded = true;
                gravityOnlyVelocity = 0f;
                downBody.transform.position = (Vector3)hit.point + Vector3.up * 0.5f;
            }
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if(!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState == state.FullBody)
        {
            Debug.Log("jump");
            jumpImpulsion = jumpForce;
            isGrounded = false;
            jumpStart = Time.time;
            jumping = true;
            gravityVelocity = jumpForce;
            appliedForce = true;

            // return pour qu'il ne fasse pas le 2eme if pour un saut normal en gounded
            return;

        }
        if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space) && upBody.isGrounded) || (Input.GetKeyDown(KeyCode.Space) && !isGrounded && currentState == state.FullBody))
        {
            Debug.Log("special jump");
            jumpImpulsion = jumpForce;
            jumpStart = Time.time;
            currentState = state.Separate;
            gravityVelocity = jumpForce*2.0f;
            appliedForce = true;
            gravityOnlyVelocity = 0.0f;
            downBody.isGrounded = isGrounded;
            upBody.isGrounded = false;
            downBody.SetInertie(movement.x/Time.deltaTime);

        }
    }

    void applyMovement()
    {
        if (currentState == state.FullBody)
        {
            if (!isGrounded && !appliedForce)
            {
                gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime,2.0f);
            }
            movement.y = gravityVelocity;   
            RaycastHit2D hit = Physics2D.Raycast(transform.position+(Vector3)movement, -Vector3.up, 1.5f);
            if (hit.transform != null)
            {
                float impactY = hit.point.y;
                movement.y = impactY - transform.position.y + 1.5f;
            }
            // on multiplie par la masse qui est de 2 pour le fullbody
            movement.y *= 2;
            transform.position += (Vector3)movement;
        }
        else
        {
            Vector3 movementDown = Vector3.zero;
            // check gravity for the upBody
            if (!upBody.isGrounded && !appliedForce)
            {
                gravityVelocity -= Mathf.Pow(gravity* Time.deltaTime,2.0f);
            }
            // check gravity for the downBody
            if (!downBody.isGrounded && !appliedForce)
            {
                gravityOnlyVelocity -= Mathf.Pow(gravity * Time.deltaTime, 2.0f);
            }
            movement.y = gravityVelocity;
            movementDown.y = gravityOnlyVelocity;

            // check if there is ground 
            RaycastHit2D hit = Physics2D.Raycast(upBody.transform.position, -Vector3.up, 1f);
            if (hit && hit.transform.tag == "ground")
            {
                Debug.Log("up hit the ground");
                float impactY = hit.point.y;
                movement.y = impactY - upBody.transform.position.y + 1.5f;
                gravityVelocity = 0.0f;
                upBody.isGrounded = true;
            }

            hit = Physics2D.Raycast(downBody.transform.position, -Vector3.up, 1f);
            if (hit && hit.transform.tag == "ground")
            {
                Debug.Log("down hit the ground");
                float impactY = hit.point.y;
                movementDown.y = impactY - downBody.transform.position.y + 1.5f;
                downBody.isGrounded = true;
                gravityOnlyVelocity = 0.0f;
            }
            else if(Vector3.Distance(upBody.transform.position+(Vector3)movement,downBody.transform.position)<1.0f)
            {
                PutTogether();
                return;
            }
            if(appliedForce)
            {
                Debug.Log(movement.y);
            }
            upBody.transform.position += (Vector3)movement;
            movementDown.x = 0.0f;

            downBody.transform.position += movementDown; 
            downBody.enabled = true;

            
        }
    }
    public void PutTogether()
    {
        Debug.Log("fusion");
        downBody.StopMovement();
        upBody.transform.position = downBody.transform.position + Vector3.up;
        currentState = state.FullBody;
        gravityVelocity = 0.0f;
        upBody.isGrounded = true;
    }

    
}
