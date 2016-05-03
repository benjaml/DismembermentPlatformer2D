using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    public GameObject fistPosition;
    public List<GameObject> fists = new List<GameObject>();

    [Header("Movement variables")]
    private float speed = 7f;
    private float gravity = 2.0f;
    public bool isGrounded = false;
    private float jumpForce = 0.3f;
    private bool jumping;

    // Private variables
    public Vector2 movement = Vector2.zero;
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
        if (currentState == state.FullBody)
        {
            checkInput();
            checkGravity();
            applyMovement();
        }
        else
        {
            if (Vector3.Distance(upBody.transform.position, downBody.transform.position) < 1f)
            {
                PutTogether();
                return;
            }
            upBody.checkInput();
            upBody.checkGravity();
            downBody.checkGravity();
            upBody.applyMovement();
            downBody.applyMovement();

        }
	}

    void checkGravity()
    {
        Debug.DrawRay(downBody.transform.position, -transform.up*0.6f, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(downBody.transform.position, -transform.up, 0.6f);
        if (hit.transform != null)
        {
            Debug.Log("Grounded");
            isGrounded = true;
            if(gravityVelocity < 0.0f)
                gravityVelocity = 0f;
        }
        else
        {
            isGrounded = false;
            gravityVelocity -= Time.deltaTime*gravity;
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if(!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space) && isGrounded && currentState == state.FullBody)
        {
            Debug.Log("jump");
            isGrounded = false;
            jumping = true;
            gravityVelocity = jumpForce;
            appliedForce = true;

            // return pour qu'il ne fasse pas le 2eme if pour un saut normal en gounded
            return;

        }
        if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space) && upBody.isGrounded) || (Input.GetKeyDown(KeyCode.Space) && !isGrounded && currentState == state.FullBody))
        {
            Debug.Log("special jump");
            currentState = state.Separate;
            upBody.Jump();
            downBody.SetInertie(movement.x/Time.deltaTime);

        }
        if(Input.GetMouseButtonDown(0))
        {
            FireArm();
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null )
            {
                if (hit.transform.tag == "hand")
                {
                    Debug.Log("worked");

                    hit.transform.GetComponent<FistComponent>().Return(fistPosition.transform.position);
                }
                else if(hit.transform.name == "Button")
                {
                    hit.transform.GetComponent<ButtonScript>().Cancel();
                    hit.transform.GetChild(0).GetChild(0).GetComponent<FistComponent>().Return(fistPosition.transform.position);
                }
            }
        }
    }

    private void FireArm()
    {
        if (fists.Count == 0)
            return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 direction = mousePos - upBody.transform.position; 
        fists[0].GetComponent<FistComponent>().Fire(direction.normalized);
        fists.RemoveAt(0);
    }

    void applyMovement()
    {
        // CheckDirection X
        float distX;
        if(movement.x>0)
            distX=1+movement.x;
        else
            distX=-1+movement.x;
        Debug.DrawRay(downBody.transform.position + Vector3.up * 0.5f, transform.right * distX, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(downBody.transform.position+Vector3.up*0.5f, transform.right, distX * 0.5f);
        if (hit.transform != null)
        {
            movement.x = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }

        // CheckDirection Y
        float distY;
        if (gravityVelocity > 0)
            distY = 1 + gravityVelocity;
        else
            distY = -1 + gravityVelocity;
        Debug.DrawRay(downBody.transform.position + Vector3.up * 0.5f, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(downBody.transform.position+Vector3.up*0.5f, transform.up, distY);
        if (hit.transform != null)
        {
            movement.y = 0;

            //if (movement.y < 0)
              //  isGrounded = false;

            transform.position = (Vector3)hit.point + (Vector3)hit.normal;
        }
        movement.y = gravityVelocity;   
        // on multiplie par la masse qui est de 2 pour le fullbody
        transform.position += (Vector3)movement;
        
            
    }
    public void PutTogether()
    {
        Debug.Log("fusion");

        Vector3 upBodyPos = upBody.transform.position;
        Vector3 downBodyPos = downBody.transform.position;
        transform.position = downBodyPos + Vector3.up * 0.5f;
        downBody.transform.position = downBodyPos;
        upBody.transform.position = upBodyPos;


        downBody.StopMovement();
        upBody.transform.position = downBody.transform.position + Vector3.up;
        currentState = state.FullBody;
        gravityVelocity = 0.0f;
        upBody.isGrounded = true;
        upBody.launched = false;
        isGrounded = true;
    }

    
}
