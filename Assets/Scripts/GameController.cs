using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public BatteryDisplay batteryDisplay;
	public BatteryManager batteryManager;
	public Lives lives;
	private ScoreDisplay scoreDisplay;
	//Lista de los sprites de enemigos que estan en la misma fila que el player
	public SpriteRenderer[] enemyCollision;
	public SpriteRenderer[] player;
	//Lista de sprites del choque
	public SpriteRenderer[] collision;
	//Velocidad 1 que equivale a 16.6 m/s
	public float speed = 1;
	public GameObject startButton;
	[HideInInspector]
	public bool startGame;

	void Awake(){
		//Singleton
		if (gameController == null) {
			gameController = this;
		} else if (gameController != this) {
			Destroy (gameObject);
		}
		scoreDisplay = GetComponent<ScoreDisplay> ();
	}

	void Start(){
		
		batteryDisplay.ShowCurrentLives ();
		if (lives.GetCurrentLives() <= 0) {
			CanPlay (false);
		} else {
			CanPlay (true);
		}
		speed = 1;
		Time.timeScale = 0;
	}
	//Función que se llama cuando salimos del juego
	void OnDisable(){

		batteryManager.SaveLastTotalTime ();
		batteryManager.SaveLastTimeRemaining ();
	}

	void Update(){

		if (startGame) {
			CarCollision ();
			scoreDisplay.Distance ();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}
	}

	public void CanPlay(bool b){

		startButton.SetActive (b);
	}

	//Lógica de colisión
	void CarCollision(){
		//Si el índice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
		for (int i = 0; i < player.Length; i++) {
			if (enemyCollision[i].enabled && player[i].enabled) {
				startGame = false;
				//mostramos la colisión
				collision[i].enabled = true;
				//removemos una vida
				batteryDisplay.RemoveLife ();
				Time.timeScale = 0;
			}
		}
	}

	//Función para reiniciar el juego
	void RestartGame(){
		SceneManager.LoadScene ("Main");
	}
}
