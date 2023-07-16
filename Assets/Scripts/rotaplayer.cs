using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotaplayer : MonoBehaviour
{
    public PlayerMovement pm;
    // Start is called before the first frame update

    public bool yRot;
     float timer = 0.25f;
    //pm.Playedwalk1
    int counter = 0;
    float y = 0.25f;
    bool once = false;
    void rotateMe()
    {
        if (counter % Mathf.Round(timer / Time.fixedDeltaTime) == 0)
        {
            if (!once) {
                timer = timer * 3;
                once = true;
            }
            y = y * -1;
        }
        transform.Rotate(0.0f, y, 0.0f);
    }
    private void FixedUpdate()
    {
        if (this.name != "ActiveNpc")
        {
            counter++;
            rotateMe();
        }
    }

}
