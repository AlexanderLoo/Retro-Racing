using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	public DataManager2 data;

	private bool isbatteryLeft = true;
	private int batteries;

	public int maxBatteries = 3;

	public bool Left(){

		isbatteryLeft = (Get () > 0) ? true : false;
		return isbatteryLeft;
	}

	public int Get(){
	
		batteries = data.Get ("CurrentBatteries");
		print (batteries);
		return batteries;
	}
	//Función usada para añadir o remover un valor a las baterías
	public void Add(int value){

		batteries += value;
		data.Set("CurrentBatteries", batteries);
	}

	public int TimeToNextCharge(int totalTime, int waitingTime){

		return totalTime + waitingTime;
	}

	public void GameOver(){

		return;
	}




}
