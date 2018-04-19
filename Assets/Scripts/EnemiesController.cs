using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//El siguiente script es un poco complicado ya que llama a las corutinas a cada rato y los corta, no sé si es eficiente...EN ESPERA DE MEJORAR LA LÓGICA
public class EnemiesController : MonoBehaviour {

	public SpriteRenderer[] leftEnemies, centerEnemies, rightEnemies;

	//El siguiente entero indica el número de enemigos en la misma fila
	private int enemyNum = 1;

	void Start(){

		DisableSprites (leftEnemies);
		DisableSprites (centerEnemies);
		DisableSprites (rightEnemies);
	}

	void LateUpdate(){
		if (GameController.gameController.startGame) {
			if (enemyNum == 1) {
				OneLineSpawn ();
			} else if (enemyNum == 2) {
				TwoLinesSpawn ();
			} else {
				return;
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
	IEnumerator SpawnEnemy(SpriteRenderer[] spriteList,int enemynum){
		while (true) {
			for (int i = 0; i < spriteList.Length; i++) {
				if (i != 0) spriteList [i - 1].enabled = false;
				spriteList [i].enabled = true;
				yield return new WaitForSeconds (GameController.gameController.speed);
				//El índice uno parecer ser el más adecuado por el espacio que tiene el player para esquivar..y por que no se superpone con otros spawns
				if (i == 1) {
					if (LevelManager.levelManager.alternateSpawn) {
						enemyNum = enemynum;
					}
					else if(LevelManager.levelManager.randomSpawn){
						enemyNum = Random.Range (1, 3);
					}else {
						enemyNum = 0;
					}
				}
			}
			enemyNum = enemynum;
			DisableSprites (spriteList);
			//Usamos break para no usar esta corutina mas
			break;
		}
	}
	//Función para spawnear un enemigo por fila
	void OneLineSpawn(){

		enemyNum = 0;
		//Lista que contiene listas de sprites
		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
		//Elegimos una linea al azar
		int index = Random.Range (0, listOfSpriteList.Length);
		SpriteRenderer[] spriteList = listOfSpriteList [index];
		//Llamamos la corutina
		StartCoroutine (SpawnEnemy (spriteList,1));
	}
	//Función para spawnear 2 enemigos por fila
	void TwoLinesSpawn(){

		enemyNum = 0;
		SpriteRenderer[][] listOfSpriteList = { leftEnemies, centerEnemies, rightEnemies };
		int index = Random.Range (0, listOfSpriteList.Length);
		int index2 = Random.Range (0, listOfSpriteList.Length);
		//usamos este bucle while para asegurarnos de que no nos toque el mismo indice dos veces
		while (index2 == index) {
			index2 = Random.Range (0, listOfSpriteList.Length);
		}
		SpriteRenderer[] spriteList = listOfSpriteList[index];
		SpriteRenderer[] spriteList2 = listOfSpriteList [index2];

		StartCoroutine (SpawnEnemy (spriteList,2));
		StartCoroutine (SpawnEnemy (spriteList2,2));
	}
}


