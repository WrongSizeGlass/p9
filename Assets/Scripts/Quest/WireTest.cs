using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTest : MonoBehaviour
{
    public GameObject cubeBottom;
    public GameObject cubeTop;

    // Start is called before the first frame update
    void Start()
    {
        

        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, cubeBottom.transform.localPosition);
        lineRenderer.SetPosition(1, cubeTop.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
