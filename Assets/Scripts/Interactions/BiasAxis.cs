using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiasAxis : MonoBehaviour
{
    float[,] DefaultBias = new float[7, 7] { 
    //______________T    riangle |  Cube   | Thick  | Thin    | Blank   | Striped  | Squard |
    /*0 Triangle */{  4, /*  |*/ (-2), /*|*/ 2, /*| */(-2),/* | */ 1,/* | */(-1),  /*|*/(-2) } ,   
    /*1 Cube     */{ (-2), /*  |*/ 4, /* |*/ 2, /*| */(-2),/* | */ 1,/* | */(-1),  /*|*/(-2) } ,
    /*2 Thick */   {  1, /*  |*/ (-1),/* |*/ 4, /*| */(-4),/* | */ 1,/* | */(-1),  /*|*/(-2) } ,
    /*3 Thin */    {  1, /*  |*/ (-1),/* |*/ (-4),/*| */ 4,/* | */ 1,/* | */(-1),  /*|*/(-2) } ,
    /*4 Blank */   {  1, /*  |*/ (-1),/* |*/ 2, /*| */(-2),/* | */ 4,/* | */(-1),  /*|*/(-4) } ,
    /*5 Striped */ {  1, /*  |*/ (-1),/* |*/ 2, /*| */(-2),/* | */(-1),/* | */ 2,  /*|*/(-1) } ,
    /*6 Squard */  {  1, /*  |*/ (-1),/* |*/ 2, /*| */(-2),/* | */(-4),/* | */(-1),  /*|*/ 4 }
    };

    [Range(0, 1)]
    public int myGeoIndex = 0;

    [Range(2, 3)]
    public int myThicknessIndex = 2;

    [Range(4, 6)]
    public int myPatterenIndex = 4;

    public int TriangleBias;

    private float geoBias = 0.0f;
    private float thickBias = 0.0f;
    private float patBias = 0.0f;

    [HideInInspector] public float npcDialogGeoBias = 0.0f;
    [HideInInspector] public float npcDialogThickBias = 0.0f;
    [HideInInspector] public float npcDialogPatBias = 0.0f;

    public float goodFaith = 0.0f;
    public float GeneralBias;
    private string type = "";
    private string npcDialog = "";

    public List<float> myBiases;
    public List<float> myBiases2;

    private Dialog Dia;

    private string respondes = "";

    private List<string> TriangleBiasKeyWords = new List<string> { "pointy", "pointies", "tip", "tips", "cheess","cheese", "stick", "sharp", "gouda",                                                                        "cheesy", "fill up", "square", "I don't want any trouble","filling",
                                                                    "Do you think you own this place" };
    private List<string> ThickBiasKeyWords = new List<string> { "fat", "smell", "stinky", "nerfed the queen", "table", "brie-hind", "chessboard",
                                                                "old doodle", "Check mate" };
    private List<string> PatternBlankBiasKeyWords = new List<string> { "blanky", "reflect", "lac-ghost intolerant", "play", "checkered", "gouda", "blank", "Pwn to C4", "pawn", "squard" };




    private List<string> playerIsNegative = new List<string> { "don't", "Fuck you", "No thank you", " busy ", "my way", "other way", "Naa" };

    private List<string> CubeGeoBiasKeyWords = new List<string> { "pointy", "" };
    private List<string> ThinBiasKeyWords = new List<string> { "cheess", "" };
    private List<string> PatternSquardBiasKeyWords = new List<string> { "chess", "" };

    int geoBiasCounter = 0;
    int geoThicknessCounter = 0;
    int geoPatterenCounter = 0;
    int playerIsNegativeCounter = 0;

    public GameObject firstPlayer;
    public GameObject SecondPlayer;
    public bool FirstPlaythrough = true;
    private static bool first = false;
    public MainScreenController mc;

    public Transform pos1;
    public Transform pos2;
    // Triangle thick blank propeties indexics = 0, 2, 4                                                                                              
    // Start is called before the first frame update
    private static bool setPlayerOnce = false;
    private static bool playerIsSet = false;
    static int randomNr = 0;
    private static bool q = false;
   public Vector3 myBias;
    Vector3 newVal;
    public Transform a;
    void Start()
    {
        myBias = GetGeneralBias(0,0,0);
        transform.position = myBias;
        Debug.Log(myBias);
        newVal = transform.position = a.GetComponent<BiasAxis>().myBias;
        Debug.Log(this.name + "my start mag " + myBias.magnitude);

    }

    public Vector3 GetGeneralBias(float geoBias, float thickBias, float patBias)
    {
        myBiases.Clear();
        myBiases = new List<float>();
        float gBias = 0;
        float tBias = 0;
        float pBias = 0;
        for (int i = 0; i < 7; i++)
        {

            float geometryBias = DefaultBias[myGeoIndex, i];
            float thicknessBias = DefaultBias[myThicknessIndex, i];
            float patterenBias = DefaultBias[myPatterenIndex, i];

            float currentBiasValues = (geometryBias * thicknessBias * patterenBias) / 3.0f;

            myBiases.Insert(i, currentBiasValues);
        }

        // once = false;
        float geoCalc = Mathf.Pow(myBiases[0], 2.0f) - myBiases[1];
        float thickCalc = Mathf.Pow( Mathf.Pow(myBiases[2], 2.0f) - myBiases[3],1/2);
        float patterenCalc = Mathf.Pow(myBiases[4], 3.0f) - myBiases[5] - myBiases[6];
        Debug.Log(geoCalc + " pat: " + patterenCalc);

       // geoCalc = geoCalc<0? (Mathf.Sqrt(geoCalc * -1) * -1) * 2: Mathf.Sqrt(geoCalc);
        
    
        
       // float thickCalc = Mathf.Sqrt((Mathf.Pow(myBiases[2], 2.0f) - myBiases[3]));
        // float patterenCalc = Mathf.Pow(((Mathf.Pow(myBiases[4], 3.0f) - myBiases[5]) - myBiases[6]),1/3) ;
        
        //= ((N6 ^ 3 - N7 - N8) ^ (1 / 3)) * 2
        //  Debug.LogError(this.name + " geoCalc " + geoCalc + " thickCalc " + thickCalc + " patterenCalc " + patterenCalc + " abc");
        //float biasCalc = Mathf.Pow(geoCalc + thickCalc + patterenCalc, 2);


        // Debug.Log( this.name + "&&&&" + IamUpdating);
        return new Vector3(geoCalc, thickCalc, patterenCalc);


        //return   Mathf.Pow( (Mathf.Pow(myBiases[0], 2.0f) - myBiases[1]) +(Mathf.Pow(myBiases[2], 2.0f) - myBiases[3]) + (Mathf.Pow(myBiases[4], 2.0f) - myBiases[5] - myBiases[6]) , 1.0f / 3.0f);


        // return  Math.Pow( Math.Pow( Math.Pow(myBiases[0], 2.0) - myBiases[1]) + (Math.Pow(myBiases[2], 2.0) - myBiases[3]) + (Math.Pow(myBiases[4], 2.0) - myBiases[5] - myBiases[6]),2.0), 1.0f/3.0);
    }
    int IamUpdating = 0;
    public float getGroupBias(int index)
    {

        return 0;
    }
    int updateCounter = 0;

    public float updateMyBias(float geoBias, float thickBias, float patBias, float gf = 0)
    {

        updateCounter++;
        List<float> updateBias = new List<float>();

        float currentBiasValues = 0.0f;
        float currentBias = 0;
        Debug.Log(this.name + "interact gf= " + gf);


        for (int i = 0; i < 7; i++)
        {

            float geometryBias = DefaultBias[myGeoIndex, i] + geoBias + gf;
            float thicknessBias = DefaultBias[myThicknessIndex, i] + thickBias + gf;
            float patterenBias = DefaultBias[myPatterenIndex, i] + patBias + gf;

            if (this.tag == "Player")
            {
                geometryBias = geometryBias - getNpcBiasFromDialog(TriangleBiasKeyWords, geoBiasCounter);
                thicknessBias = thicknessBias - getNpcBiasFromDialog(ThickBiasKeyWords, geoThicknessCounter);
                patterenBias = patterenBias - getNpcBiasFromDialog(PatternBlankBiasKeyWords, geoPatterenCounter);
                currentBias = AvoidZeroMultiplication(geometryBias, thicknessBias, patterenBias);

            }
            else
            {
                currentBias = AvoidZeroMultiplication(geometryBias, thicknessBias, patterenBias) -
                                    getNpcBiasFromDialog(playerIsNegative, playerIsNegativeCounter);
            }

            if (currentBias < 0)
            {
                currentBiasValues = (Mathf.Pow((currentBias * -1), 1.0f / 3.0f) * -1);
            }
            else
            {
                currentBiasValues = Mathf.Pow((currentBias), 1.0f / 3.0f);
            }
            updateBias.Insert(i, currentBiasValues);
        }
        geoBiasCounter = 0;
        geoThicknessCounter = 0;
        geoPatterenCounter = 0;
        playerIsNegativeCounter = 0;

        goodFaith = 0;
        once = false;
        float geoCalc = Mathf.Pow(updateBias[0], 2.0f) - updateBias[1];
        float thickCalc = Mathf.Pow(updateBias[2], 2.0f) - updateBias[3];
        float patterenCalc = Mathf.Pow(updateBias[4], 3.0f) - updateBias[5] - updateBias[6];
        float biasCalc = Mathf.Pow(geoCalc + thickCalc + patterenCalc, 2);

        if (biasCalc < 0)
        {
            biasCalc = Mathf.Pow(biasCalc * -1, 1.0f / 3.0f);
        }
        else
        {

            biasCalc = Mathf.Pow(biasCalc, 1.0f / 3.0f);
        }
        return biasCalc;
    }
    public string abc = "";
    bool once = false;
    public bool AAA = false;
    public float myMag = 0;
    // Update is called once per frame
    void Update()
    {
        if (!AAA)
        {
            transform.position = newVal - myBias;
            myMag = transform.position.magnitude;
            Debug.Log(this.name +" my mag: " + myMag);
        }
        else
        {
            transform.position = newVal + myBias;
            myMag = transform.position.magnitude;
            Debug.Log(this.name + " my mag: " + myMag);
        }
        if (this.name.Contains("Active"))
        {
            //Debug.LogError(this.name);
            if (getRespondsType() == "positive")
            {
                goodFaith = 0.55f;
            }
            else if (getRespondsType() == "negative")
            {
                goodFaith = -1.25f;
            }
            else if (getRespondsType() == "neutral")
            {

                goodFaith = 0.25f;

            }
            else
            {
                goodFaith = 0;
            }
        }
        if (this.name == "ActiveNpc")
        {
            // Debug.LogError("goodfaith:" + goodFaith);
        }
        if (goodFaith != 0 && !once)
        {

            // Debug.Log(this.name + " good faith = " + goodFaith + GetGeneralBias(geoBias, thickBias, patBias));
            once = true;
        }
    }

    public void updatePlayerBias()
    {
        goodFaith = 0;

       // GeneralBias = GetGeneralBias(geoBias, thickBias, patBias);

    }

    void response()
    {
        type = getRespondsType();
        respondes = getResponds();
    }
    float AvoidZeroMultiplication(float x, float y, float z)
    {
        int counter = 0;
        if (x == 0)
        {

            x = 1.0f;
            // Debug.Log("x is zero");
        }
        else if (y == 0)
        {

            y = 1.0f;
            // Debug.Log("y is zero");

        }
        else if (z == 0)
        {

            z = 1.0f;
            // Debug.Log("z is zero");
        }
        return (x * y * z);
    }
    int getNpcBiasFromDialog(List<string> keywords, int counter)
    {
        // type  if type is negative always provide a negative reponse
        foreach (string word in keywords)
        {
            if (respondes.Contains(word))
            {
                counter++;
                // Debug.Log(word + " counter: " + counter);
            }
        }
        if (counter > 2)
        {
            counter = 2;
        }
        return counter;
    }

    public void setResponds(string _type, string dialog)
    {
        npcDialog = dialog;
        type = _type;
    }
    public void setType(string _type)
    {

    }
    public string getRespondsType()
    {
        return type;
    }
    public string getResponds()
    {
        return npcDialog;
    }
}
