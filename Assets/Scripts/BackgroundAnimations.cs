using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimations : MonoBehaviour {

	public SpriteRenderer[] cactus;

	private bool cactusAnim = true;

	void Awake(){
		DisableSprites (cactus);
	}

	void Update(){

		if (cactusAnim && GameController.gameController.startGame) {
			cactusAnim = false;
			StartCoroutine (CactusAnim ());
		}
	}

	//Función para desactivar todos los sprites de una lista de sprites
	void DisableSprites(SpriteRenderer[] spriteList){

		foreach (SpriteRenderer sprite in spriteList) {
			sprite.enabled = false;
		}
	}

	IEnumerator CactusAnim(){
		
		while (true) {
			for (int i = 0; i < cactus.Length; i++) {
				if (i != 0) cactus [i - 1].enabled = false;
				cactus [i].enabled = true;
				yield return new WaitForSeconds (GameController.gameController.speed);
			}
			DisableSprites (cactus);
			cactusAnim = true;
			break;
		}
	}
}
