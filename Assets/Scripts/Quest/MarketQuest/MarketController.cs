using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketController : MonoBehaviour
{
    public List<ShopController> SC;
    public bool questIsComplete;
    public List<NPCDialog> nd;
    // Start is called before the first frame update
    void Start()
    {
        nd[0].IcanGiveTips = true;
        nd[1].IcanGiveTips = true;
        nd[2].IcanGiveTips = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        questIsComplete = SC[0].MyShopIsComplet && SC[1].MyShopIsComplet && SC[2].MyShopIsComplet;
        if (questIsComplete) {
            nd[0].IcanGiveTips = !SC[0].MyShopIsComplet;
            nd[1].IcanGiveTips = !SC[1].MyShopIsComplet;
            nd[2].IcanGiveTips = !SC[2].MyShopIsComplet;           
        }
        if (SC[0].MyShopIsComplet) {
            nd[0].myTips = "Thanks now I have them all";
        }
        if (SC[1].MyShopIsComplet)
        {
            nd[1].myTips = "Thanks now I have them all";
        }
        if (SC[2].MyShopIsComplet)
        {
            nd[2].myTips = "Thanks now I have them all";
        }
    }
}
