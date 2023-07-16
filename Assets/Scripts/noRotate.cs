using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noRotate : MonoBehaviour
{
    Transform startRot;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = startRot.rotation;
    }
}
