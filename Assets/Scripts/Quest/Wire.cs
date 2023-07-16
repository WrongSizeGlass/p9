using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private Color image;
    private LineRenderer lr;
    private Canvas canvas;
    private bool isDragStarted = false;
    RectTransform rt;
    Vector2 start;
    public bool isLeft = false;
    private WireTask wt;
    public Color myColor;
    private bool isSuccess;
    Camera cam;
    void Start()
    {
        image = GetComponent<Color>();
        lr = GetComponent<LineRenderer>();
        canvas = GetComponentInParent<Canvas>();
        rt = GetComponent<RectTransform>();
        wt = GetComponentInParent<WireTask>();
        cam = Camera.main.GetComponent<Camera>();
    }
  
    // Update is called once per frame
    void Update()
    {
        if (isDragStarted)
        {
            Vector2 movePos;
            Vector3 point;
           
            Vector3 pos = new Vector3(start.x, start.y, 0);
            //Vector3 pos = new Vector3(transform.position.x, Input.mousePosition.y, 0);
            //Transform cam = Camera.main.transform;

            //  mouse.y = mouse.y ;
            //cam.position = pos;
            //lr.SetPosition(0,pos);
            movePos = Input.mousePosition;
            
            lr.SetPosition(0, pos);
            point = cam.ScreenToWorldPoint(new Vector3(movePos.x, movePos.y, cam.nearClipPlane));
            // lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            /* if (movePos.y<0) {

             }*/
            /*if (movePos.y<start.y) {
                movePos.y = movePos.y - 400;
            }*/
            // movePos.y = movePos.y - 400;
            lr.SetPosition(1, point);
            // Debug.Log("start pos " + pos + "movepos " + movePos);
            Debug.Log("movePos " + movePos + " mousepos " + Input.mousePosition);
        }
        else {
            /*if (!isSuccess) {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position);
            }*/
        }
        Vector3 mypos = transform.position;
        bool isHovered = Input.mousePosition == mypos;
        if (isHovered){
            wt.currentHovedWire = this;
        }
        if (isDragStarted) { Debug.Log("is draggin"); }
       // if (Input.mousePosition == rt.position) { Debug.Log("Input.mousePosition" + Input.mousePosition + " RT " + rt.position); }
    }
    public void setColor(Color color){
        image = color;
        lr.startColor = color;
        lr.endColor = color;
        myColor = color;

    }
    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLeft && !isSuccess){
            isDragStarted = true;
            wt.currentDraggedWire = this;
            start = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }else {
            isDragStarted = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (wt. currentHovedWire != null) {
            if ( wt.currentHovedWire.myColor == myColor) {
                isSuccess = true;
            }
        }
        isDragStarted = false;
        wt.currentDraggedWire = null;
    }
}
