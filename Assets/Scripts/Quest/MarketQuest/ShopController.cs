using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    // Start is called before the first frame update
   public List<Transform> myStartObjects;
    List<Vector3> objectPositions;
    List<Quaternion> objectRotations;
    public string MyHats;
   public List<Transform> currentObjects;
    public bool MyShopIsComplet = false;
    public Transform myReset;
    void Start()
    {
        currentObjects = new List<Transform>();
        objectPositions = new List<Vector3>();
        objectRotations = new List<Quaternion>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "ShopObject") {
            
        
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ShopObject") {
            currentObjects.Insert(currentObjects.Count, other.transform);
        }

    }
    int allMyHats = 0;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ShopObject") {
            for (int i = 0; i < currentObjects.Count; i++) {
                if (currentObjects[i].name == other.transform.name) {
                    currentObjects.RemoveAt(i);
                }
            }
            
        }
        if (other.tag == "Player" ) {
            
            for (int i =0; i<currentObjects.Count; i++) {
                if (currentObjects[i].name.Contains(MyHats)) {
                    allMyHats++;
                    Debug.LogError(currentObjects[i].name + allMyHats);
                }
            }
            Debug.LogError(allMyHats);
            if (allMyHats == 3){
                MyShopIsComplet = true;
            }else {
                allMyHats = 0;
            }
        }
        
    }
   
    // Update is called once per frame
    void Update()
    {
        if (myStartObjects[0]==null && !MyShopIsComplet) {
           // myStartObjects.RemoveAt(0);
            setStartObjects();
        }
        if (MyShopIsComplet) {
            Debug.Log(this.name + " I am done ");
            //Destroy(myReset.gameObject);
            //myReset = null;
        }
    }
    void setStartObjects() {
        for (int i = 0; i < currentObjects.Count; i++){
            myStartObjects.Insert(i, currentObjects[i]);
            objectPositions.Insert(i, currentObjects[i].position);
            objectRotations.Insert(i, currentObjects[i].rotation);
        }
    }
    public void ResetObjects(){

       
            for (int i = 0; i < myStartObjects.Count; i++) {
                myStartObjects[i].position = objectPositions[i];
                myStartObjects[i].rotation = objectRotations[i];
            }
        
        
    }
}
