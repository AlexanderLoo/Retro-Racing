using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour {

	//Sprite personalizables del juego
	public Sprite player;
	public Sprite enemy;
	public Sprite live;
	public Sprite battery;

	//Listas de Imagenes
	public List<SpriteRenderer> playerArray;
	public List<SpriteRenderer> enemiesArray;
	public List<Image> batteryArray;

	//UI
	public Text scoreText;

//	void Start(){
//
//		FillSpriteArray (playerArray, player);
//		FillSpriteArray (enemiesArray, enemy);
//		FillImageArray (batteryArray, battery);
//	}

	public void FillSpriteArray(List<SpriteRenderer> spriteArray, Sprite sprite){

		foreach (SpriteRenderer _sr in spriteArray) {
			_sr.sprite = sprite;
		}
	}

	public void FillImageArray(List<Image> imageArray, Sprite sprite){

		foreach (Image image in imageArray) {
			image.sprite = sprite;
		}
	}

	public void DisableAllSprites(List<SpriteRenderer> spriteArray){

		foreach (SpriteRenderer _sr in spriteArray) {
			_sr.enabled = false;
		}
	}
	public void DisableAllImage(List<Image> imageArray){

		foreach (Image image in imageArray) {
			image.enabled = false;
		}
	}

	public void Console(){
		return;
	}

	public void Lives(){
		return;
	}


}
