using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTask : MonoBehaviour
{
    public List<Color> wireColors = new List<Color>();
    public List<Wire> leftWires = new List<Wire>();
    public List<Wire> rightWires = new List<Wire>();

    private List<Color> avaliableColors;
    private List<int> avaliableLeftWiresIndex;
    private List<int> avaliableRightWiresIndex;
    public Wire currentDraggedWire;
    public Wire currentHovedWire;
    // Start is called before the first frame update
    void Start()
    {
        avaliableColors = new List<Color>(wireColors);
        avaliableLeftWiresIndex = new List<int>();
        avaliableRightWiresIndex = new List<int>();
     
        for (int i =0; i< leftWires.Count; i++) { avaliableLeftWiresIndex.Add(i); }
        for (int i =0; i< rightWires.Count; i++) { avaliableRightWiresIndex.Add(i); }
        while (avaliableColors.Count > 0 && avaliableLeftWiresIndex.Count>0 && avaliableRightWiresIndex.Count>0) {
            Color pickedColor = avaliableColors[Random.Range(0, avaliableColors.Count)];
            int pickedLeftWireIndex = Random.Range(0, avaliableLeftWiresIndex.Count);
            int pickedRightWireIndex = Random.Range(0, avaliableRightWiresIndex.Count);

            leftWires[avaliableLeftWiresIndex[pickedLeftWireIndex]].setColor(pickedColor);
            rightWires[avaliableRightWiresIndex[pickedRightWireIndex]].setColor(pickedColor);

            avaliableColors.Remove(pickedColor);
            avaliableLeftWiresIndex.RemoveAt(pickedLeftWireIndex);
            avaliableRightWiresIndex.RemoveAt(pickedRightWireIndex);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
