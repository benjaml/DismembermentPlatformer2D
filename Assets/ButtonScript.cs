using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonScript : MonoBehaviour {

    public List<AbstractActivable> tasks = new List<AbstractActivable>();
    public List<AbstractActivable> cancelTasks = new List<AbstractActivable>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Activate();
        }
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "hand" && !col.GetComponent<FistComponent>().enableGravity)
        {
            col.transform.parent = transform.GetChild(0).transform;
            Activate();
        }
    }

    void Activate()
    {
        foreach(AbstractActivable activable in  tasks)
        {
            activable.Activate();
        }
    }
    public void Cancel()
    {
        foreach (AbstractActivable activable in cancelTasks)
        {
            activable.Activate();
        }
    }
}
