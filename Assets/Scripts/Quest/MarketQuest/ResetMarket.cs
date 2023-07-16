using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMarket : MonoBehaviour
{
    public Transform shop;
    private ShopController sp;
    public List<Transform> myStartObjects;
    private List<Transform> objectPositions;
  

    private void Start()
    {
        sp = shop.GetComponent<ShopController>();
        objectPositions = new List<Transform>();
        for (int i = 0; i < myStartObjects.Count; i++)
        {
            objectPositions.Insert(i, myStartObjects[i]);
           
        }
        Debug.Log(objectPositions.Count);
    }
    // Start is called before the first frame update
    public void ResetShop(){
        sp.ResetObjects();

    }
}
