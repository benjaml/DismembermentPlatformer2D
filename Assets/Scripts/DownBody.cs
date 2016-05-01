using UnityEngine;
using System.Collections;

public class DownBody : MonoBehaviour {

    public float inertie;
    public bool moveAlone = false;
    public bool isGrounded = false;
    public float gravity;
    private float gravityVelocity;
    private bool appliedForce;

	
	// Update is called once per frame
	void Update () {
        if(moveAlone)
            transform.position += new Vector3(inertie*Time.deltaTime, 0.0f, 0.0f);
	}

    public void SetInertie(float speed)
    {
        inertie = speed;
        moveAlone = true;
    }
    public void StopMovement()
    {
        inertie = 0;
        moveAlone = false;
    }
    public void checkGravity()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.5f);
        Debug.DrawLine(transform.position, transform.position - transform.up * 0.5f);
        if (hit.transform != null)
        {
            isGrounded = true;
            gravityVelocity = 0f;
            transform.position = (Vector3)hit.point + Vector3.up * 0.5f;
        }
    }

    public void applyMovement()
    {
        Vector3 movementDown = Vector3.zero;
        // check gravity for the downBody
        if (!isGrounded && !appliedForce)
        {
            gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime, 2.0f);
        }
        movementDown.y = gravityVelocity;

        // check if there is ground 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 1f);
        if (hit && hit.transform.tag == "ground")
        {
            Debug.Log("down hit the ground");
            float impactY = hit.point.y;
            movementDown.y = impactY - transform.position.y + 1.5f;
            isGrounded = true;
            gravityVelocity = 0.0f;
        }
        else 
        movementDown.x = 0.0f;

        transform.position += movementDown; 
        enabled = true;

            
    }
}
