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
		return batteries;
	}

	public void Remove(int value){

		batteries -= value;
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
}
