using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public SpriteRenderer[] enemy;
	public SpriteRenderer[] player;
	public Image[] lives;
	public bool inGame;
	public int currentLife;

	void Awake(){
		//Singleton
		if (gameController == null) {
			gameController = this;
		} else if (gameController != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		//EN ESPERA DE SUGERENCIAS PARA MEJORAR LA LÓGICA
		if (PlayerPrefs.GetInt ("FirstTime") == 0) {
			PlayerPrefs.SetInt ("FirstTime", 1);
			currentLife = 3;
			PlayerPrefs.SetInt ("CurrentLife", currentLife);
		} else {
			currentLife = PlayerPrefs.GetInt ("CurrentLife");
		}
		ShowCurrentLives ();
		if (currentLife > 0) {
			inGame = true;
		} else {
			currentLife = 0;
			inGame = false;
		}
	}

	void Update(){
		
		if (inGame) {
			//Si el indice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
			for (int i = 0; i < player.Length; i++) {
				if (enemy[i].enabled && player[i].enabled) {
					inGame = false;
					PauseGame (0);
					RemoveLife ();
				}
			}
		}
	}
	//Función para pausar el juego, el parámetro timeScale 0: para pausar, 1: para continuar el juego
	public void PauseGame(int timeScale){

		Time.timeScale = timeScale;
	}
	//Función para remover una vida
	void RemoveLife(){

		currentLife -= 1;
		PlayerPrefs.SetInt ("CurrentLife", currentLife);
		for (int i = 0; i < lives.Length; i++) {
			if (lives[i].enabled) {
				lives [i].enabled = false;
				break;
			}
		}
	}
	//Función para mostrar las vidas disponibles al iniciar el juego
	void ShowCurrentLives(){

		currentLife = PlayerPrefs.GetInt ("CurrentLife");
		for (int i = currentLife -1; i > -1; i--) {
			lives [i].enabled = true;
		}
	}
}
