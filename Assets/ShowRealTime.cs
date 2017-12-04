using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRealTime : MonoBehaviour {

	private Text time;
	private Text amOrPm;

	void Awake(){

		time = GetComponent<Text> ();
		amOrPm = GetComponentInChildren<Text> ();
	}

	void Update(){


	}
}
