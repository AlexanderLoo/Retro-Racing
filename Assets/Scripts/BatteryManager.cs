using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BatteryManager : MonoBehaviour {

	private BatteryDisplay batteryDisplay;

	//tiempo de espera en segundos
	public int timeToWait = 60;
	//tiempo total desde la medianoche en segundos
	private int totalTime;
	//variable a alcanzar para obtener una vida
	private int getNewLife;
	//Booleano que controla el estado de carga de vida, esto permite que no se resetee el tiempo de espera si se pierde mas de una vida
	private bool waitingForLife = false;

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
		}
	}

	//Esta función se ejecuta cada frame, por eso limitamos su función con el booleano 'waitingForLife'
	void LivesManager(){

		if (getNewLife > totalTime) {
			//por ahora mostramos un print en la consola, más adelante mostramos el tiempo de espera en el UI
			print (getNewLife - totalTime);
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
					}
				}
			}
		}
	}
}
