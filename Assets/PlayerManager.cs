using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    [Header("Body Parts")]
    public GameObject upBody;
    public GameObject downBody;
    public GameObject[] fists = new GameObject[2];

    [Header("Movement variables")]
    public float speed;
    public float gravity;
    public bool isGrounded = false;


    // Private variables
    Vector2 movement = Vector2.zero;
    float gravityVelocity = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // reset movement variable
        movement = Vector2.zero;

        checkGravity();
        checkInput();
        checkCollision();
        applyMovement();
	}

    void checkGravity()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.up,1.5f);
        Debug.DrawLine(transform.position, transform.position - transform.up * 1.5f);
        if(hit.transform != null)
        {
            Debug.Log(hit.transform.name);
            isGrounded = true;
            gravityVelocity = 0f;
        }
        else
        {
            isGrounded = false;
            gravityVelocity += Mathf.Pow(gravity * Time.deltaTime,2f);
            movement += (Vector2)transform.up * -gravityVelocity;
        }
    }
    void checkInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
    }

    void applyMovement()
    {
        transform.position += (Vector3)movement;
    }

    void checkCollision()
    {
        Vector2 nextFramePosition = (Vector2)transform.position + movement; 
        // check down Collision

    }
}
