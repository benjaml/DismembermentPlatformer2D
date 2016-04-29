using UnityEngine;
using System.Collections;
using System;

public class FistComponent : MonoBehaviour {

    public float velocityX;
    public float velocityY;
    public float force;
    public float gravity;
    public float friction;
    private bool fired = false;
    private bool returnBool = false;
    
	
	// Update is called once per frame
	void Update () {
        if (!fired)
            return;
        if(!returnBool)
        {
            velocityX *= 1-Time.deltaTime;
            velocityY -= Mathf.Pow(gravity, 2) * Time.deltaTime;
            transform.position += new Vector3(velocityX, velocityY, 0.0f) * Time.deltaTime;
            CheckCollision();
        }
        else
        {
            
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
            transform.position = (Vector3)hit.point + (Vector3)hit.normal * 0.15f;
        }

        // CheckDirection Y
        hit = Physics2D.Raycast(transform.position, transform.up, velocityY * Time.deltaTime);
        if (hit.transform != null)
        {
            Debug.Log("lol");
            if (velocityY < 0)
                fired = false;
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
        //TODO: Remettre le fist dans le joueur
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
