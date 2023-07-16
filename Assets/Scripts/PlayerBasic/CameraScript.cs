using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraScript : MonoBehaviour
{
   private float mouseX;
	private float mouseY;
	public Vector3 deltaRotation;
	public float mouseSensitivity = 1;
	public Transform playerBody;
	public Rigidbody rb;
	private float xAxisClamp = 0.0f;
	private float yAxisClamp = 0.0f;
	private Vector3 myPos;
	private Vector3 PlayerPos;
	private Vector3 playermodelPos;
	private Vector3 differencePos;
	public Rigidbody grabPos;
	public BoxCollider cap;
	float y=0;
	private PlayerMovement bm;
	public int maxRot = 45;
	public int minRot = -45;
	private Rigidbody grabObj;
	private PlayerInteract PI;
	void Start()
	{
		//rb.GetComponent<Rigidbody>().rotation = Quaternion.identity;
		bm = GetComponentInParent<PlayerMovement>();
		myPos = GetComponent<Transform>().position;
		PlayerPos = rb.GetComponent<Transform>().position;
		playermodelPos = grabPos.GetComponent<Transform>().position;
		differencePos = PlayerPos - myPos;
		y = transform.position.y - playermodelPos.y;
		PI = GetComponentInParent<PlayerInteract>();
	}	
	

	// Update is called once per frame
	void Update()
	{
		differencePos = PlayerPos - myPos;
		rotateCamra();
	//	unlockMouse();
	///	rb.velocity = playermodelRb.velocity;
		//transform.position = myPos- differencePos;//new Vector3(myPos.x - differencePos.x, myPos.y - differencePos.y, myPos.z-differencePos.z);

	}

	//This method does that when the mouse turns the character body rotates with the camra
	void rotateCamra()
	{

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		float rotAmountX = mouseX * mouseSensitivity;
		float rotAmountY = mouseY * mouseSensitivity;

		xAxisClamp -= rotAmountY;
		yAxisClamp -= rotAmountX;

		Vector3 targetRotationCamra = transform.rotation.eulerAngles;
		Vector3 targetRotationBody = rb.rotation.eulerAngles;
		//Vector3 targetRotationPrefab = playermodelRb.rotation.eulerAngles;

		targetRotationCamra.x -= rotAmountY;//invert the input = -=
		targetRotationBody.y += rotAmountX; //rotates the body
		targetRotationCamra.z = 0; // no cam flip
		targetRotationCamra.z = 0;
		//locks the camra rotation's  x coordinat between -90 and 90 degrees 
		// look at the 3D camera degress
		if (xAxisClamp > maxRot)
		{
			xAxisClamp = maxRot;
			targetRotationCamra.x = maxRot;

		}
		else if (xAxisClamp < minRot)
		{

			xAxisClamp = minRot;
			targetRotationCamra.x = minRot;
		}

		
		//Debug.Log(xAxisClamp);
		//targetRotationCamra.x = -5;
		transform.rotation = Quaternion.Euler(targetRotationCamra);
		
		//deltaRotation = Quaternion.Euler(targetRotationBody * Time.deltaTime);
		rb.rotation = Quaternion.Euler(targetRotationBody);
		/*try	{
			if (PI.grabObjectRB != null){
				PI.grabObjectRB.MoveRotation(Quaternion.Euler(targetRotationCamra));
			}
		}catch (Exception e) { 
		}*/
		//grabObjectRB
		//grabPos.rotation = Quaternion.Euler(targetRotationBody);
		//rb.MoveRotation(Quaternion.Euler(targetRotationBody));
		//targetRotationBody.y = targetRotationBody.y;

		//playermodelRb.MoveRotation(Quaternion.Euler(targetRotationCamra));



	}

}
