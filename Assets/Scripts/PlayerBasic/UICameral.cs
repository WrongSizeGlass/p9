using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameral : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.depth = 10;
        Debug.Log(cam.depth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
