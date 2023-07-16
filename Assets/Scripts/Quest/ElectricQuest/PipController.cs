using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipController : MonoBehaviour
{
    public Transform mypip;
    private PipLocked pl;

    bool onPosition = false;
    bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
        pl = mypip.GetComponent<PipLocked>();
    }
    public bool PipIsDone() {
        return pl.Locked;
    }

    private void OnTriggerStay(Collider other)
    {
       // Debug.Log(other.tag + " name: " + other.name);
        if (other.tag == "PipQuest" && !onPosition)
        {
            if (other.transform.position == transform.position)
            {
                onPosition = true;
                Debug.Log("onposition");
            }
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;
            if (mypip == other.transform) {
                pl.Locked = true;
            }           
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.tag == "PipQuest" ){
            onPosition = false;
        }
    }

}
