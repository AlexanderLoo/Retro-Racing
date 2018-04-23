using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BatteryManager : MonoBehaviour {

	private BatteryDisplay batteryDisplay;
	private Lives lives;

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
		lives = GetComponent<Lives> ();
		UpdateRemainingTime ();
	}

	void Update(){

		GetTotalTime ();
		GetLivesController ();
	}

	void GetTotalTime(){

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
	}
	//Función para esperar por una vida
	public void WaitForLife(int time){
		
		if (!waitingForLife) {
			getNewLife = totalTime + time;
			waitingForLife = true;
			batteryDisplay.batteryCountDown.SetActive (true);
		}
	}
	//Función que se llama cuando tenemos la batería llena, esto corta el conteo de espera de próxima vida
	public void BatteryFilled(){

		//se vuelve falso por que no esperamos mas vidas
		waitingForLife = false;
		batteryDisplay.batteryCountDown.SetActive (false);
	}

	//Esta función se ejecuta cada frame, por eso limitamos su función con el booleano 'waitingForLife'
	void GetLivesController(){

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
						BatteryFilled ();
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

	//función para guardar el último momento que se jugo
	public void SaveLastTotalTime(){

		PlayerPrefs.SetInt ("LastTotalTime", totalTime);
	}

	int GetLastTotalTime(){

		return PlayerPrefs.GetInt("LastTotalTime");
	}

	public void SaveLastTimeRemaining(){

		PlayerPrefs.SetInt ("LastTimeRemaining",timeRemaining);
	}

	int GetLastTimeRemaining(){

		return PlayerPrefs.GetInt ("LastTimeRemaining");
	}
	//Función que se llama al inico del juego para obtener las vidas ganadas fuera del juego
	void UpdateRemainingTime(){
		//Si no tenemos llena las vidas cuando buscamos en la memoria...
		if (lives.GetCurrentLives() != lives.maxLife) {
			GetTotalTime ();
			//Comprobamos si se paso el tiempo total para que se llenen todas nuestras vidas
			if (totalTime - GetLastTotalTime () >= (lives.maxLife - lives.GetCurrentLives () - 1) * timeToWait + GetLastTimeRemaining ()) {
				//en ese caso llenamos todas nuestras vidas al máximo
				lives.SetCurrentLives (lives.maxLife);
			} else {
				//caso contrario obtenemos el tiempo que nos falta para llenarlas..
				int totalTimeRemaining = ((lives.maxLife - lives.GetCurrentLives () - 1) * timeToWait + GetLastTimeRemaining ()) - (totalTime - GetLastTotalTime ());
				//Establecemos la cantidad de vida que hemos ganado
				lives.SetCurrentLives ((totalTime - GetLastTotalTime())/timeToWait + lives.GetCurrentLives());
				//Establecemos la cantidad pendiente que nos falta para la próxima vida
				timeRemaining = totalTimeRemaining % timeToWait;
				WaitForLife (timeRemaining);
			}
		}
	}
}
