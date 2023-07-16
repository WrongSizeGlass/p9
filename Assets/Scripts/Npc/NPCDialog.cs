using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    private Transform activeNpc;
    private Transform activePlayer;
    private PlayerInteract PI;
    public Text npcAnswer;
    private GoodWillSystem gw;
    private BiasFoundation otherBias;
    private BiasFoundation myBias;
    private float goodWill;
    [HideInInspector] public int dialogIndex=0;
    private string positive = "positive";
    private string neutral = "neutral";
    private string negative = "negative";
    private string completed = "completed";
    public string dialogstring;
    private bool isQuestActive = false;
    QuestList QL;
    public float minNegativeGoodWill = 4.5f;
    public float maxPositiveGoodWill = 6.5f;
    private float myID = 0;
    public MeshRenderer petRock;
    public GameObject Leash;
    private AudioSource sound;
    
    public Text q1;
    public Text q2;
    public Text q3;
    public Text q4;
    
    // 0 = fast dgsdsdrawing, 1 4 sec drawing.
    public List<AudioClip> ac;
// Dialog lists
    // Hallos
    List<string> PositiveHellos = new List<string> { " Hi it is lovely to see you" ,
                                                     " Hey how are you " ,                                                     
                                                     " Greetings and Salutations ",
                                                     " Hey good to see you, what's good? "
                                                     };

    List<string> NeutralHellos = new List<string> {" Hi " , 
                                                   " Hey " ,
                                                   " Hello"                                                
                                                    };

    List<string> NegativeHellosForPlayer1 = new List<string> { " Hi you pointy cheese ",
                                                               " Hey pointy can you help me filling the hole in my cheese ",
                                                               " What do you want?  Do you think you own this place?  ",
                                                               " I don't want any trouble here ",
                                                               " Look at what the pointy fat cat dragged in"
                                                              };

    List<string> NegativeHellosForPlayer2 = new List<string> { " Hello squard square ",
                                                               " Hi chessboard ", 
                                                               " Hey great you are here I am in need of a table ",
                                                               " Are you a some old doodle a childe throw away? ",
                                                               " Can I use you to fill up a whole in my wall? " 
                                                              };
    // Remarks                                                            
    List<string> NegativeRemarksForPlayer1 = new List<string> { " Smell you later ",
                                                                " You are making the weather is extra cheesy. " ,
                                                                " I believe you’ve been up to no gouda.."  };

    List<string> NegativeRemarksForPlayer2 = new List<string> { " Have I seen you in Ikea? ",
                                                                " Check mate. ", " I am no one's pawn" };

    List<string> PositiveRemarks = new List<string> { " Nice to meet you. ", " Great to see you. " };

    List<string> NeutralRemarks = new List<string> { " " };

    // Goodbuys                                                            
    List<string> NegativeGoodByesForPlayer1 = new List<string> { " FUCK OFF " };
    List<string> NegativeGoodByesForPlayer2 = new List<string> { " Get out here! I don't want to degrade into a chess board" };

    List<string> GoodByes = new List<string> { " See you around ", " Goodbye " };


    // List<string> PositiveGoodByesForPlayer1 = new List<string> { "  ", " " };

    
     // Quest intro
     List<string> PositiveQuestIntro = new List<string> { "I am glad you are here for I could use your help" ,
                                                        "If you don't mind I could use you help " ,
                                                        "I could use your help if you are willing "
                                                        };

    List<string> NeutralQuestIntro = new List<string> { "Can you help me finde something " ,
                                                        "I need you to help me with something  " ,
                                                        "our city needs your help "
                                                        };

    List<string> NegativeQuestIntroPlayer1 = new List<string> { " I could use your help but Look out! \n" +
                                                                "The killer’s brie-hind you! \n" +
                                                                "oh wait it is you." ,
                                                                "You’re gouda, but I’m feta. ",
                                                                "I have a job for some that is and blank sharp as you"
                                                                };

    List<string> NegativeQuestIntroPlayer2 = new List<string> {  "Hey I need you and a table ",
                                                                "Hey table! I have a job for you" ,
                                                                "I can't play chess but I have a need for a chessboard " ,
                                                                
                                                                
                                                               
                                                               };

 
    // Quest descriptions
    private List<string> QuestDescription = new List<string> { "I have lost my hat and I am hoping you can find it for me \n" +
                                                    " Last time I had it was near the well behind my house. \n" +
                                                    "My hat has black and green squares. ",

                                                    "My colleagues at the 'Market' has messed up their inventory. \n You ask them what they are missing" ,

                                                    "Some how the electricity from my house is gone \n Can you perhabs take a look and fix it? "
                                                     };
    // Quest Completed
    private List<string> CompletedQuestList = new List<string> { "Thank you for doing this ", "Thanks for this ",
                                                                "Good job now  " };

    private List<string> PositiveCompletedQuestListPlayer1 = new List<string> {"Thank you, your help is much appreciated",
                                                                              "Thank you, You’re looking gouda!",
                                                                              "Thank you, for your help"};

    private List<string> PositiveCompletedQuestListPlayer2 = new List<string> {"Thank you, I knew you would help, \n" +
                                                                              "I am so tried of people using us as pawns ",
                                                                              "Thank you, your help is much appreciated",
                                                                              "Thank you I am in check"
                                                                              };

    private List<string> negativeCompletedQuestListPlayer1 = new List<string> { "Thanks ... now go! \n I hate the smell of cheess.",
                                                                               "Now Go away stinky, before you stick me down. ",
                                                                               "I gotta get out of here, I’m lac-ghost intolerant."};
    private List<string> negativeCompletedQuestListPlayer2 = new List<string> { "Thanks ... now go! " +
                                                                                "\n I don't have time to play chess.",
                                                                               " Thanks for the help, \n" +
                                                                               " you must have had a checkered past right? ",
                                                                               " Once a pawn a time I got help from a chessboard..." };



    private List<string> negativeHellos;
    private List<string> negativeQuestIntro;
    private List<string> negativeRemarks;
    private List<string> positiveQuestComplete;
    private List<string> negativeQuestComplete;
    private string myQuest = "Empty";
    private string myQuestIsCompleted = "Empty";
    //private string defaultString = "";
    public int myQuestIndex = 4;
    float timer = 0.0f;

    void questGivers() {
        if (myID==0) {
        
        
        }
    
    
    }

    Quaternion backupRot;
    private Text myText;
    // Start is called before the first frame update
    void Start(){
        
        sound = GetComponent<AudioSource>();
        timer = UnityEngine.Random.Range(0.9f, 1.5f);
        negativeHellos = new List<string>();
        negativeQuestIntro = new List<string>();
        negativeRemarks = new List<string>();
        negativeQuestComplete = new List<string>();
        positiveQuestComplete = new List<string>();
        backupRot = new Quaternion(0,0,0,0);
        npcAnswer = GameObject.Find("NpcAnswer").GetComponent<Text>();


 

        if (myQuestIndex==0) {
            npcAnswer.enabled = false;
            npcAnswer.text = q1.text;
            
        }
        if (myQuestIndex == 1){
            npcAnswer.enabled = false;
            npcAnswer.text = q2.text;
        }
        if (myQuestIndex == 2){
            npcAnswer.enabled = false;
            npcAnswer.text = q3.text;
        }
        



        QL = GameObject.Find("NPCDialogSystem").GetComponent<QuestList>();
        activeNpc = GetComponent<Transform>();
        myID = UnityEngine.Random.Range(0.1f,5);
       // defaultString = npcAnswer.text;
    }
    public bool hasAlreadyCompletedMyQuest = false;
    bool AllowHelloDialog = false;
    bool AllowDialog = false;
    void copyList(List<string> originalList, List<string> newList) {
        
        for (int i = 0; i < originalList.Count; i++){
            newList.Insert(i, originalList[i]);
        }
    }
    int counter = 0;
    float y = 0.25f;
    bool once;
    void rotateMe(){
        if (counter % Mathf.Round(timer / Time.fixedDeltaTime) == 0){
            if (!once)
            {
                timer = timer * 3;
                once = true;
            }
            y = y * -1;
        }
        transform.Rotate(0.0f, y, 0.0f);
    }
    private void FixedUpdate(){

        try
        {
            if (PI.activeNpc != this.transform)
            {
                if (transform.rotation.x!=0) {
                    backupRot.y = y;
                    transform.rotation = backupRot;
                }
                counter++;
                rotateMe();
            }
        }
        catch (Exception e) { 
        
        }
    }
    Dialog dia;
    bool stop = false;
    // Update is called once per frame
    void Update()
    {
        
        if (activeNpc == null && GameObject.Find("ActiveNpc")){
            //activeNpc = GameObject.Find("ActiveNpc").GetComponent<Transform>();
        }
        if(activePlayer == null){
            activePlayer = GameObject.Find("ActivePlayer").GetComponent<Transform>();
            otherBias = activePlayer.GetComponent<BiasFoundation>();
            PI = activePlayer.GetComponent<PlayerInteract>();
            // copy string list depended on who the player is
            if (otherBias.myGeoIndex == 0){

                copyList(NegativeHellosForPlayer1, negativeHellos);
                copyList(NegativeQuestIntroPlayer1, negativeQuestIntro);
                copyList(NegativeRemarksForPlayer1, negativeRemarks);
                copyList(negativeCompletedQuestListPlayer1, negativeQuestComplete);
                copyList(PositiveCompletedQuestListPlayer1, positiveQuestComplete);  
            }else {
                copyList(NegativeHellosForPlayer2, negativeHellos);
                copyList(NegativeQuestIntroPlayer2, negativeQuestIntro);              
                copyList(NegativeRemarksForPlayer2, negativeRemarks);
                copyList(negativeCompletedQuestListPlayer2, negativeQuestComplete);
                copyList(PositiveCompletedQuestListPlayer2, positiveQuestComplete);
            }
        }
        if(PI.isInteracting && gw == null && this.name == "ActiveNpc"){
           gw = GetComponent<GoodWillSystem>();
            AllowDialog = gw.CanInteract;
            otherBias = activePlayer.GetComponent<BiasFoundation>();
            PI = activePlayer.GetComponent<PlayerInteract>();
            dia = PI.transform.GetComponent<Dialog>();
            if (gw.questCompleted){
                hasAlreadyCompletedMyQuest = true;
            }
            // npcAnswer.text = defaultString;
            //Debug.Log("is QL null: " +(QL==null));
           
        }
       /* if(!PI.isInteracting){

        }*/
        
        if (PI.Interact() && this.name == "ActiveNpc" && PI.activeNpc==this.transform)
        {
            backupRot.y = this.transform.rotation.y;
            string dialogString = "";
            transform.LookAt(PI.transform);
            
            if (myQuestIndex == 0){
                q1.enabled = true;
                q2.enabled = false;
                q3.enabled = false;
                q4.enabled = false;

            }
            if (myQuestIndex == 1){
                q1.enabled = false;
                q2.enabled = true;
                q3.enabled = false;
                q4.enabled = false;

            }
            if (myQuestIndex == 2){
                q3.enabled = true;
                q2.enabled = false;
                q1.enabled = false;
                q4.enabled = false;

            }
            if (myQuestIndex ==4) {
                q3.enabled = false;
                q2.enabled = false;
                q1.enabled = false;
                q4.enabled = true;

            }

            // say hello
            if (this.gw.playerInteractionCounter == 0 && !hasSaidHello){
                updateGoodWillAndInteractions(1);
                dialogString = setDialog(NeutralHellos, PositiveHellos, negativeHellos, gw.questNpc);
                playSound(0);
                hasSaidHello = true;
                npcAnswer.text = dialogString;
            }
            else if (gw.questNpc && gw.playerInteractionCounter > 0)
            {




                getQuestRelatedDialog();

                // Debug.Log("quest related dialog ");

                // Give Tips to player [ Market place quest]
            }
            else if (IcanGiveTips && !gw.questNpc && gw.playerInteractionCounter == 1
                        && !DisallowTips.questIsCompleted && !IhaveGivenTips)
            {

                dialogString = myTips;
                otherBias.setResponds("neutral", dialogstring);
                updateGoodWillAndInteractions(2);
                playSound(ac.Count - 1);
                IhaveGivenTips = true;
                hasSaidGoodby = true;
                Debug.Log("Give tips ");
                npcAnswer.text = dialogString;
                backUpstring = dialogString;
                // Say goodbye
            }
            else if (!gw.questNpc && this.gw.playerInteractionCounter >= 1 && !hasSaidGoodby && !IcanGiveTips )
            {

                end = false;
                updateGoodWillAndInteractions(2);
                gw.MyGoodWill = gw.getGoodWillModifier(PI);
                goodWill = gw.MyGoodWill;
                dialogString = setDialog(GoodByes, GoodByes, GoodByes, gw.questNpc, NeutralRemarks, PositiveRemarks, negativeRemarks);
                playSound(ac.Count - 1);
                hasSaidGoodby = true;
                end = true;
                Debug.Log(" say good bye ");
                npcAnswer.text = dialogString;
                backUpstring = dialogString;

            }
            else {
                 
                if (hasSaidGoodby) {
                    
                    npcAnswer.text = backUpstring;
                }
            }
            
            
        }
        else
        {
           
            activeNpc = null;
        }

        if (questLogo!=null) {
            questLogo.gameObject.SetActive(!CompletedQuest);
            if (CompletedQuest) {
                // questLogo.position = new Vector3(0,-999,0);
               
            
            }
        }


    }
    float pitch;
    string backUpstring;
    void playSound(int index ) {
        int i = index;
        if (goodWill < minNegativeGoodWill && !end){

            pitch = UnityEngine.Random.Range(0.5f, 0.8f);
        }else if (goodWill > maxPositiveGoodWill && !end){

            pitch = UnityEngine.Random.Range(1.3f, 1.5f);
        }else if(goodWill> minNegativeGoodWill && goodWill < maxPositiveGoodWill && !end)
        { 
            
            pitch = 1.0f;
        }
        if (!sound.isPlaying)
        {
            end = false;
            Debug.LogError("99 stop playing music");
            sound.Stop();
            if (CompletedQuest) {
                sound.enabled = false;
            }
            
        }

        if (!end && !sound.isPlaying  && i==index)
        {
            end = true;
            sound.pitch = pitch;
            sound.volume = UnityEngine.Random.Range(0.8f, 1f);
            sound.PlayOneShot(ac[index]);
            sound.Play();
            Debug.Log(" say good bye play index " + index);
            i++;
        }else {
            
            Debug.LogError("99 Else is true");

        }

    }

    /*
       bool playing = false;
    public void playSound() {
        if (!audioSource.isPlaying) {
            playing = false;
            audioSource.Stop();
        }
        if (!playing && !audioSource.isPlaying) {
            playing = true;
            audioSource.PlayOneShot(Explosion, volume);
        }
    }
     
     
     */
    bool end = false;
    void updateGoodWillAndInteractions(int counterValue) {
     //   gw.MyGoodWill = gw.getGoodWillModifier(PI);
        goodWill = this.gw.MyGoodWill;
        this.gw.npcInteractionCounter++;
        this.gw.interactions++;
        Debug.LogError(goodWill + " ___ " ); 
        
    }

    [HideInInspector] public bool hasGivenQuestIntro = false;
    public bool hasGivenDescription = false;
    [HideInInspector] public bool hasSaidHello = false;
    [HideInInspector] public bool hasSaidGoodby = false;
    public bool IcanGiveTips = false;
    [HideInInspector] public bool IhaveGivenTips = false;
    [HideInInspector] public bool CompletedQuest = false;
    [HideInInspector] public bool SaidHello = false;
    [HideInInspector] public bool askedForQuest = false;
    [HideInInspector] public bool acceptedQuest = false;
    [HideInInspector] public bool saidGoodBye = false;

    public Transform questLogo;
    public GoodWillSystem DisallowTips;
    public string myTips = "I am missing a hat that represent me";
    bool stopPlaying = false;
    void getQuestRelatedDialog(string CostumDescription = "" ) {
        int index = 0;
        string dialogString = "";
        // quest intro
        if (this.gw.playerInteractionCounter == 1 && !hasGivenQuestIntro && !sound.isPlaying && this.SaidHello && gw.hasUpdated)
        {
           
            //end = false;
            updateGoodWillAndInteractions(2);
            dialogString =setDialog(NeutralQuestIntro, PositiveQuestIntro, negativeQuestIntro, gw.questNpc);
            index = 2;
            playSound(index);
            hasGivenQuestIntro = true;
            Debug.Log("888 " + "isplaying " + sound.isPlaying);
            gw.hasUpdated = false;
            // end = true;
            // Debug.LogError(" quest intro"  + hasGivenQuestIntro);
            npcAnswer.text = dialogString;
        }
        else
        // quest description
        if (this.gw.playerInteractionCounter == 2 &&  !this.hasGivenDescription &&  !this.gw.questCompleted && !sound.isPlaying && gw.hasUpdated)
        {
           // end = false;
            updateGoodWillAndInteractions(3);
            
            dialogString = QuestDescription[myQuestIndex];
            Debug.LogError("test " + QuestDescription[myQuestIndex]);
            index = 1;
            playSound(index);
            hasGivenDescription = true;
            npcAnswer.text = dialogString;
            gw.hasUpdated = false;
            // end = true;
            //Debug.LogError(" Description " + hasGivenDescription);
        }
        else
        // quest complete
        if (this.gw.playerInteractionCounter >= 2 && this.hasGivenQuestIntro && this.gw.questCompleted && !sound.isPlaying&& gw.hasUpdated)
        {
            //  end = false;
            gw.hasUpdated = false;
            if (this.hasGivenDescription){
                index = 2;
            }else {
                index = 1;
            }
            stopPlaying = false;
            if (!sound.isPlaying) {
                
                playSound(index);
            }
           // end = true;
            updateGoodWillAndInteractions(3);
            dialogString = setDialog(CompletedQuestList, positiveQuestComplete, negativeQuestComplete, false);
            if (hasAlreadyCompletedMyQuest) {
                dialogString = "Oh you have already done my task \n " + dialogString;
            }

            if (this.myQuestIndex == 0 && goodWill > maxPositiveGoodWill)
            {
                dialogString = dialogString + " \n Here is a Hat as thank you a reward";
                PI.TurnOnRewardHat = true;
            }
            if (this.myQuestIndex == 1 && goodWill > maxPositiveGoodWill)
            {
                dialogString = dialogString + " \n Thanks for your help, how come you don't have a Pet Rock? \n" +
                "here you take my spare as thanks \n" +
                "I called it Ginny but you can call it what you want";
                this.petRock.gameObject.SetActive(true);               
            }
            if (this.myQuestIndex == 2 && goodWill > maxPositiveGoodWill)
            {
                dialogString = dialogString + " \n  Thanks for your help, here is a leash for your Petrock";
                Leash.active = true;
                leash = true;

            }


            this.CompletedQuest = true;

            npcAnswer.text = dialogString;
        }
        else if (gw.playerInteractionCounter >= 2 && gw.questCompleted && CompletedQuest)  {
            index = ac.Count - 1;
            playSound(index);
            if (!sound.isPlaying) {
                sound.enabled = false;
            }
            dialogString = "";

        }

        //npcAnswer.text = dialogstring;
        //Debug.LogError("NPC hasGivenDescription : " + hasGivenQuestIntro + " nd.acceptedQuest :" + askedForQuest + " nd.askedForQuest :" + SaidHello);
    }

    public bool leash = false;
    string setDialog(List<string> neutralDialog, List<string> positiveDialog, List<string> negativeDialog, bool quest=false, List<string> neutralRemarks = null, List<string> positiveRemarks = null, List<string> negativeRemarks = null) {
        
       // allow neutral
        if (inRange(goodWill, minNegativeGoodWill, maxPositiveGoodWill)) {

           return Dialog(neutralDialog, neutral, gw.questNpc, neutralRemarks);
       
        // allow positive
        }else if (  goodWill> maxPositiveGoodWill) {
            return Dialog(positiveDialog, positive, gw.questNpc, positiveRemarks);
        
        // allow negative
        }else{ 

            return Dialog(negativeDialog, negative, gw.questNpc, negativeRemarks);         
        }
    }

    string Dialog(List<string> dialog, string microagression, bool quest =false ,List<string> remarks = null){
        try{
            if (quest){
                this.dialogIndex = this.myQuestIndex;
            }else {
                dialogIndex = (int)UnityEngine.Random.Range(0, dialog.Count);
            }
            
            if (remarks == null) {
                
                dialogstring = dialog[dialogIndex];
                otherBias.setResponds(microagression, dialogstring);
                return dialogstring;


            }else{
                
                int remarkIndex = (int)UnityEngine.Random.Range(0, remarks.Count);
                dialogstring = remarks[remarkIndex]+"\n"+dialog[dialogIndex];
                otherBias.setResponds(microagression, dialogstring);
                return dialogstring;
            }
            
        }catch (Exception e){
            // dialogstring = null;
           return dialogstring = " ERROR  out of dialog options from list first index = " + dialog[0];
            //gw.npcInteractionCounter++;
        }
    }

    bool inRange (float value, float min, float max = 99999){
        return value > min && value < max;
    }

}
/*
               
 
 
 */