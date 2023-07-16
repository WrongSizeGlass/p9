using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainScreenController : MonoBehaviour
{
    public List<GoodWillSystem> gws;
    public PlayerInteract PI;
    public Dialog D;
    public List<insideCube> IC;

    public Canvas PauseCanvas;
    public Canvas EndCanvas;
    public RawImage bg;
    public RawImage check1;
    public RawImage check2;
    public RawImage check3;
    public Button continueBTN;
    public Text exitBTN;
    public List<Text> collections;
    public List<NPCDialog> nd;
    public Text PT;
    public Image PTbg;
    public Text continueQuist;
    int counter = 0;
    int Ecounter = 0;
    public bool pause = false;
    public bool exit = false;

    public static bool first = false;

    static int endGameCounter = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate(){
        counter++;
        if (!exit)
        {
            pauseGame();

        }

    }
    private void Update()
    {
        OnAndOff();
        endGame();
        Debug.Log("end game: " + (gws[0].questCompleted && gws[1].questCompleted && gws[2].questCompleted && !PI.PcCanInteract()));
        
    }




    void pauseGame() {
        //if (Input.GetKeyDown(KeyCode.Escape)){
        if (Input.GetKey(KeyCode.Escape)){
            if ((counter) % Mathf.Round(0.125f / Time.fixedDeltaTime) == 0){
                Ecounter++;
            }

        }
        pause = Ecounter % 2 == 1;
    }
    void OnAndOff() {
        PauseCanvas.enabled = pause;
        bg.enabled = pause;
        PT.enabled = pause;
        PTbg.enabled = pause;
        
        check1.enabled = pause && gws[1].questCompleted;       
        check2.enabled = pause && gws[2].questCompleted;
        check3.enabled = pause && gws[0].questCompleted;
        Time.timeScale = Ecounter % 2 == 1 ? 0 : 1;
    }
    void endGame() {
        if (gws[0].questCompleted && gws[1].questCompleted && gws[2].questCompleted && !PI.PcCanInteract())
         //   && Vector3.Distance(gws[0].transform.position,PI.transform.position)>3.0f && Vector3.Distance(gws[1].transform.position, PI.transform.position) > 3.0f && Vector3.Distance(gws[2].transform.position, PI.transform.position) > 3.0f)
        {

            exit = true;
            EndCanvas.enabled = true;
            D.enabled = false;
            first = true;
            Cursor.lockState = CursorLockMode.None;
            collections[2].text = "You got a Pet Rock: " + nd[2].petRock.gameObject.active;
            collections[1].text = "You got a Leash   : " + nd[1].leash;
            collections[0].text = "You got a Hat     : " + PI.TurnOnRewardHat;

            if (endGameCounter == 1)
            {
                continueQuist.text = "Thank you for playing \n you may now exit the game \n and return to the Questionaire";
                exitBTN.enabled = true;
                //exitBTN.transform.position = continueBTN.transform.position;
                //Destroy(continueBTN);

            }
        }
    }
    private static int randomNr = 9;
    private static bool setGameOnce = false;

    private static bool setGame() {
        if (!setGameOnce){
            setGameOnce = true;
            randomNr = Random.Range(1, 3);
            Debug.Log("first " + first );
            first = randomNr % 2 == 1;
            return first;
            
        } else {
            Debug.Log("first else" + first);
            if (first){
                first = !first;
                Debug.Log(" first is true now false " + first);
                return first;
            }else {
                first = !first;
                Debug.Log(" first is false now true " + first);
                return first;
            }
        }
        
       // Debug.Log("first: " + first + " random nr: " + randomNr + " setgame " + setGameOnce);
    }


    public bool getFirst() {
        
        return setGame();
    }
    public void resume() {
        Ecounter++;
        Debug.LogError(Ecounter);
       // pause = false;
    }

    public void newGame() {
        // SceneManager.LoadSceneAsync(1);
        if (exit) {
            endGameCounter++;
            if (endGameCounter % 2 == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } else {
                exitGame();
            }
        }
    }
    public void exitGame() {
        if (exit) {
            Application.Quit();
            Debug.LogError("Exit the game");
        }
    }


}
