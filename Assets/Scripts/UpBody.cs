using UnityEngine;
using System.Collections;

public class UpBody : MonoBehaviour {

    public bool launched = false;
    public bool isGrounded = true;
    public float gravity;
    public float jumpForce;
    public float speed;
    public Vector3 direction;
    public float velocityX;
    public float velocityY;
    private Vector2 movement;



    void Update()
    {
        if (!launched )
        {
            return;
        }
        else 
        {
            velocityX *= 1 - Time.deltaTime;
            velocityY -= Mathf.Pow(gravity, 2) * Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            CheckCollision();
        }
    }


    private void CheckCollision()
    {
        // CheckDirection X
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, velocityX * Time.deltaTime);
        if (hit.transform != null)
        {
            velocityX = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }

        // CheckDirection Y
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
            
    }
    public void applyMovement()
    {
        // check gravity for the upBody
        if (!isGrounded && !appliedForce)
        {
            gravityVelocity -= Mathf.Pow(gravity* Time.deltaTime,2.0f);
        }
        movement.y = gravityVelocity;

        // check if there is ground 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 1f);
        if (hit && hit.transform.tag == "ground")
        {
            Debug.Log("up hit the ground");
            float impactY = hit.point.y;
            movement.y = impactY - transform.position.y + 1.5f;
            gravityVelocity = 0.0f;
            isGrounded = true;
        }
        hit = Physics2D.Raycast(transform.position, Vector3.up, 1f);
        if (hit && hit.transform.tag == "ground")
        {
            Debug.Log("up hit the roof");
            gravityVelocity = 0.0f;
        }
        if(appliedForce)
        {
            Debug.Log(movement.y);
        }
        transform.position += (Vector3)movement;

            
    }

    public void Jump()
    {
        velocityY = jumpForce;
        launched = true;

    }


    public float gravityVelocity { get; set; }

    public bool appliedForce { get; set; }
}
