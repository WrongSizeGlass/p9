using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
   // public GameObject InteractCanvas;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 5)] public float speed = 5;

   /* [Header("Sound Files")]
    [Tooltip("for sound")]

    public AudioClip QuickJumpSound;
    public AudioClip LongJumpSound;
    public AudioClip GrabbingSound;
    public AudioClip LandingSound;
    private AudioSource audio;*/

    [HideInInspector] public RaycastHit hit;

    private BoxCollider BodyCollider;

    private Vector3 playerPos;
    private Vector3 downDirection;
    private Vector3 grabDirection;
    private Vector3 raypos;
    bool landingBool;

    private float downDisRange;
    private float downDis = 0.1f;
    private float jump = 0;
    private float vertical = 0;
    private float horizontal = 0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    private float crouchSpeed = 1f;
    private float grabY;
    private float startWeight;
    private float weight;
    private float startJumpSpeed = 0f;
    private float JumpBarUnits = 0.000f;
    private float velY = 1;

    private bool forward;
    private bool backward;
    public bool movement;

    private int counter = 0;
    bool active = true;
    AudioSource sound;
    public AudioClip walk1;
    public AudioClip walk2;
    // Start is called before the first frame update
    private void Awake()
    {
        if (active)
        {
            gameObject.name = "ActivePlayer";
        }

    }
    void Start()
    {
        sound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        //audio = GetComponent<AudioSource>();
        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;
        BodyCollider = GetComponent<BoxCollider>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        crouchSpeed = startSpeed / 2;
        startWeight = rb.mass;
        weight = startWeight;
        horiMax = startSpeed * 1.6f;
    }
    Vector3 pos;
    int walkCounter = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
       


        forward = Input.GetAxis("Vertical") > 0;
        backward = Input.GetAxis("Vertical") < 0;
        movement = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        raypos = BodyCollider.transform.position;
        // raypos.y = raypos.y + 0.32f;// 927692
        // crouching = pas.crouch;
        pos = rb.position;
       
        playerPos = pos;

        move();
        if (!onSurface())
        {
           // rb.useGravity = true;
        }
        else { //rb.useGravity = false; 
        }
        if (movement){
            if ((counter) % Mathf.Round(0.1f / Time.fixedDeltaTime) == 0)
            {
                isWalking = true;

            }else {
                isWalking = false;
            }
        }


    }

    void move()
    {
        vertical = Input.GetAxis("Vertical") * runSpeed();

        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");

            if (!rayHit())
            {
               
                rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime);
            if (!sound.isPlaying && vertical != 0)
            {
                playWalkingAudio();
            }

        }
            else if (rayHit() && !forward)
            {
                
                 rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime);
                if (!sound.isPlaying && horizontal != 0)
                {
                     playWalkingAudio();
                }

        }
            else
            {
                if (speed > 0) { speed--; }
            }
    }
    bool start = false;
    bool isWalking;
   public bool Playedwalk1 = false;
   public bool Playedwalk2 = true;
    void playWalkingAudio() {

        if (!sound.isPlaying && !start && movement && !Playedwalk1 && Playedwalk2){
            Playedwalk1 = true;
            Playedwalk2 = false;
            start = true;
            sound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            sound.volume = UnityEngine.Random.Range(0.55f, 0.65f);
            sound.PlayOneShot(walk1);
            sound.Play();

        }else if (!sound.isPlaying && !start && movement && Playedwalk1 && !Playedwalk2){
            Playedwalk1 = false;
            Playedwalk2 = true;
            start = true;
            sound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            sound.volume = UnityEngine.Random.Range(0.45f, 0.55f);
            sound.PlayOneShot(walk2);
            sound.Play();


        }
        else{
            if (!sound.isPlaying) {
                start = false;
            }
        }      
    }

    float horiMax;
    public bool prevent = false;
    float diffMaxSpeed;
    private float runSpeed()
    {
        diffMaxSpeed = maxSpeed;
        speed = Mathf.Clamp(speed, startSpeed, diffMaxSpeed);
        return speed;
    }

    public bool rayHit()
    {
        //Debug.DrawRay(raypos,transform.right);
        try
        {
            if (Physics.Raycast(raypos,transform.right, out hit, 0.75f) && hit.collider.attachedRigidbody)
            {
                return false;
            }
            else
            {
                return Physics.Raycast(raypos, transform.right, out hit, 0.75f);
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    /* public bool rayHitStop()
     {
         Debug.DrawRay(raypos, Camera.main.transform.forward);
         try
         {
             if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody)
             {
                 return false;
             }
             else
             {
                 return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
             }
         }
         catch (Exception e)
         {
             return false;
         }
     }*/

    public bool onSurface()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, downDirection, downDisRange);

    }
    public bool objectInfront()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, Vector3.forward, 0.1f);

    }
}
