using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager levelManager;

	//variables que determinan los niveles
	public bool alternateSpawn = true, randomSpawn;

	void Awake(){
		//Singleton
		if (levelManager == null) {
			levelManager = this;
		} else if (levelManager != this) {
			Destroy (gameObject);
		}
	}

//	//Variable para determinar en cuanto score se aumenta la dificultad
//	public int changeLevel = 500;
//
//	void UpdateLevel(){
//
//		if (GameController.gameController.speed > 0.1f) {
//			if (GameController.gameController.score > currentScore) {
//				currentScore += changeLevel;
//				changeLevel *=2;
//				GameController.gameController.speed -= 0.1f;
//			}
//		}
//	}
}
