using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	public float score;
	public Text scoreText;

	void Start(){
		
		score = 0;
		scoreText.text = score.ToString ();
	}

	//Función para calcular la distancia en km y mostrarlas en el score
	public void Distance(){
		//Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(km/h * 0.277)
		//Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
		//distance = (16.6 / speed) * time ---> distance = (16.6 / speed) * Time.time(distancia en metros, distancia en km --> score/1000)
		//dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

		float distance = (float)(16.6 /GameController.gameController.speed) * Time.timeSinceLevelLoad;
		score = distance;
		scoreText.text = Mathf.Round (score).ToString ();
	}
}
