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
    public float gravityOnlyVelocity = 0.0f;
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
            isGrounded = true;
            gravityVelocity = 0f;
            //TODO: Faire plusieurs controller pour chaque partie du corp
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if(!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jump");
            jumpImpulsion = jumpForce;
            isGrounded = false;
            jumpStart = Time.time;
            jumping = true;
            gravityVelocity = jumpForce;
            // return pour qu'il ne fasse pas le 2eme if pour un saut normal en gounded
            return;

        }
        if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Space) && !isGrounded && currentState == state.FullBody))
        {
            Debug.Log("special jump");
            jumpImpulsion = jumpForce;
            jumping = true;
            jumpStart = Time.time;
            currentState = state.Separate;
            gravityVelocity = jumpForce;
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
            Vector3 movementDown = Vector3.zero;
            // check gravity for the upBody
            if (!upBody.isGrounded)
            {
                gravityVelocity -= (gravity/2.0f) * Time.deltaTime;
            }
            // check gravity for the downBody
            if (!downBody.isGrounded)
            {
                gravityOnlyVelocity -= gravity * Time.deltaTime;
            }
            movement.y = gravityVelocity;
            movementDown.y = gravityOnlyVelocity;

            // check if there is ground 
            RaycastHit2D hit = Physics2D.Raycast(upBody.transform.position, -Vector3.up, 1.5f);
            if (hit && hit.transform.tag == "ground")
            {
                Debug.Log("up hit the ground");
                float impactY = hit.point.y;
                movement.y = impactY - upBody.transform.position.y + 1.5f;
                gravityVelocity = 0.0f;
                upBody.isGrounded = true;
            }

            hit = Physics2D.Raycast(downBody.transform.position, -Vector3.up, 1.5f);
            if (hit && hit.transform.tag == "ground")
            {
                Debug.Log("down hit the ground");
                float impactY = hit.point.y;
                movementDown.y = impactY - downBody.transform.position.y + 1.5f;
                downBody.isGrounded = true;
                gravityOnlyVelocity = 0.0f;
            }
            else if(Vector3.Distance(upBody.transform.position,downBody.transform.position)<1.0f)
            {

                downBody.StopMovement();
                upBody.transform.position = downBody.transform.position + Vector3.up;
                currentState = state.FullBody;
                gravityVelocity = 0.0f;
                return;
            }
            upBody.transform.position += (Vector3)movement;
            movementDown.x = 0.0f;

            downBody.transform.position += movementDown; 
            downBody.enabled = true;

            //TODO: rajouter la gravité sur les jambes reste un bug quand on est en l'air
            //TODO: Bug de décalage quand le haut touche le sol
            //TODO: Bug le perso s'envole ? quand on saute spécial
            
        }
    }


}
