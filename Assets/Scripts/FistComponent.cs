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
    
    void Start()
    {
        fistPosition = GameObject.FindGameObjectWithTag("up").transform.GetChild(1).gameObject;
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

            }
            CheckCollisionReturn();
        }
	}

    private void CheckCollision()
    {
        // CheckDirection X
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, velocityX*Time.deltaTime);
        if (hit.transform != null)
        {
            fired = false;
            velocityX = 0;
            gameObject.layer = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }

        // CheckDirection Y
        hit = Physics2D.Raycast(transform.position, transform.up, velocityY * Time.deltaTime);
        if (hit.transform != null)
        {
            if (velocityY < 0)
                fired = false;
            velocityY = 0;
            gameObject.layer = 0;
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }
    }

    private void CheckCollisionReturn()
    {
        // CheckDirection X
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, velocityX * Time.deltaTime);
        if (hit.transform != null && hit.transform != transform)
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

    public void Return(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction *= force;
        velocityX = direction.x;
        velocityY = direction.y;
        returnBool = true;
        gameObject.layer = 2;
        Debug.DrawLine(transform.position, transform.position + direction * 5f, Color.red, 10f);
    }

    public void Fire(Vector3 direction)
    {
        direction *= force;
        velocityX = direction.x;
        velocityY = direction.y;
        fired = true;
        transform.parent = null;

    }
}
