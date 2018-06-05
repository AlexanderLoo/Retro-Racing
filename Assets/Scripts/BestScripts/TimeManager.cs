using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {

	//Para mostrar la hora actual
	public int hour;
	public int minute;
	public int seconds;
	public string amPm = "AM";

	public int totalTime; //tiempo en segundos transcurrido desde las 00:00 horas

	void Update(){

		Time ();
	}

	public void Time(){

		DateTime time = DateTime.Now;

		hour = time.Hour;
		minute = time.Minute;
		seconds = time.Second;

		totalTime = (hour * 3600) + (minute * 60) + seconds;
		//print (totalTime);
	}

	public bool EnemiesMoveNow(){

		return false;
	}

	public int Now(){

		return 0;
	}
}

