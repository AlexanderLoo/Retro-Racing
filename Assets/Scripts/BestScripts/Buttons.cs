using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/{

	public GameObject startButton;
	private bool leftPressed, rightPressed;
	private bool start = false;

	public bool StartPressed(){

		return start;
	}

	public bool PausePressed(){

		return false;
	}

	public bool Left(){
		
		return leftPressed;
	}

	public bool Right(){

		return rightPressed;
	}

	public void ButtonPressed(){

		start = !start;
	}
	//Función para controllador usando teclas
	public void KeysController(){

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			leftPressed = true;
		} else {
			leftPressed = false;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			rightPressed = true;
		} else {
			rightPressed = false;
		}
	}

//	void OnPointerDown(PointerEventData ped){
//		return;
//	}
//
//	void OnPointerUp(PointerEventData ped){
//		return;
//	}
}
