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
        /*if(moveAlone)
            transform.position += new Vector3(inertie*Time.deltaTime, 0.0f, 0.0f);
         */
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
        if (hit.transform != null && hit.transform.tag != "movable")
        {
            isGrounded = true;
            gravityVelocity = 0f;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }
        else
        {
            isGrounded = false;
            gravityVelocity -= Time.deltaTime;
        }
    }

    public void applyMovement()
    {
        // TODO : check right and left collision
        Vector3 movementDown = Vector3.zero;
        movementDown.x = inertie*Time.deltaTime;
        float distX;
        if (movementDown.x > 0)
            distX = 1 + movementDown.x;
        else
            distX = -1 + movementDown.x;
        Debug.DrawRay(transform.position, transform.right * distX, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distX * 0.5f);
        if (hit.transform != null )
        {
            movementDown.x = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.5f;
        }
        // check gravity for the downBody
        if (!isGrounded && !appliedForce)
        {
            gravityVelocity -= Mathf.Pow(gravity * Time.deltaTime, 2.0f);
        }
        movementDown.y = gravityVelocity;

        // check if there is ground 
        Debug.DrawRay(transform.position, -Vector3.up, Color.blue);
        hit = Physics2D.Raycast(transform.position, -Vector3.up, 1f);
        if (hit && hit.transform.tag == "ground" )
        {
            float impactY = hit.point.y;
            movementDown.y = impactY - transform.position.y + 1.5f;
            isGrounded = true;
            gravityVelocity = 0.0f;
        }

        transform.position += movementDown;
        if (movementDown.x != 0 && isGrounded || inertie != 0f)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("move", true);
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("move", false);
        }
        if (movementDown.x > 0f)
            transform.localScale = Vector3.one;
        else if (movementDown.x < 0f)
            transform.localScale = new Vector3(-transform.parent.localScale.x, 1f, 1f);
        enabled = true;

            
    }
}
