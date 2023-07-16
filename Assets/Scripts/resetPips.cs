using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPips : MonoBehaviour
{
    public Transform redpip;
    public Transform bluepip;

    private PipLocked redLock;
    private PipLocked blueLock;
    private Vector3 redPipPos;
    private Vector3 bluePipPos;
    // Start is called before the first frame update
    void Start()
    {
        redPipPos = redpip.position;
        bluePipPos = bluepip.position;
        redLock = redpip.GetComponent<PipLocked>();
        blueLock = bluepip.GetComponent<PipLocked>();
    }
    public void resetPip() {
        redLock.Locked = false;
        blueLock.Locked = false;
        redpip.position = redPipPos;
        bluepip.position = bluePipPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
