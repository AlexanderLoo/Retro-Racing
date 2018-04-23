using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	private Animator anim;
	public GameObject pauseButton;

	void Awake(){
		anim = GetComponent<Animator> ();
	}

	public void PauseGame(){

		Time.timeScale = 0;
		GameController.gameController.startGame = false;
	}

	//Función para iniciar el conteo de inico de juego
	public void ReadyToPlay(){

		if (!GameController.gameController.startGame) {
			anim.SetTrigger ("Start");
		}
	}
	//Al terminar la animación del conteo, comenzamos el juego(esta función se llama en el evento al terminar la animación de countdown)
	public void StartTheGame(){

		Time.timeScale = 1;
		GameController.gameController.startGame = true;
		//Al estar en pleno juego mostramos el botón de pausa
		pauseButton.SetActive (true);
	}
}
