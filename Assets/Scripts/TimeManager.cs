using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour {

	//Para mostrar la hora actual(local)
	public int hour;
	public int minute;
	public int seconds;
	public string amPm = "AM";

	public int currentTime; //Tiempo total en segundos desde 1970

	void Awake(){

		Time ();
	}

	void Update(){

		Time ();
	}

	public void Time(){

		DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		currentTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

		DateTime time = DateTime.Now; //Tiempo local

		hour = time.Hour;
		minute = time.Minute;
		seconds = time.Second;
	}

	public bool EnemiesMoveNow(){

		return false;
	}

	public int Now(){

		return 0;
	}
}

