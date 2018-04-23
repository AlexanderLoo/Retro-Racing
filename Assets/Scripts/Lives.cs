using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour {

	public int maxLife=3;

	void Awake(){

		if (PlayerPrefs.GetInt ("FirstTime") == 0) {
			PlayerPrefs.SetInt ("FirstTime", 1);
			SetLivesFirstTime ();
		}
	}

	//Esta función se usa la primera vez que se entra al juego
	public void SetLivesFirstTime(){

		SetCurrentLives (maxLife);
	}

	//Función para agregar o remover vida(dependiendo del parámetro; si es positivo se agrega una vida, si es negativo se remueve)
	public void LivesManager(int i){

		SetCurrentLives (GetCurrentLives() + i);
	}

	//Guarda en la memoria la cantidad de vida que nos queda donde i es el valor a guardar
	public void SetCurrentLives(int i){

		PlayerPrefs.SetInt ("CurrentLife", i);
	}
	//Retorna la cantidad de vida guardada en la memoria
	public int GetCurrentLives(){

		return PlayerPrefs.GetInt ("CurrentLife");
	}
}
