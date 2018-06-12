using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingEnemies : MonoBehaviour {

	public Transform[] leftPosition, centerPosition, rightPosition;
	//Con estas variables manejamos las proporciones
	public float propInX = 0.25f, propInY = 0.5f;

	/*void Start(){

		EnemiesNewPositions (leftPosition, -ScreenController.screen.screenWidth / 5.3f, 0.25f);
		EnemiesNewPositions (centerPosition);
		EnemiesNewPositions (rightPosition, ScreenController.screen.screenWidth / 5.3f, -0.25f);
	}

	void EnemiesNewPositions(Transform[] transformList, float lastXPos = 0, float proportionX = 0){

		//POR EL MOMENTO ESTAS PROPORCIONES FUNCIONAN BIEN
		//float lastXPos = -1.5f;
		float lastYPos = 3;
		float actualXProportion = proportionX;
		float actualYProportion = 0.5f;

		for (int i = 0; i < transformList.Length; i++) {
			Vector3 newPos = new Vector3 (lastXPos - actualXProportion, lastYPos - actualYProportion, 0);
			transformList [i].position = newPos;
			actualXProportion += proportionX;
			lastXPos = newPos.x;
			actualYProportion += 0.5f;
			lastYPos = newPos.y;
		}
	}*/
	void Start(){

		EnemiesNewPositions (leftPosition, -ScreenController.screen.screenWidth / 2, propInX);
		EnemiesNewPositions (centerPosition);
		EnemiesNewPositions (rightPosition, ScreenController.screen.screenWidth / 2, -propInX);
	}
	//Función para posicionar los enemigos de forma proporcional usando perspectiva(un punto de fuga)
	void EnemiesNewPositions(Transform[] transformList, float lastXPos = 0, float proportionInX = 0, float proportionInY = 0.5f){

		float actualXProportion = proportionInX * 5;
		lastXPos -= actualXProportion;

		float actualYProportion = proportionInY * 5;
		float lastYPos = -ScreenController.screen.screenHeight / 3 - actualYProportion;

		for (int i = transformList.Length - 1; i > -1; i--) {
			Vector3 newPos = new Vector3 (lastXPos + actualXProportion,lastYPos + actualYProportion, 0);
			transformList [i].position = newPos;

			actualXProportion -= proportionInX;
			lastXPos = newPos.x;
			actualYProportion -= proportionInY;
			lastYPos = newPos.y;
		}
	}
}
