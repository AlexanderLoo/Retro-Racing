﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	//Lista de los sprites de enemigos que estan en la misma fila que el player
	public SpriteRenderer[] enemyCollision;
	public SpriteRenderer[] player;
	//Lista de sprites del choque
	public SpriteRenderer[] collision;
	public Image[] lives;
	//Velocidad 1 que equivale a 16.6 m/s
	public float speed = 1;
	public float score;
	public Text scoreText;
	[HideInInspector]
	public bool canPlay;
	public GameObject starButton;
	[HideInInspector]
	public bool startGame;
	//public bool gameOver;
	[HideInInspector]
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
		if (currentLife <= 0) {
			canPlay = false;
		} else {
			canPlay = true;
			starButton.SetActive (true);
		}
		speed = 1;
		score = 0;
		scoreText.text = score.ToString();
		Time.timeScale = 0;
	}

	void Update(){

		if (startGame) {
			CarCollision ();
			Distance ();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}
	}
	//Función para mostrar las vidas disponibles al iniciar el juego
	void ShowCurrentLives(){

		currentLife = PlayerPrefs.GetInt ("CurrentLife");

		for (int i = lives.Length -1; i > lives.Length - 1 - currentLife ; i--) {
			lives [i].enabled = true;
		}
	}

	//Lógica de colisión
	void CarCollision(){
		//Si el índice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
		for (int i = 0; i < player.Length; i++) {
			if (enemyCollision[i].enabled && player[i].enabled) {
				startGame = false;
				//mostramos la colisión
				collision[i].enabled = true;
				//removemos la vida
				RemoveLife ();
				Time.timeScale = 0;
			}
		}
	}

	//Función para remover una vida
	void RemoveLife(){

		currentLife -= 1;
		PlayerPrefs.SetInt ("CurrentLife", currentLife);
		for (int i = 0; i < lives.Length; i++) {
			if (lives[i].enabled) {
				//lives [i].enabled = false;
				lives[i].GetComponent<Animator>().enabled = true;
				break;
			}
		}
	}

	//Función para calcular la distancia en km y mostrarlas en el score
	void Distance(){
		//Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(km/h * 0.277)
		//Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
		//distance = (16.6 / speed) * time ---> distance = (16.6 / speed) * Time.time(distancia en metros, distancia en km --> score/1000)
		//dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

		float distance = (float)(16.6 /speed) * Time.timeSinceLevelLoad;
		score = distance;
		scoreText.text = Mathf.Round (score).ToString ();
	}

	//Función para reiniciar el juego
	void RestartGame(){
		SceneManager.LoadScene ("Main");
	}

}
