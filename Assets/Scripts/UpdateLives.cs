using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateLives : MonoBehaviour {

	/*********************************************************** PLACE HOLDERS *******************************************************************/

	//Función que Guarda en la memoria la cantidad de vida que nos queda, usar como único parametro un entero como el valor a guardar
	//GameController.gameController.SetCurrentLives ();

	//Función para obetener las vidas guardadas en la memoria, la función retorna un entero(no requiere parametro)
	//GameController.gameController.GetCurrentLives();



	void Start(){


	}

	void Update(){

		//Asignamos a time la hora completa actual(esto incluye fecha completa y hora)
		DateTime time = DateTime.Now;

		//Las siguientes variables acceden individualmente a cada componente:

		int day = time.Day;
		int hour = time.Hour;
		int minute = time.Minute;
		int seconds = time.Second;

		//print(minute);
	}
}
