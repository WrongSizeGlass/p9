using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoodWillSystem : MonoBehaviour
{
    private float startGoodWill =5.5f;
    public float MyGoodWill = 0;
    public float GoodWillOffset = 0;
    private int otherGoodWill = 0;

    private BiasFoundation myBiasScript;
    private BiasFoundation otherBiasScript;

    private PlayerInteract playerPI;
    private PlayerInteract getPlayerPI;

    private GoodWillSystem playerGoodWillScript;
    private GoodWillSystem npcGoodWillScript;

    private NPCDialog npcdia;

    public float myBiasValue;
    private float originalBiasValue;
    public float myIndividualBiases;
    public int otherGeoIndex, otherThickIndex, otherPatIndex;
    
    public float otherGeoBias, otherThickBias, otherPatBias;
    public int playerInteractionCounter = 0;
    public int npcInteractionCounter = 0;
    private bool canInteract = false;
    private bool FirstNpcInteract = false;
    private bool FirstPlayerInteract = false;
    public int interactions = 0;

    private float biasModifier;
    private float goodWillModifier;
    bool Npc = false;
    bool isInteracting = false;
    public bool questNpc = false;
    
   
    public bool CanInteract = false;
    public List<float> playerBiasList;

    [HideInInspector] public bool helloDialog = true;

    [HideInInspector] public bool sayGoodBye = true;
    [HideInInspector] public bool questIsCompletedDialog = false;
    [HideInInspector] public bool questIsCompleted = false;
    [HideInInspector] public bool askForQuestDialog = false;
    [HideInInspector] public bool acceptQuestDialog = false;
    [HideInInspector] public bool MyGoodWillHasUpdated = false;


    public ElectricQuestController EQC;
    public MarketController MC;
    
    //bool npcHasQuest = false;
    [Header("Quest is complete ")]
    public bool questCompleted = false;
    // Start is called before the first frame update
    Dialog dia;
    void Start()
    {
        myBiasScript = GetComponent<BiasFoundation>();
        if (this.gameObject.tag != "Player") {
            Npc = true;
            if(questNpc){
                askForQuestDialog = true;
                acceptQuestDialog = true;
                questIsCompletedDialog = true;
                
            }
            npcdia = GetComponent<NPCDialog>();

        }
        else {
            dia = GetComponent<Dialog>();
            playerPI = GetComponent<PlayerInteract>();
        }
        float value = 0;

        value = myBiasScript.GeneralBias;
       // Debug.LogError("¤¤ " + (value<0));
     
        
       MyGoodWill = startGoodWill;

    }
    bool onetime = false;
    private void abc()
    {
        if (Npc && !onetime)
        {
            playerGoodWillScript = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<GoodWillSystem>();
            getPlayerPI = playerGoodWillScript.transform.GetComponent<PlayerInteract>();
            MyGoodWill = getGoodWillModifier(getPlayerPI);
            onetime = true;
        }
    }

    public float getGoodWillModifier(PlayerInteract PI, bool updatePlayer =false) {
        biasModifier = 0;
        myBiasValue = 0;
        float gf = 0;
        gf = goodFaith;
        if (otherBiasScript == null && Npc) {
            otherBiasScript = PI.GetComponent<BiasFoundation>();
        }

        if (otherBiasScript == null && !Npc && !updatePlayer) {
            otherBiasScript = PI.activeNpc.GetComponent<BiasFoundation>();
        }
        
            otherGeoIndex = otherBiasScript.myGeoIndex;
            otherThickIndex = otherBiasScript.myThicknessIndex;
            otherPatIndex = otherBiasScript.myPatterenIndex;

            otherGeoBias =  myBiasScript.myBiases[otherGeoIndex] ;
            otherThickBias =  myBiasScript.myBiases[otherThickIndex] ;
            otherPatBias =  myBiasScript.myBiases[otherPatIndex] ;

        float biasSum = (otherGeoBias + otherThickBias + otherPatBias);
           
        myIndividualBiases = biasSum==0?0: biasSum / 3;
        myBiasValue = myBiasScript.updateMyBias(otherGeoBias, otherThickBias, otherPatBias,gf);
        biasModifier = (myBiasValue + myIndividualBiases);

        return biasModifier;
    }
  
  [HideInInspector]  public int savePlayerInteractionCounter = 0;
    bool updatePlayerAfterInteraction = false;
    bool q = false;
    // Update is called once per frame
    void Update() {
        abc();
        // Initialization
        if (!Npc) {
            if (playerPI.isInteracting) {

                isInteracting = playerPI.Interact();
                if (npcGoodWillScript == null && playerPI.activeNpc!=null) {
                    npcGoodWillScript = playerPI.activeNpc.GetComponent<GoodWillSystem>();

                }
                npcInteractionCounter = playerPI.activeNpc != null ? this.npcGoodWillScript.npcInteractionCounter: npcInteractionCounter;
                updatePlayerAfterInteraction = true;
            }else{
                if(updatePlayerAfterInteraction){
                    myBiasScript.updatePlayerBias();
                    MyGoodWill = myBiasScript.GeneralBias;
                    updatePlayerAfterInteraction = false;
                }
            }
            npcInteractionCounter = Mathf.Clamp(npcInteractionCounter, 0, 4);
            playerInteractionCounter = Mathf.Clamp(playerInteractionCounter, 0, 4);
            if (!playerPI.isInteracting) {
                goodFaith = 0;
            }
        }
        if (this.name == "ActiveNpc")
        {

            if (playerGoodWillScript == null)
            {

                playerGoodWillScript = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<GoodWillSystem>();
                getPlayerPI = playerGoodWillScript.transform.GetComponent<PlayerInteract>();
                npcInteractionCounter = 0;
            }

            isInteracting = getPlayerPI.isInteracting;
            npcInteractionCounter = Mathf.Clamp(npcInteractionCounter, 0, 4);
            

        } else {

            isInteracting = false;
        }


        if (this.name == "ActivePlayer" && playerPI.Interact())
        {
            if (MyGoodWill == startGoodWill)
            {
                MyGoodWill = myBiasScript.GeneralBias;
            }

            if (playerPI.isInteracting)
            {
                if (myBiasScript.getResponds() != "")
                {
                    if (myBiasScript.getRespondsType() == "positive")
                    {
                        this.goodFaith += 0.75f;
                    }
                    else if (myBiasScript.getRespondsType() == "negative")
                    {
                        this.goodFaith -= 0.75f;
                    }
                    else
                    {
                        this.goodFaith += 0.12f;
                    }
                    MyGoodWill = (getGoodWillModifier(playerPI) + this.goodFaith);
                    myBiasScript.setResponds("Neutral", "");

                }

            }

        }
        
        if (isInteracting && this.name == "ActiveNpc"){          
            if ( !npcdia.hasSaidHello){
                MyGoodWill = getGoodWillModifier(getPlayerPI);
                meFirst = true;
            }

            if (myBiasScript.getResponds() != ""){
                if (myBiasScript.getRespondsType() == "positive"){
                    this.goodFaith += 0.75f;

                }else if (myBiasScript.getRespondsType() == "negative"){
                    this.goodFaith -= 0.75f;

                }else{
                    this.goodFaith += 0.15f;
                }
                MyGoodWill = (getGoodWillModifier(getPlayerPI) + this.goodFaith); 
                myBiasScript.setResponds("Neutral", "");
                hasUpdated = true;
            }           
        }else {
            if (backupGW != 0)
            {
               // Debug.LogError("<> " + backupGW);
                MyGoodWill = backupGW;

            }

            goodFaith = 0;
        }
        // Player exit dialog reset counters
        if (!Npc && playerPI.overrideShutdown)
        {
            otherBiasScript = null;
            playerInteractionCounter = 0;
            npcInteractionCounter = 0;
            interactions = 0;
            npcGoodWillScript = null;
        }

        if (save && Npc && !hasSaved && interactions>0){
            savePlayerBiasList();
            //getPlayerPI.GetComponent<Dialog>().ExitDialog();
        }

        if (Npc && isInteracting && getPlayerPI.activeNpc == this.transform) {
            
            if (npcdia.myQuestIndex==2) {
                questCompleted = EQC.ThisQuestIsComplete();
            }
            if (npcdia.myQuestIndex == 1) {
                questCompleted = MC.questIsComplete;
            }
            if (npcdia.myQuestIndex == 0 && getPlayerPI !=null) {
                if (getPlayerPI.MissingHatFound || getPlayerPI.HaveRewardHat) {
                    questCompleted = true;
                }
                
            }
          //  Debug.LogError("id" + npcdia.myQuestIndex + " I am interaction my quest is " + questCompleted);
        }

    }
    public bool hasUpdated = false;
    public bool canContinue = false;
    float backupGW =0;
    public bool meFirst = false;
    int a = 0;
    public float goodFaith = 0;
    public bool save = false;
    public bool hasSaved = false;
    public void savePlayerBiasList(){

        playerBiasList = new List<float>();
        for (int i = 0; i < otherBiasScript.myBiases.Count; i++){
            playerBiasList.Insert(i, otherBiasScript.myBiases[i]);
        }
        backupGW = MyGoodWill;
        savePlayerInteractionCounter = playerGoodWillScript.playerInteractionCounter;
        hasSaved = true;
    }

}
