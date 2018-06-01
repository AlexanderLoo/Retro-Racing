using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	private bool isbatteryLeft = true;
	private int batteries;

	public int maxBatteries = 3;

	public bool Left(){

		isbatteryLeft = (Get () > 0) ? true : false;
		return isbatteryLeft;
	}

	public int Get(){
	
		batteries = GetDataInMemory ("CurrentBatteries");
		print (batteries);
		return batteries;
	}
	//Función usada para añadir o remover un valor a las baterías
	public void Add(int value){

		batteries += value;
		SetDataInMemory ("CurrentBatteries", batteries);
	}

	public int TimeToNextCharge(){
		
		return 0;
	}

	public void GameOver(){

		return;
	}

	private int GetDataInMemory(string name){

		int value = PlayerPrefs.GetInt (name);
		return value;
	}

	public void SetDataInMemory(string name, int value){
		
		PlayerPrefs.SetInt (name, value);
	}
}
