using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BatteryManager : MonoBehaviour {


	private DateTime time = DateTime.Now;
	private int batteryPacksLeft = 3;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Llamar esto para ver cuanto de tiempo le queda de espera al jugador para recien poder empezar a jugar
	public Text timeLeft() {

	}	

	// Llamar a esto cuando el player se estrelle
	public void playerLost() {
		batteryPacksLeft--;
	}

	// Llamar esto para ver si hay bateria para que el jugador pueda jugar o no
	public bool batteryAvailable() {
		return true;
	}

	// Llamar esto para ver cuanta carga le queda
	public int batteryLeft() {
		return batteryPacksLeft;
	}


}
