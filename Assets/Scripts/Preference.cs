using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preference : MonoBehaviour {

	public int GetInt(string name){

		int value = PlayerPrefs.GetInt (name);
		return value;
	}

	public void SetInt(string name, int value){

		PlayerPrefs.SetInt (name, value);
	}

	public string GetString(string name){
		
		string value = PlayerPrefs.GetString (name);
		return value;
	}

	public void SetString(string name, string value){

		PlayerPrefs.SetString (name, value);
	}

	//TEST
	public int SetInterupted(bool value){

		if (value) {
			return 1;
		} else {
			return 0;
		}
	}
}
