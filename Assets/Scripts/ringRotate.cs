using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ringRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool yRot;
    public float timer = 10;
    int counter = 0;
    float y = -1f;
    void rotateMe(){
        if (counter % Mathf.Round(timer / Time.fixedDeltaTime) == 0)
        {
            y = y * -1;
        }
        if (!yRot){
            transform.Rotate(y, y, y);
        }else {

            transform.Rotate(0.0f, y, 0.0f);
        }
    }
    private void FixedUpdate(){
        if (this.name != "ActiveNpc")
        {
            counter++;
            rotateMe();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
