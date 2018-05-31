using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

	private bool isbatteryLeft = true;
	private int batteries = 3;

	public bool Left(){
		
		return isbatteryLeft;
	}

	public int Get(){

		return batteries;
	}

	public void Remove(int value){

		batteries -= value;
	}
}
