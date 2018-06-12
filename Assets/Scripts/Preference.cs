using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preference : MonoBehaviour {

	public int Get(string name){

		int value = PlayerPrefs.GetInt (name);
		return value;
	}

	public void Set(string name, int value){

		PlayerPrefs.SetInt (name, value);
	}
}
