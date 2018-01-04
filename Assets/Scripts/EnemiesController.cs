using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//El siguiente script es un poco complicado ya que llama a las corutinas a cada rato y no sé si es eficiente...EN ESPERA DE MEJORAR LA LÓGICA
public class EnemiesController : MonoBehaviour {

	public SpriteRenderer[] leftEnemies, centerEnemies, rightEnemies;
	//Con esta variable regulamos el tiempo de espera para el siguente spawn
	public float spawnTimeInSeconds;
	//Los siguientes booleanos son los niveles de dificultad del juego
	private bool oneLineSpawn = false, twoLineSpawn = true;

	void Start(){

		DisableSprites (leftEnemies);
		DisableSprites (centerEnemies);
		DisableSprites (rightEnemies);
	}
	void Update(){

		if (GameController.gameController.startGame) {
			if (oneLineSpawn) {
				OneLineSpawn ();
			}
			if (twoLineSpawn) {
				TwoLinesSpawn ();
			}
		}
	}
	//Función para desactivar los sprites de los enemigos
	void DisableSprites(SpriteRenderer[] spriteList){

		foreach (SpriteRenderer sprite in spriteList) {

			sprite.enabled = false;
		}
	}
//	//Función para spawnear los enemigos
//	IEnumerator SpawnOneLineEnemy(){
//
//		oneLineSpawn = false;
//		//Lista que contiene listas de sprites
//		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
//		//POR MIENTRAS USAMOS UN WHILE LOOP
//		while (true) {
//			//Seleccionamos una lista de sprite al azar y aplicamos la lógica de "movimiento"
//			int index = Random.Range (0, listOfSpriteList.Length);
//			SpriteRenderer[] spriteList = listOfSpriteList[index];
//
//			for (int i = 0; i < spriteList.Length; i++) {
//				if (i != 0) spriteList [i - 1].enabled = false;
//				spriteList [i].enabled = true;
//				yield return new WaitForSeconds (speed);
//			}
//			DisableSprites (spriteList);
//			oneLineSpawn = true;
//			break;
//		}
//	}
	IEnumerator SpawnEnemy(SpriteRenderer[] spriteList){
		while (true) {
			for (int i = 0; i < spriteList.Length; i++) {
				if (i != 0) spriteList [i - 1].enabled = false;
				spriteList [i].enabled = true;
				yield return new WaitForSeconds (GameController.gameController.speed);
			}
			twoLineSpawn = true;
			DisableSprites (spriteList);
			//Usamos break para no usar esta corutina mas
			break;
		}
	}

	void OneLineSpawn(){

		oneLineSpawn = false;
		//Lista que contiene listas de sprites
		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
		//Elegimos una linea al azar
		int index = Random.Range (0, listOfSpriteList.Length);
		SpriteRenderer[] spriteList = listOfSpriteList [index];
		//Llamamos la corutina
		StartCoroutine (SpawnEnemy (spriteList));
	}

	void TwoLinesSpawn(){

		twoLineSpawn = false;
		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
		int index = Random.Range (0, listOfSpriteList.Length);
		int index2 = Random.Range (0, listOfSpriteList.Length);
		while (index2 == index) {
			index2 = Random.Range (0, listOfSpriteList.Length);
		}
		SpriteRenderer[] spriteList = listOfSpriteList[index];
		SpriteRenderer[] spriteList2 = listOfSpriteList [index2];

		StartCoroutine (SpawnEnemy (spriteList));
		StartCoroutine (SpawnEnemy (spriteList2));
	}
}


