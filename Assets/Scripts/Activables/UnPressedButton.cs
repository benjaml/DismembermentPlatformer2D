using UnityEngine;
using System.Collections;
using System;

public class UnPressedButton : AbstractActivable
{
    public override void Activate()
    {
        GetComponent<Animator>().SetTrigger("unpress");
    }
}
