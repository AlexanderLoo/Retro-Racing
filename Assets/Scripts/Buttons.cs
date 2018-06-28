using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/{

	public GameObject startButton, playButton, pauseButton;
	private bool leftPressed = false; 
	private bool rightPressed = false;
	private bool start = false;
	private bool pause = false;

	public bool StartPressed(){

		return start;
	}

	public bool PausePressed(){

		return pause;
	}

	public bool Left(){
		
		return leftPressed;
	}

	public bool Right(){

		return rightPressed;
	}
	//TEST
	public void SetStart(bool value){
		start = value;
	}

	public void SetPause(bool value){
		pause = value;
	}
	//TEST
	public void SetLeft(bool value){
		leftPressed = value;
	}

	public void SetRight(bool value){
		rightPressed = value;
	}

	public void Show(GameObject button, bool value){

		button.SetActive (value);
	}
    //Si presionamos el botón de atrás del celular
    public void Back(){

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
	//TEST
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
