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
    private float gravity = 10.0f ;
    public bool isGrounded = false;
    public float jumpForce = 0.1f;
    private float jumpLength = 1f;
    private float jumpStart;
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
            checkInput();
            upBody.checkGravity();
            downBody.checkGravity();
            upBody.applyMovement();
            downBody.applyMovement();
            if (Vector3.Distance(transform.position + (Vector3)movement, downBody.transform.position) < 1.0f)
            {
                PutTogether();
                return;
            }
        }
	}

    void checkGravity()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1.0f);
        if (hit.transform != null)
        {
            isGrounded = true;
            if(gravityVelocity < 0.0f)
                gravityVelocity = 0f;
        }
        else
            gravityVelocity -= Time.deltaTime;
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
        Debug.DrawRay(transform.position, transform.right * distX, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distX*0.5f);
        if (hit.transform != null)
        {
            movement.x = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }

        // CheckDirection Y
        float distY;
        if(movement.y>0)
            distY=1+movement.y;
        else
            distY=-1+movement.y;
        Debug.DrawRay(transform.position, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(transform.position, transform.up, distY);
        if (hit.transform != null)
        {
            movement.y = 0;

            if (movement.y < 0)
                isGrounded = true;

            transform.position = (Vector3)hit.point + (Vector3)hit.normal;
        }
        movement.y = gravityVelocity;   
        // on multiplie par la masse qui est de 2 pour le fullbody
        transform.position += (Vector3)movement;
        
            
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
