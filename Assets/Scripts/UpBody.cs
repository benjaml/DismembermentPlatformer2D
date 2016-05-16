using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpBody : MonoBehaviour {

    public bool launched = false;
    public bool isGrounded = true;
    public float gravity;
    public float jumpForce;
    private float speed = 7f;
    public Vector3 direction;
    public float velocityX;
    public float velocityY;
    public Vector2 movement;
    public float moydt;

    private GameObject fistPosition;
    private List<GameObject> fists;
    public bool atracted;
    public Vector3 attractedDirection;

    void Start()
    {
        fistPosition = transform.parent.GetComponent<PlayerManager>().fistPosition;
        fists = transform.parent.GetComponent<PlayerManager>().fists;
        atracted = false;
        attractedDirection = Vector3.zero;
    }
    public void checkInput()
    {
        movement = Vector2.zero;
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
        if (Input.GetMouseButtonDown(0))
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
    private void FireArm()
    {
        if (fists.Count == 0)
            return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 direction = mousePos - transform.position;
        fists[0].GetComponent<FistComponent>().Fire(direction.normalized);
        fists.RemoveAt(0);
        if (mousePos.x > transform.position.x)
            transform.localScale = Vector3.one;
        else if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("shoot");
    }
    private void Attract(Vector3 positionToGo)
    {
        atracted = true;
        attractedDirection = positionToGo - transform.position;
        attractedDirection.Normalize();
        movement = attractedDirection * speed * Time.deltaTime;

    }

    public void checkGravity()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.5f);
        Debug.DrawLine(transform.position, transform.position - transform.up * 0.5f);
        if (hit.transform != null)
        {
            isGrounded = true;
            gravityVelocity = 0f;
            transform.position = (Vector3)hit.point + Vector3.up*0.5f;
        }
        else
        {
            isGrounded = false;
            gravityVelocity -= Time.deltaTime;
        }
            
    }
   /* public void applyMovement()
    {
        if(atracted)
        {
            movement = attractedDirection * speed * Time.deltaTime;
            if (!IsStillAttracted())
            {

                atracted = false;
                return;
            }
        }

        // CheckDirection X
        float distX;
        if (movement.x > 0)
            distX = 1 + movement.x;
        else
            distX = -1 + movement.x;
        Debug.DrawRay(transform.position, transform.right * distX, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distX * 0.5f);
        if (hit.transform != null)
        {
            movement.x = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }

        // CheckDirection Y
        float distY;
        if (gravityVelocity > 0)
            distY = 0.5f + gravityVelocity;
        else
            distY = -0.5f + gravityVelocity;
        Debug.DrawRay(transform.position, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(transform.position, transform.up, distY);
        if (hit.transform != null)
        {
            movement.y = 0;

            if (gravityVelocity < 0)
              isGrounded = true;

            transform.position = (Vector3)hit.point + (Vector3)hit.normal*0.5f;
        }
        movement.y = gravityVelocity;
        // on multiplie par la masse qui est de 2 pour le fullbody
        transform.position += (Vector3)movement;

            
    }*/
    public void applyMovement()
    {
        if (atracted)
        {
            movement = attractedDirection * speed * Time.deltaTime;
            if (!IsStillAttracted())
            {
                gravityVelocity = 0;
                atracted = false;
                return;
            }
        }
        // CheckDirection X
        float distX;
        if (movement.x > 0)
            distX = 0.5f + movement.x;
        else
            distX = -0.5f + movement.x;
        Debug.DrawRay(transform.position, transform.right * distX, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distX * 0.5f);
        if (hit.transform != null)
        {
            movement.x = 0;
            if (atracted)
                attractedDirection = Vector3.zero;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }

        // CheckDirection Y
        float distY;
        if (!atracted)
        {
            if (gravityVelocity > 0)
                distY = 0.5f + gravityVelocity;
            else
                distY = -0.5f + gravityVelocity;
        }
        else
        {
            if (movement.y > 0)
                distY = 0.5f + movement.y;
            else
                distY = -0.5f + movement.y;
        }
        Debug.DrawRay(transform.position, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(transform.position, transform.up, distY);
        if (hit.transform != null)
        {
            movement.y = 0;
            if (atracted)
                attractedDirection = Vector3.zero;

            transform.position = (Vector3)hit.point + (Vector3)hit.normal*0.5f;
        }
        if (!atracted)
            movement.y = gravityVelocity;
        // on multiplie par la masse qui est de 2 pour le fullbody
        transform.position += (Vector3)movement;
        if (movement.x != 0 && isGrounded)
        {
            if (movement.x > 0f)
                transform.localScale = Vector3.one;
            else if (movement.x < 0f)
                transform.localScale = new Vector3(-transform.parent.localScale.x, 1f, 1f);
            transform.GetChild(0).GetComponent<Animator>().SetBool("move", true);
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("move", false);
        }

    }

    public void Jump()
    {
        gravityVelocity = jumpForce*moydt;
        launched = true;
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("jump");
    }

    bool IsStillAttracted()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
    public float gravityVelocity { get; set; }

    public bool appliedForce { get; set; }
}
