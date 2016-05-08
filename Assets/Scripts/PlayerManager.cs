using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

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
    public bool atracted;

    [Header("Movement variables")]
    private float speed = 7f;
    private float gravity = 2.0f;
    public bool isGrounded = false;
    private float jumpForce = 0.25f;
    private bool jumping;

    // Private variables
    public Vector2 movement = Vector2.zero;
    public float gravityVelocity = 0.0f;
    public float gravityOnlyVelocity = 0.0f;
    public float jumpImpulsion = 0f;
    private Vector3 vel;
    private Vector3 attractedDirection = Vector3.zero;

    bool appliedForce = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.P))
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        if(atracted)
        {

            // applymovemnt and check collision
            movement = attractedDirection * speed * Time.deltaTime;
            checkInput();
            applyMovement();
            //check is still cliking
            if (!IsStillAttracted())
            {
                atracted = false;
            }
            return;
        }
        else
        {
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

    bool IsStillAttracted()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    void checkInput()
    {
        if(atracted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireArm();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.transform.tag == "hand")
                    {
                        Debug.Log("worked");

                        hit.transform.GetComponent<FistComponent>().Return(fistPosition.transform.position);
                    }
                    else if (hit.transform.name == "Button" && hit.transform.GetChild(0).GetChild(0) != null)
                    {
                        hit.transform.GetComponent<ButtonScript>().Cancel();
                    }
                }
            }
        }
        else
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
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        if (hit.transform.tag == "hand")
                        {
                            Debug.Log("worked");

                            hit.transform.GetComponent<FistComponent>().Return(fistPosition.transform.position);
                        }
                        else if (hit.transform.name == "Button" && hit.transform.GetChild(0).GetChild(0) != null)
                        {
                            hit.transform.GetComponent<ButtonScript>().Cancel();
                        }

                    }
                    else
                    {
                        if (hit.transform.tag == "hand")
                        {
                            Attract(hit.point);
                        }
                        else if (hit.transform.name == "Button" && hit.transform.GetChild(0).GetChild(0) != null)
                        {
                            Attract(hit.point);
                        }
                    }
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
    private void Attract(Vector3 positionToGo)
    {
        atracted = true;
        attractedDirection = positionToGo - transform.position;
        attractedDirection.Normalize();

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
            if (atracted)
                attractedDirection = Vector3.zero;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }

        // CheckDirection Y
        float distY;
        if(!atracted)
        {
            if (gravityVelocity > 0)
                distY = 1 + gravityVelocity;
            else
                distY = -1 + gravityVelocity;
        }
        else
        {
            if (movement.y > 0)
                distY = 1 + movement.y;
            else
                distY = -1 + movement.y;
        }
        Debug.DrawRay(downBody.transform.position + Vector3.up * 0.5f, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(downBody.transform.position+Vector3.up*0.5f, transform.up, distY);
        if (hit.transform != null)
        {
            movement.y = 0;
            if (atracted)
                attractedDirection = Vector3.zero;

            transform.position = (Vector3)hit.point + (Vector3)hit.normal;
        }
        if(!atracted)
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
