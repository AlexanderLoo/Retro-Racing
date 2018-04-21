using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BatteryManager : MonoBehaviour {

	private BatteryDisplay batteryDisplay;

	//tiempo de espera en segundos
	public int timeToWait = 900;
	//tiempo total desde la medianoche en segundos
	private int totalTime;
	//variable a alcanzar para obtener una vida
	private int getNewLife;
	//Booleano que controla el estado de carga de vida, esto permite que no se resetee el tiempo de espera si se pierde mas de una vida
	private bool waitingForLife = false;
	//Entero que nos indica cuanto tiempo total nos falta en segundos
	private int timeRemaining;
	//Las siguiente variables indican el tiempo que falta en segundos y en minutos
	private int timeRemainingInSeconds, timeRemainingInMinutes;

	void Awake(){

		batteryDisplay = GetComponent<BatteryDisplay> ();
	}

	void Update(){

		//Asignamos a time la hora completa actual(esto incluye fecha completa y hora)
		DateTime time = DateTime.Now;

		//Las siguientes variables acceden individualmente a cada componente:

		int day = time.Day;
		int hour = time.Hour;
		int minute = time.Minute;
		int seconds = time.Second;

		//Convertimos las variables en segundos:
		hour *= 3600;
		minute *= 60;
		//Obtenemos los segundos totales desde el día anterior(medianoche)
		totalTime = hour + minute + seconds;
		LivesManager ();
	}
	//Función para esperar por una vida
	public void WaitForLife(){
		
		if (!waitingForLife) {
			getNewLife = totalTime + timeToWait;
			waitingForLife = true;
			batteryDisplay.batteryCountDown.SetActive (true);
		}
	}

	//Esta función se ejecuta cada frame, por eso limitamos su función con el booleano 'waitingForLife'
	void LivesManager(){

		if (getNewLife > totalTime) {
			TimeRemaining ();
			batteryDisplay.ShowCountDown (batteryDisplay.minutesText, timeRemainingInMinutes);
			batteryDisplay.ShowCountDown (batteryDisplay.secondsText, timeRemainingInSeconds);
		} else {
			if (waitingForLife) {
				batteryDisplay.AddLife ();
				//La siguiente lógica permite actualizar el conteo de obtención de nueva vida, si es que no tenemos la bateria llena
				for (int i = 0; i < batteryDisplay.livesImage.Length; i++) {
					if (!batteryDisplay.livesImage [i].enabled) {
						getNewLife = totalTime + timeToWait;
						break;
					} else {
						//se vuelve falso sólo si estamos full de batería
						waitingForLife = false;
						batteryDisplay.batteryCountDown.SetActive (false);
					}
				}
			}
		}
	}
	//Esta función transforma los segundos de espera en minutos y segundos de forma las leible
	void TimeRemaining(){

		timeRemaining = getNewLife - totalTime;
		timeRemainingInMinutes = timeRemaining / 60;
		timeRemainingInSeconds = timeRemaining % 60;
	}
}
