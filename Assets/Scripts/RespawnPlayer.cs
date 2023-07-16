using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public List<Transform> pos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.LogWarning(" respawn ");
            if (Vector3.Distance(player.position,pos[0].position)< Vector3.Distance(player.position, pos[1].position) &&
                Vector3.Distance(player.position, pos[0].position) < Vector3.Distance(player.position, pos[2].position) ) {
                player.position = pos[0].position;
            }else
            if (Vector3.Distance(player.position, pos[1].position) < Vector3.Distance(player.position, pos[0].position) &&
                Vector3.Distance(player.position, pos[1].position) < Vector3.Distance(player.position, pos[2].position)){
                player.position = pos[1].position;
            }else
            if (Vector3.Distance(player.position, pos[2].position) < Vector3.Distance(player.position, pos[1].position) &&
                Vector3.Distance(player.position, pos[2].position) < Vector3.Distance(player.position, pos[0].position)){
                player.position = pos[2].position;
            }

        }
    }

}
