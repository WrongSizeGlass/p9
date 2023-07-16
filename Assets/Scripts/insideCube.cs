using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insideCube : MonoBehaviour
{
    // Start is called before the first frame update
    public bool empty = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            empty = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            empty = true;
        }
    }

}
