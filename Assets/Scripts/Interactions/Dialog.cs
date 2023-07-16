using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class Dialog : MonoBehaviour
{
    public Canvas dialogCanvas;
    private PlayerInteract PI;
    private bool activeDialog = false;
    private GoodWillSystem GWS;
    private BiasFoundation npcBF;
    public float minNegativeGoodWill = 4.5f;
    public float minNegativeGoodWillOffset = 2.5f;
    public float maxPositiveGoodWill = 6.5f;
    public float maxPositiveGoodWillOffset = 8.5f;
    private float myGoodWill = 5;
    private string type = "empty";
    private string positive = "positive";
    private string negative = "negative";
    private string neutral = "neutral";
    private string selectedDialog = "Empty";
    public List<Button> dialogButtons;
    private List<Text> dialogOptions;
    public int activeButton = 3;
    public MainScreenController mc;
    AudioSource audio;
    public AudioClip click;
    private bool start = false;
    void playAudio()
    {

        if (!audio.isPlaying && !start )
        {

            start = true;
            audio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audio.volume = UnityEngine.Random.Range(0.55f, 6.5f);
            audio.PlayOneShot(click);
            audio.Play();
            //Debug.LogError("Play!!");

        }     
        else
        {
            if (!audio.isPlaying)
            {
                start = false;
                audio.Stop();
            }
        }
    }
    // geo: 0,1 | thick 2,3  | pat 4,6
    /*

     0: tri thick
     1: tri thin
     2: cube thick
     3,4: cube thin
     5,6: cube thin squard
     */
    List<string> Hellos = new List<string> { " Hi." ,
                                             " Hey." ,
                                             " Greetings. "
                                           };

    List<string> zzzznegativeAddOnes = new List<string> {// This should be moved to npcdialog script

                       /* triangle thick */           "Cheese",
                       /* triangle thin */            "Triangle",
                      /* cube thick */                "Brick", "Box" ,
                      /* cube, thin*/                 "Table", "plank",
                      /* cube,thin, squard*/          "Doodle","Chessboard"
                                                    };

    List<string> HelloNegativeAddOnes = new List<string> { "I am busy. ", "You are in my way ", "I was going the other way " };

    List<string> HelloPositiveAddOnes = new List<string> { "Top of the mornin’ to ya!. ",
                                                           "Sunshine!",
                                                           "What's good. "
                                                         };
    // Quest strings
    List<string> AskForQuest = new List<string> { "What do you need? ","What can I help you with? ", "What can I do? ",
                                                  "What should I do? ", "How can I help you? " };

    List<string> AskForQuestPositiveAddOns = new List<string> { " I would be happy to help. ", " Sure. ", " Yea. ", " Of course. " };

    List<string> AskForQuestNegativeAddOns = new List<string> { "No I don't think so but. ", "I don't like you but. ",
                                                                "Hmmm Only out of the kindness of my heart. ",
                                                                "I don't want to work for you but. ", "Naa not today but. " };
    // Accept Quest
    List<string> AcceptQuest = new List<string> { "Sure", "See you later", "I will do it ", "I should go." };
    List<string> EmptyAddOns = new List<string> { ".  ", ".  ", ".  ", ".  " };

    // Dialog Type
    List<string> types = new List<string> { "neutral", "negative", "positive" };

    List<string> possibleDialogs = new List<string> { " BTN1 ", " BTN2 ", " BTN3 ", "BTN4" };


    List<string> questCompleted = new List<string> { " I have done it ", " I got your {Thing} ", " I have completed your task " };

    List<string> GoodByes = new List<string> { "I should go. ",
                                               "See you around. " ,
                                               "GoodBye. "

                                              };



    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        dialogCanvas.enabled = false;
        PI = GetComponent<PlayerInteract>();
        GWS = GetComponent<GoodWillSystem>();
        // npcBF = PI.activeNpc.transform.GetComponent<BiasFoundation>();
        dialogOptions = new List<Text>();
        for (int i = 0; i < dialogButtons.Count; i++)
        {
            dialogOptions.Insert(i, dialogButtons[i].GetComponentInChildren<Text>());
        }
        minNegativeGoodWillOffset = minNegativeGoodWill - 2;
        maxPositiveGoodWillOffset = maxPositiveGoodWill + 2;
        turnButtonsOnAndOff(true);

    }
    void Awake()
    {
        Debug.LogWarning("Unlock cursor press q ");
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    // 5.5f <-> 6.5f  Neutral
    // Goodwill > 6.5f  Positive
    // Goodwill < 5.5f Negative


    bool doubleNegative = false;
    bool doublePositive = false;

    void GetDialog(bool writeDialogOnce, List<string> mainList, List<string> positiveAddOns, List<string> negativeAddOns,
                     bool noAddOns = false)
    {
        int randomMainIndex=0;
        int randomMainIndex2=0;
        int randomAddOn = 0;
        int randomAddOn2 = 0;
        doubleNegative = false;
        doublePositive = false;
        if (writeDialogOnce)
        {
            
            // say something then add on fx Hello
                for (int i = 0; i < dialogOptions.Count; i++)
                {
                    if (i == 0)
                    {
                        dialogOptions[0].text = mainList[(int)UnityEngine.Random.Range(0, mainList.Count - 1)];
                        //type = neutral;
                    }
                    else
                    {

                         // give an options from each
                            doubleNegative = false;
                            doublePositive = false;
                            if (i % 2 == 1)
                            {
                                 randomMainIndex = (int)UnityEngine.Random.Range(0, mainList.Count);
                                 randomAddOn = (int)UnityEngine.Random.Range(0, negativeAddOns.Count);
                                // give a negative
                                dialogOptions[1].text = mainList[randomMainIndex] + " " + negativeAddOns[randomAddOn];
                                type = negative;
                                // dialog 1 = negative
                              //  Debug.LogError(" one of each" + myGoodWill);

                            }
                            else
                            { // give a positive

                                 randomMainIndex2 = (int)UnityEngine.Random.Range(0, mainList.Count);
                                if (randomMainIndex2 == randomMainIndex) {
                                    randomMainIndex2 = (int)UnityEngine.Random.Range(0, mainList.Count);
                                }
                                randomAddOn2 = (int)UnityEngine.Random.Range(0, negativeAddOns.Count);
                                if (randomAddOn2 == randomAddOn) {
                                    randomAddOn2 = (int)UnityEngine.Random.Range(0, negativeAddOns.Count);
                                }
                                dialogOptions[2].text = mainList[randomMainIndex2] + " " + positiveAddOns[randomAddOn2];
                                type = positive;
                                // dialog 2 = postive
                            }
                        

                        possibleDialogs[i] = dialogOptions[i].text;
                    }
                }
            
        }

    }

    void askForQuest(List<string> mainList, List<string> negativeAddOns, List<string> positiveAddOns)
    {
        for (int i = 0; i < dialogOptions.Count; i++)
        {

            if (i == 0)
            {
                dialogOptions[i].text = mainList[(int)UnityEngine.Random.Range(0, mainList.Count - 1)];
                type = neutral;
            }
            else
            {

                
                    if (i % 2 == 1)
                    { // give a negative
                        dialogOptions[i].text = negativeAddOns[(int)UnityEngine.Random.Range(0, negativeAddOns.Count - 1)] + " " + mainList[(int)UnityEngine.Random.Range(0, mainList.Count - 1)];
                        type = negative;
                        Debug.LogError("one of each " + myGoodWill);

                    }
                    else
                    { // give a positive
                        dialogOptions[i].text = positiveAddOns[(int)UnityEngine.Random.Range(0, positiveAddOns.Count - 1)] + " " + mainList[(int)UnityEngine.Random.Range(0, mainList.Count - 1)];
                        type = positive;
                        // dialog 2 = postive
                    }
                
            }
            possibleDialogs[i] = dialogOptions[i].text;
        }

    }



    void getGoodbyeDialog(List<string> mainList)
    {

        for (int i = 0; i < dialogOptions.Count; i++)
        {
            dialogOptions[i].text = mainList[(int)UnityEngine.Random.Range(0, mainList.Count - 1)];
            type = neutral;
        }
        // turnButtonsOnAndOff(false);

    }


    int index = 0;
    void setTypes(int index)
    {
        type = types[index];
    }
    public string getTypes()
    {
        return type;
    }
    string getDialog(int index)
    {
        return possibleDialogs[index];
    }
    public void getNeutralButtonValues()
    {

        activeButton = 0;
    }
    public void getPositiveButtonValues()
    {

        activeButton = 2;
    }
    public void getNegativeButtonValues()
    {

        activeButton = 1;
    }

    void turnButtonsOnAndOff(bool on)
    {
        for (int i = 0; i < dialogButtons.Count; i++)
        {
            dialogButtons[i].enabled = on;
            //dialogButtons[i].isActiveAndEnabled(on);
            dialogOptions[i].enabled = on;

        }

    }
    bool FinnishedWithQuestNpc = false;
    int hasInteracted = 0;
    bool once = false;
    void getHelloDialog()
    {
        if (helloDialog)
        {
            GetDialog(helloDialog, Hellos, HelloPositiveAddOnes, HelloNegativeAddOnes);
            helloDialog = false;
        }

        if (activeButton != 3 && !helloDialog)
        {
            Debug.Log("player say HI");
           
           // Debug.LogError("otherGW is null: " + (otherGW == null));
            otherGW.playerInteractionCounter = 1;

            GWS.playerInteractionCounter = 1;
            nd.SaidHello = true;
            Debug.Log("nd.SaidHello" + nd.SaidHello);
            sendDialogValues();
            activeButton = 3;
            // ResetDialogVariables();
        }

    }
    void getAskForQuestDialog()
    {
        if (askForQuestDialog)
        {

            askForQuest(AskForQuest, AskForQuestNegativeAddOns, AskForQuestPositiveAddOns);
            askForQuestDialog = false;
        }

        if (activeButton != 3 && !askForQuestDialog)
        {
            Debug.LogError("ASK FOR QUEST !!!!!!!!!!");
            sendDialogValues();
            activeButton = 3;

            otherGW.playerInteractionCounter = 2;
            GWS.playerInteractionCounter = 2;
            askForQuestOnce = true;
            nd.askedForQuest = true;
            //ResetDialogVariables();
        }
    }
    void getAcceptQuestDialog()
    {
        Debug.Log("ACCEPT QUEST");
        if (acceptQuestDialog)
        {
            getGoodbyeDialog(AcceptQuest);
            acceptQuestDialog = false;
        }
        //acceptQuestDialog = false;
        // turnButtonsOnAndOff(false);

        Debug.Log(" ACCEPT QUEST activeButton " + activeButton);
        if (activeButton != 3 && !acceptQuestDialog)
        {
            sendDialogValues();

            otherGW.playerInteractionCounter = 3;
            GWS.playerInteractionCounter = 3;
            acceptQuistOnce = true;
            nd.acceptedQuest = true;

            ExitDialog();
            //turnButtonsOnAndOff(false);

        }
    }
    bool notGoodBye = false;
    void newdialog()
    {
        if (this.nd.hasSaidHello && !this.nd.SaidHello)
        {
            notGoodBye = true;
            getHelloDialog();
            

        }
        else if (this.nd.hasGivenQuestIntro && !this.nd.askedForQuest && this.nd.SaidHello)
        {
            Debug.Log("player say askedForQuest");
            notGoodBye = true;
            getAskForQuestDialog();
            
            //Debug.LogError("getAskForQuestDialog !!!!!!!!!!!");


        } else if (this.nd.hasGivenDescription && !this.nd.acceptedQuest && this.nd.askedForQuest) {
            Debug.Log("player say accept quest");
            
            getAcceptQuestDialog();
            notGoodBye = false;

            Debug.LogError("getAcceptQuestDialog !!!!!!!!!!!");

        } else if (this.nd.CompletedQuest && this.nd.acceptedQuest) {
            notGoodBye = false;
            dialogOptions[0].text = "";
            dialogOptions[1].text = "";
            dialogOptions[2].text = "";
            if (activeButton != 3){
                
                ExitDialog();
            }

        }
        else if (this.nd.CompletedQuest && this.nd.hasGivenQuestIntro && !this.nd.acceptedQuest)
        {
                turnButtonsOnAndOff(false);
            notGoodBye = false;
        }
        else if (nd.hasSaidGoodby && !nd.saidGoodBye)
        {
            Debug.Log("player say goodbye");
           // sendDialogValues();
            turnButtonsOnAndOff(false);
            notGoodBye = false;
        }
        // Debug.LogError("Player hasGivenDescription : " + this.nd.hasGivenQuestIntro + " acceptedQuest :" + this.nd.askedForQuest + " askedForQuest :" + this.nd.SaidHello);


    }
    //counter should be 1 if goodbye and 2 or 3 if quest is completed
    void GetGoodByes(int counter)
    {
        if (sayGoodBye)
        {
            getGoodbyeDialog(GoodByes);
        }
        sayGoodBye = false;
        //  turnButtonsOnAndOff(false);
        if (activeButton != 3)
        {
            sendDialogValues();
            // nd.saidGoodBye = true;
            if (counter == 1)
            {
                otherGW.playerInteractionCounter = 1;
            }
            else
            {
                otherGW.playerInteractionCounter = 3;
            }

            Debug.LogError("EXIT!!");
        }

    }

    bool acceptQuistOnce = false;
   
    bool askForQuestOnce = false;
    
    bool helloOnce = false;

   

    public bool helloDialog = true;
    bool sayGoodBye = true;
    public bool askForQuestDialog = false;
    public bool acceptQuestDialog = false;
    bool questIsCompletedDialog = false;
    bool questIsCompleted = false;
    bool npcHasQuest = false;
    bool HasSetupDialogOptions = false;
    public bool thisShouldWork = false;
    GoodWillSystem otherGW;
    NPCDialog nd;
    void setupDialog()
    {


        askForQuestDialog = true;
        helloDialog = true; 
        acceptQuestDialog = true;
        //Debug.LogError("¤¤¤"+acceptQuestDialog);
        sayGoodBye = true;
        if (!nd.CompletedQuest) {
            nd.acceptedQuest = false;
        }
        questIsCompletedDialog = true;
        FinnishedWithQuestNpc = false;

        HasSetupDialogOptions = true;
        Exit = false;

    }

    int a = 0;
    void Update()
    {
        // Debug.LogError("? dialog:" + GetNpcDia() + " type " + GetNpcType()); ;
        /*if (GetNpcType() == positive || GetNpcType() == neutral || GetNpcType() == negative) {
            thisShouldWork = true;
        }*/


        Mathf.Clamp(GWS.playerInteractionCounter, 0, 4);
        activeDialog = PI.activeNpc!=null &&  PI.activeNpc.GetComponent<GoodWillSystem>() != null && PI.Interact() && !PI.overrideShutdown;
        dialogCanvas.enabled = activeDialog;
        if (PI.Interact() && nd==null)
        {
            
            npcBF = this.PI.activeNpc.GetComponent<BiasFoundation>();
            otherGW = this.PI.activeNpc.GetComponent<GoodWillSystem>();
            nd = this.PI.activeNpc.GetComponent<NPCDialog>();
            setupDialog();



            questIsCompletedDialog = otherGW.questIsCompletedDialog;
            npcHasQuest = otherGW.questNpc;
            HasSetupDialogOptions = true;

            if (!dialogOptions[1].enabled) {
                a++;
                turnButtonsOnAndOff(true);
            }
            activeButton = 3;
            if (this.nd.hasGivenDescription)
            {
                otherGW.playerInteractionCounter = 3;
            }
            else
            {
                otherGW.playerInteractionCounter = 0;
            }
            //Debug.LogError("dialog set up " + a);

        }
        if (PI.Interact()){

            Cursor.lockState = CursorLockMode.None;
            myGoodWill = GWS.MyGoodWill;
            newdialog();
            //start = true;
        }
        else{

            Debug.Log(" dialog setup" + HasSetupDialogOptions + PI.Interact());
            HasSetupDialogOptions = false;
            if (nd!=null) {
                ResetDialogVariables();
            }
            if (!mc.pause || mc.exit)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        try
        {
            if (PI.Ecounter % 2 == 1 && Cursor.lockState == CursorLockMode.Locked)
            {
                ExitDialog();
            }
        }
        catch (Exception e) { 
        }

        
    }

    void setRespondesType()
    {

        if (notGoodBye && activeButton == 1)
        {
            type = negative;
        }
        else if (notGoodBye && activeButton == 2)
        {
            type = positive;
        }
        else if (!notGoodBye || activeButton==0)
        {
            type = neutral;
        }
        

    }
    public void sendDialogValues()
    {
        setRespondesType();
        npcBF.setResponds(type, getDialog(activeButton));
        playAudio();
        activeButton = 3;
    }

    public void ResetDialogVariables()
    {
        //acceptQuestDialog = true;


        if (PI.Ecounter % 2 == 1)
        {
            PI.Ecounter++;
        }
        Debug.Log(" ecounter er lige" + (PI.Ecounter % 2 == 0));
       
        npcBF = null;
        otherGW = null;
        nd = null;
        once = false;
        HasSetupDialogOptions = false;

       // Exit = false;
        if (!PI.overrideShutdown)
        {

            questIsCompleted = false;
        }
        



    }


    void unlockMouse()
    {
        if (Input.GetKeyDown("q"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void DialogIsPositive()
    {// GWS.playerInteractionCounter should be incremented with the dialog that is sent to npcdialog
        Debug.Log(" Nice clicked ");
        GWS.playerInteractionCounter++;
        otherGW.playerInteractionCounter++;
    }
    public void DialogIsNeutral()
    {
        Debug.Log("Neutral clicked ");
        GWS.playerInteractionCounter++;
        otherGW.playerInteractionCounter++;
    }

    public void DialogIsNegative()
    {
        Debug.Log("negative clicked ");
        GWS.playerInteractionCounter++;
        otherGW.playerInteractionCounter++;
    }

    public void ExitDialog()
    {
        activeButton = 0;
        sendDialogValues();    
        SaveAndExit();

    }
    public bool Exit = false;
    public void SaveAndExit()
    {
        //ResetDialogVariables();
        PI.activeNpc.GetComponent<GoodWillSystem>().save = true;
        if (PI.activeNpc.GetComponent<GoodWillSystem>().hasSaved)
        {
            //otherGW = null;
            //npcBF = null;
            
            Debug.Log("override is true" + PI.Interact());
        }
        PI.overrideShutdown = true;
        Debug.LogError("Active npc " + PI.activeNpc ==null);
        //HasSetupDialogOptions = false;
        Exit = true;

    }

    public string diaToPlayer = "";
    public string TypeToPlayer = "";
    public void SetNpcDialog(string dia)
    {
        diaToPlayer = dia;
    }
    public void SetNpcType(string type)
    {
        TypeToPlayer = type;
    }
    public string GetNpcDia()
    {
        return diaToPlayer;
    }
    public string GetNpcType()
    {
        return TypeToPlayer;
    }



}
