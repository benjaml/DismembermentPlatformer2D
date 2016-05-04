using UnityEngine;
using System.Collections;
using System;

public class UnPressedButton : AbstractActivable
{
    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("unpress");
        if (transform.GetChild(0).GetChild(0).tag == "hand")
        {
            transform.GetChild(0).GetChild(0).position -= Vector3.up*0.15f;
            transform.GetChild(0).GetChild(0).GetComponent<FistComponent>().enableGravity = true;
        }
    }
}
