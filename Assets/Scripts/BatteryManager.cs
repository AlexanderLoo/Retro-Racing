using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BatteryManager : MonoBehaviour {

	//esta variable almacena la cantidad de bateria que nos queda
	public int currentLife;
	//las vidas que se muestran en el UI
	public Image[] lives;

	//tiempo de espera en segundos
	public int timeToWait = 60;
	//tiempo total desde la medianoche en segundos
	private int totalTime;
	//variable a alcanzar para obtener una vida
	private int getNewLife;
	//Booleano que controla el estado de carga de vida, esto permite que no se resetee el tiempo de espera si se pierde mas de una vida
	private bool waitingForLife;

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
		
	//Función para mostrar las vidas disponibles al iniciar el juego
	public void ShowCurrentLives(){

		currentLife = GetCurrentLives ();

		for (int i = lives.Length -1; i > lives.Length - 1 - currentLife ; i--) {
			lives [i].enabled = true;
		}
	}

	//Función para remover vida
	public void RemoveLife(){

		currentLife -= 1;
		SetCurrentLives (currentLife);
		for (int i = 0; i < lives.Length; i++) {
			if (lives[i].enabled) {
				//lives [i].enabled = false;
				lives[i].GetComponent<Animator>().enabled = true;
				if (!waitingForLife) {
					getNewLife = totalTime + timeToWait;
					waitingForLife = true;
				}
				break;
			}
		}
	}

	//Función para agregar vida
	public void AddLife(){

		currentLife += 1;
		SetCurrentLives (currentLife);
		for (int i = lives.Length -1; i > lives.Length - 1 - currentLife ; i--) {
			if (!lives[i].enabled) {
				lives [i].enabled = true;
			}
		}
	}

	//Esta función se ejecuta cada frame, por eso limitamos su función con el booleano 'waitingForLife'
	void LivesManager(){

		if (getNewLife > totalTime) {
			//por ahora mostramos un print en la consola, más adelante mostramos el tiempo de espera en el UI
			print (getNewLife - totalTime);
		} else {
			if (waitingForLife) {
				AddLife ();
				//La siguiente lógica permite actualizar el conteo de obtención de nueva vida, si es que no tenemos la bateria llena
				for (int i = 0; i < lives.Length; i++) {
					if (!lives[i].enabled) {
						getNewLife = totalTime + timeToWait;
						break;
					}
				}
				//se vuelve falso sólo si estamos full de batería
				waitingForLife = false;
			}
		}
	}

	//Guarda en la memoria la cantidad de vida que nos queda donde i es el valor a guardar
	public void SetCurrentLives(int i){

		PlayerPrefs.SetInt ("CurrentLife", i);
	}
	//Retorna la cantidad de vida guardada en la memoria
	public int GetCurrentLives(){

		return PlayerPrefs.GetInt ("CurrentLife");
	}

	// MARTÍN, EL SIGUIENTE COMENTARIO ES TU PLACEHOLDER QUE ANTES ESCRIBISTE , HE PUESTO UNOs COMENTARIOS..LO GUARDO POR SEACASO
	//	private DateTime time = DateTime.Now;
	//	private int batteryPacksLeft = 3;
	//
	//
	//
	//	// Use this for initialization
	//	void Start () {
	//		
	//	}
	//	
	//	// Update is called once per frame
	//	void Update () {
	//		
	//	}
	//
	//
	//	// Llamar esto para ver cuanto de tiempo le queda de espera al jugador para recien poder empezar a jugar(ESTA FUNCIÓN TODAVÍA NO EXISTE, QUEDA PENDIENTE...)
	//	public Text timeLeft() {
	//
	//	}	
	//
	//	// Llamar a esto cuando el player se estrelle(ESTA FUNCIÓN SE ENCUENTRA EN GAMECONTROLLER 'CarCollision()')
	//	public void playerLost() {
	//		batteryPacksLeft--;
	//	}
	//
	//	// Llamar esto para ver si hay bateria para que el jugador pueda jugar o no(ESTA FUNCIÓN YA EXISTE, SE ENCUENTRA EN GAMECONTROLLER EN UNA VARIABLE LLAMADA 'canPlay')
	//	public bool batteryAvailable() {
	//		return true;
	//	}
	//
	//	// Llamar esto para ver cuanta carga le queda(ESTA FUNCIÓN YA EXISTE, SE ACCEDE CON LA FUNCIÓN 'GetCurrentLife()')
	//	public int batteryLeft() {
	//		return batteryPacksLeft;
	//	}
}
