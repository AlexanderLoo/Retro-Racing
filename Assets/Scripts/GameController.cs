using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public SpriteRenderer[] enemy;
	public SpriteRenderer[] player;
	public bool inGame;

	void Awake(){
		//Singleton
		if (gameController == null) {
			gameController = this;
		} else if (gameController != this) {
			Destroy (gameObject);
		}
	}

	void Update(){
		//Si el indice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
		for (int i = 0; i < player.Length; i++) {
			if (enemy[i].enabled && player[i].enabled) {
				inGame = false;
				Time.timeScale = 0;
			}
		}
	}

	//FUNCIÓN TEMPORAL PARA TESTING
	public void RestartGame(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("Main");
	}
}
