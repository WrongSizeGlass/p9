using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Text startInteract;
    private Outline ol;
    [HideInInspector] public Transform activeNpc;
    [HideInInspector] public Transform SelectNpc;
    public Color color;
    public Transform lowerPart;
    [HideInInspector] public RaycastHit hit;
    GoodWillSystem GWS;
    GoodWillSystem otherGWS;
    BiasFoundation BF;
    PlayerInteract thisScript;
    public BoxCollider BodyCollider;
    private CameraScript CS;
    private PlayerMovement PM;
    private Vector3 raypos;

    [HideInInspector] public bool isInteracting = false;

    private string npcName;
    private string activeNpcName = "ActiveNpc";
    private float downDisRange;
    private float downDis = 0.1f;
    public bool overrideShutdown = false;
    public bool questCompleted = false;
    float updateGoodWill = 0;

    public Transform grabbingPos;
    private Transform grabbedObject;
    public Rigidbody grabObjectRB;
    bool hasPressed = false;
    bool canGrab = true;
    public int Ecounter = 0;
    bool activateGrabShopObj = false;
    [HideInInspector] public bool MissingHatFound = false;
    [HideInInspector] public bool HaveRewardHat = false;
    [HideInInspector] public bool TurnOnRewardHat = false;
    public MeshRenderer Triangle;
    public MeshRenderer Cube;
    public bool PlayerIsNotInteracting = false;
    public AudioClip grabsound;
    private AudioSource sound;
    private Vector3 middlepart;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        GWS = GetComponent<GoodWillSystem>();
        // BodyCollider = GetComponent<BoxCollider>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        BF = GetComponent<BiasFoundation>();
        thisScript = GetComponent<PlayerInteract>();
        CS = transform.GetChild(0).GetComponent<CameraScript>();
        PM = GetComponent<PlayerMovement>();
        dia = GetComponent<Dialog>();

    }
    int counter = 0;
    Vector3 downRay;
    bool doNotInteract;
    bool doNotGrab;
    List<string> tags = new List<string> { "PipQuest", "ShopObject", "Reset", "LostHat", "NPC", "resetPips" };
    bool play = false;
    bool WireBool = false;
    bool GrabHatBool = false;
    // Update is called once per frame
    void Update()
    {
        counter++;
        raypos = BodyCollider.transform.position;
        middlepart = lowerPart.position;
        middlepart.y = middlepart.y + 0.333f;
       
        PressInteract();
        hasPressed = Ecounter % 2 == 1;
        if (PcCanInteract() && !activateGrabWire){
            switch (hit.collider.tag){
                case "NPC":
                    startUp();
                    IAmInteracting();
                    break;
                case "LostHat":
                    startUp();
                    grabMissingHat();
                    break;
                case "resetPips":
                    resetPips();
                    break;
                case "Reset":
                    resetShopObjects();
                    break;
                case "ShopObject":
                    WireBool = true;
                    break;
                case "PipQuest":
                    WireBool = true;
                    break;
            }
        }
        if (WireBool){
            grabWire();
        }else {
            if (activateGrabWire) {
                resetGrabbing();
            }
        }

        totalReset();
       
        if (TurnOnRewardHat)
        {
            if (BF.FirstPlaythrough && !Triangle.enabled)
            {
                Triangle.enabled = true;
            }
            if (!BF.FirstPlaythrough && !Cube.enabled)
            {
                Cube.enabled = true;
            }

        }

    }
    bool checkOnce = false;
    bool start = false;
    void playAudio()
    {

        if (!sound.isPlaying && !start)
        {

            start = true;
            sound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            sound.volume = UnityEngine.Random.Range(0.55f, 0.65f);
            sound.PlayOneShot(grabsound);
            sound.Play();

        }
        else
        {
            if (!sound.isPlaying)
            {
                start = false;
                sound.Stop();
            }
        }
    }
    void PressInteract(){
        if (Input.GetKeyDown(KeyCode.E) && PcCanInteract()){
          Ecounter++;
          playAudio();
        }
    }
    void grabMissingHat()
    {


           // ol = hit.collider.transform.GetComponent<Outline>();
           // ol.enabled = true;
          //  startInteract.enabled = true;
            if (hasPressed)
            {
                ol.gameObject.SetActive(false);
                //ol.enabled = false;
                startInteract.enabled = false;

                MissingHatFound = true;
                //hit.collider.enabled = false;
                Ecounter++;
            }
        

    }

    void resetShopObjects()
    {

            startInteract.enabled = true;
            if (hasPressed)
            {
                hit.collider.transform.GetComponent<ResetMarket>().ResetShop();
                //hit.collider.transform.GetComponent<Outline>().ResetShop();
                startInteract.enabled = false;
                Ecounter++;// resetPips
        }


    }
    void resetPips()
    {

        startInteract.enabled = true;
        if (hasPressed)
        {
            
            hit.collider.transform.GetComponent<resetPips>().resetPip();
            startInteract.enabled = false;
            Ecounter++;// resetPips
        }


    }

    void grabShobObj()
    {
        if ( !activateGrabShopObj)
        {
            try {
                grabbedObject = hit.collider.transform.GetComponent<Transform>();
            } catch (Exception e) {
            }
            if (activeNpc != null)
            {
                ol = grabbedObject.GetComponent<Outline>();
                ol.enabled = true;
                startInteract.enabled = true;
                grabObjectRB = grabbedObject.GetComponent<Rigidbody>();
            }
            if (hasPressed)
            {
                activateGrabShopObj = true;
            }

        }
        if (hasPressed && activateGrabShopObj)
        {
            startInteract.enabled = false;
            if (ol != null)
                ol.enabled = false;
            grabObjectRB.useGravity = false;
            //grabbedObject.position = grabbingPos.position;



        }
        else
        {
            if (grabbedObject != null && activateGrabShopObj)
            {
                //resetGrabbing();
                activateGrabShopObj = false;
                //Ecounter++;
            }
        }
    }

    bool activateGrabWire = false;
    void grabWire(){

        if ( !activateGrabWire ){
            try{
                activeNpc = hit.collider.transform.GetComponent<Transform>();
            }
            catch (Exception e){
            }
            if (activeNpc != null && activeNpc.name!="ActiveNpc")
            {
               
                ol = activeNpc.GetComponent<Outline>();
                ol.OutlineColor = Color.yellow;
                ol.enabled = true;
                startInteract.enabled = true;
                grabObjectRB = activeNpc.GetComponent<Rigidbody>();
                
                if (hasPressed){
                    activateGrabWire = true;
                    grabObjectRB.useGravity = false;
                    grabObjectRB.position = grabbingPos.position;
                }
                if (activeNpc.GetComponent<PipLocked>() != null)
                {
                    canGrab = !activeNpc.GetComponent<PipLocked>().Locked;
                }
                else { canGrab = true; }
            }          
        }

        if (hasPressed && activateGrabWire && canGrab) {
            ol.OutlineColor = Color.grey;//color; ;
            if (activateGrabWire)
            {
                grabObjectRB.useGravity = false;
                grabObjectRB.position = grabbingPos.position;
            }
            
            startInteract.enabled = false;
            
            startInteract.enabled = false;
            //ol.enabled = false;
            

        }else {
            WireBool = false;
        }
    }

    Transform backupTransform;

    void totalReset() {
        if (!PcCanInteract() || temp==null) {
            if (ol !=null) {
                ol.enabled = false;
            }
            startInteract.enabled = false;
            overrideShutdown = false;
            isInteracting = false;
            questCompleted = false;
            activateGrabShopObj = false;
            activateGrabWire = false;

            if (grabObjectRB!=null) {
                grabObjectRB.useGravity = true;
            }

            ol = null;
            grabObjectRB = null;
            grabbedObject = null;
            
            if (activeNpc != null && activeNpc.name == "ActiveNPC")
            {
                activeNpc.name = npcName;
            }

            activeNpc = null;
            
            CS.enabled = true;
            PM.enabled = true;
            if (Ecounter%2==1) {
                Ecounter++;
            }
        }
    
    }

    void resetGrabbing()
    {
        // if (canGrab) { canGrab = false; }
        if (hasPressed){
            Ecounter++;
        }
        //Ecounter++;
        startInteract.enabled = false;
        ol.enabled = false;

        grabObjectRB.useGravity = true;
        activeNpc = null;
        activateGrabShopObj = false;
        activateGrabWire = false;
        ol = null;
        grabObjectRB = null;
        grabbedObject = null;
        CS.enabled = true;
        PM.enabled = true;
        // WireBool = false;

    }
    public void ResetInteraction(){

        startInteract.enabled = false;
        overrideShutdown = false;
        isInteracting = false;

        if (ol != null){
            ol.enabled = false;
            ol = null;
        }
        if (activeNpc != null && activeNpc.name=="ActiveNPC"){
            activeNpc.name = npcName;
        }

        activeNpc = null;
        questCompleted = false;

        CS.enabled = true;
        PM.enabled = true;
    }

    bool incrementEcounter = false;
    void startUp()
    {
        checkOnce = false;
       // Debug.LogError("target " + temp.name);
        activeNpc = temp;
        if (ol == null)
        {

            ol = activeNpc.GetComponent<Outline>();
            ol.enabled = true;
            startInteract.enabled = true;
        }

        if (hasPressed)
        {
            ol.enabled = false;
            startInteract.enabled = false;
        }
    }

    public Transform temp;
    void IAmInteracting()
    {
        if (hasPressed && !activateGrabWire  )
        {

            activeNpc.name = activeNpcName;

            isInteracting = hasPressed;
            incrementEcounter = true;
            // Get states from previus convensation from the same NPC
            if (activeNpc.GetComponent<GoodWillSystem>().npcInteractionCounter != 0)
            {
                GWS.playerInteractionCounter = activeNpc.GetComponent<GoodWillSystem>().savePlayerInteractionCounter;
                GWS.npcInteractionCounter = activeNpc.GetComponent<GoodWillSystem>().npcInteractionCounter;
                GWS.interactions = activeNpc.GetComponent<GoodWillSystem>().interactions;
                //BF.myBiases.Clear();
                for (int i = 0; i < activeNpc.GetComponent<GoodWillSystem>().playerBiasList.Count; i++)
                {
                    BF.myBiases[i] = activeNpc.GetComponent<GoodWillSystem>().playerBiasList[i];
                }
            }
            else
            {
                GWS.playerInteractionCounter = 0;
                GWS.npcInteractionCounter = 0;
                GWS.interactions = 0;
            }
            CS.enabled = false;
            PM.enabled = false;
            

        }


    }
    Dialog dia;
    
    public bool Interact()
    {
        return isInteracting;
    }
    /*if (!overrideShutdown) {
        Debug.DrawRay(raypos, transform.right, Color.red);
        try {
            return Physics.Raycast(raypos, transform.right, out hit, 3.0f) && hit.collider.tag == "NPC";

        } catch (Exception e) {
            return false;
        }
    }else{
      //  Debug.Log(" !!! override !!! " + overrideShutdown);
        //ResetInteraction();
        return false;
    }
}*/

    public bool GrabInteract(string tag)
    {


        Debug.DrawRay(lowerPart.position, transform.right, Color.blue);
        try
        {
            return Physics.Raycast(raypos, transform.right, out hit, 2f) && hit.collider.tag == tag ||
             Physics.Raycast(lowerPart.position, transform.right, out hit, 2f) && hit.collider.tag == tag||
             Physics.Raycast(middlepart, transform.right, out hit, 2f) && hit.collider.tag == tag;

        }
        catch (Exception e)
        {
            return false;
        }


    }

    public bool PcCanInteract()
    {
        if (!overrideShutdown){
            Debug.DrawRay(raypos, transform.right, Color.red);
            Debug.DrawRay(middlepart, transform.right, Color.green);
            Debug.DrawRay(lowerPart.position, transform.right, Color.blue);
            try{
                if (Physics.Raycast(raypos, transform.right, out hit, 2f) ||
                    Physics.Raycast(middlepart, transform.right, out hit, 2f) ||
                    Physics.Raycast(lowerPart.position, transform.right, out hit, 2f)){
                    foreach (string tag in tags){
                        if (hit.collider.tag == tag){
                            temp = hit.collider.transform.GetComponent<Transform>();
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
            catch (Exception e){
                return false;
            }
        }else{
            ResetInteraction();
            return false;
        }
    }


}
