using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	private bool isbatteryLeft = true;
	public int batteries;

	public int maxBatteries = 3;

	public bool Left(){

		isbatteryLeft = (Get () > 0) ? true : false;
		return isbatteryLeft;
	}

	public int Get(){

		return batteries;
	}
	//Función usada para añadir o remover un valor a las baterías
	public void Add(int value){

		batteries += value;
		batteries = (batteries > maxBatteries) ? maxBatteries : batteries;
	}

	public void GameOver(){

		return;
	}
}
