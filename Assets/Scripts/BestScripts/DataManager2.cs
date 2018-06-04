using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager2 : MonoBehaviour {

	public int GetDataInMemory(string name){

		int value = PlayerPrefs.GetInt (name);
		return value;
	}

	public void SetDataInMemory(string name, int value){

		PlayerPrefs.SetInt (name, value);
	}
}
