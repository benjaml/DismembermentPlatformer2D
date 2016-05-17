using UnityEngine;
using System.Collections;
using System;

public class FistComponent : MonoBehaviour {

    public float velocityX;
    public float velocityY;
    public float force;
    public float gravity;
    public float friction;
    public GameObject fistPosition;
    public GameObject player;
    public bool fired = false;
    public bool returnBool = false;
    public bool enableGravity = false;
    
    void Start()
    {
        fistPosition = GameObject.FindGameObjectWithTag("up").transform.GetChild(0).GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if(!returnBool && fired)
        {
            velocityX *= 1-Time.deltaTime;
            velocityY -= Mathf.Pow(gravity, 2) * Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            CheckCollision();
        }
        else if(returnBool)
        {
            velocityX *= 1 + Time.deltaTime;
            velocityY *= 1 + Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            if(Vector3.Distance(transform.position, fistPosition.transform.position) <1f)
            {

                transform.position = fistPosition.transform.position;
                transform.parent = fistPosition.transform;
                player.GetComponent<PlayerManager>().fists.Add(gameObject);
                returnBool = false;
                gameObject.layer = 0;
                GetComponent<BoxCollider2D>().enabled = false;
                return;

            }
            CheckCollisionReturn();
        }
        if(enableGravity)
        {
            velocityY -= Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            CheckCollisionGravityOnly();
        }
	}

    private void CheckCollision()
    {
        // CheckDirection X
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, 0.9f * velocityX * Time.deltaTime);
        if (hit.transform != null && hit.transform != transform)
        {
            fired = false;
            velocityX = 0;
            gameObject.layer = 0;
            transform.position = (Vector3)hit.point+(Vector3)hit.normal*0.15f;
        }

        // CheckDirection Y
        hit = Physics2D.Raycast(transform.position, Vector3.up, 0.9f * velocityY * Time.deltaTime);
        if (hit.transform != null && hit.transform != transform)
        {
            velocityY = 0;
            gameObject.layer = 0;
            fired = false;
            transform.position = (Vector3)hit.point - Vector3.up * velocityY;
        }
    }

    private void CheckCollisionReturn()
    {
        // CheckDirection X
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, velocityX * Time.deltaTime);
        if (hit.transform != null && hit.transform != transform /*&& hit.transform.tag != "up"*/)
        {
            returnBool = false;
            gameObject.layer = 0;
            velocityX = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }

        // CheckDirection Y
        hit = Physics2D.Raycast(transform.position, transform.up, velocityY * Time.deltaTime);
        if (hit.transform != null && hit.transform != transform)
        {
            if (velocityY < 0)
            {
                returnBool = false;
                gameObject.layer = 0;
            }
            velocityY = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }
    }
    private void CheckCollisionGravityOnly()
    {
       
        // CheckDirection Y
        Debug.DrawRay(transform.position-Vector3.up*0.5f, Vector3.up * velocityY,Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position - Vector3.up * 0.5f, Vector3.up, velocityY);
        if (hit.transform != null && hit.transform != transform)
        {
            if (velocityY < 0)
                enableGravity = false;
            velocityY = 0;
            transform.position = (Vector3)(hit.point + hit.normal * 0.15f);
        }
        transform.position += Vector3.up*velocityY;
    }

    public void Return(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction *= force;
        velocityX = direction.x/2f;
        velocityY = direction.y / 2f;
        returnBool = true;
        enableGravity = false;
        gameObject.layer = 2;
        Debug.DrawLine(transform.position, transform.position + direction * 5f, Color.red, 10f);
    }

    public void Fire(Vector3 direction)
    {
        direction *= force;
        velocityX = direction.x;
        velocityY = direction.y;
        fired = true;
        enableGravity = false;
        transform.parent = null;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        gameObject.layer = 2;
        GetComponent<BoxCollider2D>().enabled = true;

    }
}
