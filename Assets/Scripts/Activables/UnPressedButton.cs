using UnityEngine;
using System.Collections;
using System;

public class UnPressedButton : AbstractActivable
{
    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("unpress");
        if (transform.GetChild(0).GetChild(0).GetChild(0).tag == "hand")
        {

            Invoke("release", 0.2f);
        }
    }
    
    void release()
    {
        GameObject tmp = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        tmp.GetComponent<FistComponent>().enableGravity = true;
        tmp.GetComponent<FistComponent>().velocityY = 0;
        tmp.transform.parent = null;
    }
}
