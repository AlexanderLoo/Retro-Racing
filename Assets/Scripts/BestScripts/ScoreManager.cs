﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {


	public float Distance(float lowestSpeed, int gameSpeed, float racingTime){
		//Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(velocidad normal)
		//Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
		//distance = (16.6 / speed) * racingTime
		//dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

		float distance = (float)(lowestSpeed /gameSpeed) * racingTime;
		float score = Mathf.Round (distance);
		return score;
	}
}