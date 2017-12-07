using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {

	public SpriteRenderer[] leftEnemies, centerEnemies, rightEnemies;
	//Velocidad en que transitan los sprites
	public float speed = 1;
	//Con esta variable regulamos el tiempo de espera para el siguente spawn
	public float spawnTimeInSeconds;

	void Start(){

		DisableSprites (leftEnemies);
		DisableSprites (centerEnemies);
		DisableSprites (rightEnemies);
		StartCoroutine (SpawnEnemies ());
	}
	void Update(){


	}
	//Función para desactivar los sprites de los enemigos
	void DisableSprites(SpriteRenderer[] spriteList){

		foreach (SpriteRenderer sprite in spriteList) {

			sprite.enabled = false;
		}
	}

	//Función para spawnear los enemigos
	IEnumerator SpawnEnemies(){

		//Lista multidimensional que contiene listas de sprites
		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
		//POR MIENTRAS USAMOS UN WHILE LOOP
		while (true) {
			//Seleccionamos una lista de sprite al azar y aplicamos la lógica de "movimiento"
			int index = Random.Range (0, listOfSpriteList.Length);
			SpriteRenderer[] spriteList = listOfSpriteList[index];

			for (int i = 0; i < spriteList.Length; i++) {
				if (i != 0) spriteList [i - 1].enabled = false;
				spriteList [i].enabled = true;
				yield return new WaitForSeconds (speed);
			}
			DisableSprites (spriteList);
		}
	}

	IEnumerator SpawnEnemy(SpriteRenderer[] spriteList){

		yield return new WaitForSeconds (1);

		while (true) {
			for (int i = 0; i < spriteList.Length; i++) {
				if (i != 0) spriteList [i - 1].enabled = false;
				spriteList [i].enabled = true;
				yield return new WaitForSeconds (speed);
			}
			DisableSprites (spriteList);
			break;
		}
	}
}


