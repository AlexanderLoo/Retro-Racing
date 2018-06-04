using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {

	//Para mostrar la hora actual
	public int hour;
	public int minute;
	public string amPm = "AM";

	public int totalTime;

	void Update(){

		RealTime ();
	}

	public void RealTime(){

		DateTime time = DateTime.Now;

		hour = time.Hour;
		minute = time.Minute;

	}

	public int GetTotal(){

		DateTime time = DateTime.Now;

		int day = time.Day;
		int hour = time.Hour;
		int minute = time.Minute;
		int seconds = time.Second;

		//convertimos todo en segundos
		hour *= 3600;
		minute *= 60;

		totalTime = hour + minute + seconds;
		return totalTime;
	}


	public bool EnemiesMoveNow(){

		return false;
	}

	public int Now(){

		return 0;
	}
}

