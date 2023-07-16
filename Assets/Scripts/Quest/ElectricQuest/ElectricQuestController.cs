using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricQuestController : MonoBehaviour
{
    public PipController bluePip;
    public PipController redPip; 
    public bool ThisQuestIsComplete(){
        return bluePip.PipIsDone() && redPip.PipIsDone();
    }
}
