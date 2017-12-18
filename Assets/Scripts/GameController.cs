﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public SpriteRenderer[] enemy;
	public SpriteRenderer[] player;
	public SpriteRenderer[] collision;
	public Image[] lives;
	public bool canPlay;
	public GameObject starButton;
	public bool startGame;
	//public bool gameOver;
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
	}

	void Update(){
		//Lógica de colisión
		if (startGame) {
			//Si el indice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
			for (int i = 0; i < player.Length; i++) {
				if (enemy[i].enabled && player[i].enabled) {
					startGame = false;
					//mostramos la colisión
					collision[i].enabled = true;
					RemoveLife ();
					//INVOCACIÓN TEMPORAL
					Invoke ("RestartGame",3);
				}
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
	//Función para reiniciar el juego
	void RestartGame(){
		SceneManager.LoadScene ("Main");
	}
}
