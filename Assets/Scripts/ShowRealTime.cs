using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShowRealTime : MonoBehaviour {

	public Text timeText;
	public Text amOrPm;

	void Update(){
		//Obtenemos la hora actual(formato 24 horas)
		DateTime time = DateTime.Now;

		int hour = time.Hour;
		int minute = time.Minute;
		string amPm = "AM";

		//Con esta condicional nos aseguramos que se muestre la hora en formato 12 horas
		if (hour > 12) {
			hour -= 12;
			amPm = "PM";
		}
		//Si el minuto es menor a 10, nos aseguramos que se muestre un cero adelante
		if (minute < 10) timeText.text = hour.ToString () + ":0" + minute.ToString ();
		else timeText.text = hour.ToString () + ":" + minute.ToString ();	

		amOrPm.text = amPm;
	}
}
