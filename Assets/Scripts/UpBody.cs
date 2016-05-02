using UnityEngine;
using System.Collections;

public class UpBody : MonoBehaviour {

    public bool launched = false;
    public bool isGrounded = true;
    public float gravity;
    public float jumpForce;
    private float speed = 7f;
    public Vector3 direction;
    public float velocityX;
    public float velocityY;
    private Vector2 movement;



    void Update()
    {
        /*if (!launched )
        {
            return;
        }
        else 
        {
            velocityX *= 1 - Time.deltaTime;
            velocityY -= Mathf.Pow(gravity, 2) * Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            CheckCollision();
        }*/
    }


    private void CheckCollision()
    {
        Debug.Log("CheckCollision");
        // CheckDirection X
        Debug.DrawRay(transform.position, transform.right * velocityX * Time.deltaTime, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, velocityX * Time.deltaTime);
        if (hit.transform != null)
        {
            velocityX = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }

        // CheckDirection Y
        Debug.DrawRay(transform.position, transform.up * velocityY * Time.deltaTime, Color.blue);
        hit = Physics2D.Raycast(transform.position, transform.up, velocityY * Time.deltaTime);
        if (hit.transform != null)
        {
            if (velocityY < 0)
                isGrounded = true;
            velocityY = 0;
            gameObject.layer = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }
    }

    public void checkInput()
    {
        movement = Vector2.zero;
        float h = Input.GetAxisRaw("Horizontal");
        movement += (Vector2)transform.right * h * Time.deltaTime * speed;
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
    public void applyMovement()
    {
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
            distY = 1 + gravityVelocity;
        else
            distY = -1 + gravityVelocity;
        Debug.DrawRay(transform.position, transform.up * distY, Color.blue);
        hit = Physics2D.Raycast(transform.position, transform.up, distY);
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

    public void Jump()
    {
        gravityVelocity = jumpForce;
        launched = true;

    }


    public float gravityVelocity { get; set; }

    public bool appliedForce { get; set; }
}
