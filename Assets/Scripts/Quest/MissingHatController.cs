using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingHatController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Player;
    // public Transform QuestGiver;
    
    PlayerInteract PI;
    NPCDialog ND;

    BiasFoundation playerBias;
    // 0 = original hat, 1 = triangel player hat, 2 = cube player hat, 3 = QuestGiver: MissingHat, 4 = QuestGiver: RewardHat
    public List<MeshRenderer> showMissingHat;
    int playerHat = 1;
    GoodWillSystem gws;
    void Start()
    {
        PI = Player.GetComponent<PlayerInteract>();
        playerBias = Player.GetComponent<BiasFoundation>();
        ND = GetComponent<NPCDialog>();
        gws = GetComponent<GoodWillSystem>();
        if (playerBias.FirstPlaythrough){
            playerHat = 1;
        } else {
            playerHat = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PI.MissingHatFound && !showMissingHat[playerHat].enabled && !gws.questCompleted) {
            showMissingHat[playerHat].enabled = true;
            Debug.LogError(" turn on player hat");
        }
        if (gws.questCompleted && PI.activeNpc == this.transform && PI.Interact()) {
            if (PI.MissingHatFound && !showMissingHat[3].enabled) {
                showMissingHat[3].enabled = true;
                showMissingHat[playerHat].enabled = false;
                Debug.Log("npc hat= " + showMissingHat[3].enabled+  " player hat " + showMissingHat[playerHat].enabled);
            
            }/*else if (PI.HaveRewardHat && !showMissingHat[4].enabled) {
                showMissingHat[4].enabled = true;
                showMissingHat[playerHat].enabled = false;
                Debug.Log("npc hat= " + showMissingHat[4].enabled + " player hat " + showMissingHat[playerHat].enabled);
            }*/
        
        }
    }
    
}
