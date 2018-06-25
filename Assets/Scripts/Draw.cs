﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour {

	//Sprite personalizables del juego
	public Sprite car;
	public Sprite battery;
	public Sprite live;

	//Listas de Imagenes
	public List<SpriteRenderer> playerArray; //Buscamos las referencias en el editor para no perder el orden
	public List<SpriteRenderer> enemiesArray;
	public List<SpriteRenderer> enemiesBackgroundArray;
	public List<Image> batteryArray; //Buscamos las referencias en el editor para no perder el orden
	public List<Image> batteryBackgroundArray;
	public List<Image> livesArray;

	//UI
	public Text scoreText;
	public Text countMinutes;
	public Text countSeconds;

	public Text hour;
	public Text minute;
	public Text amOrPm;

	public GameObject countDown;

	void Awake(){

		FindSpriteRendererArray (enemiesArray, "Enemy");
		FindSpriteRendererArray (enemiesBackgroundArray, "EnemyBackground");
		FindImageArray (batteryBackgroundArray, "BatteryBackground");
	}

	//Función para acceder al tamaño de la pantalla
	public int GetScreenWidth(){

		return Screen.width;
	}
	public int GetScreenHeight(){

		return Screen.height;
	}

	//Funciones para buscar las listas en la escena según su tag
	void FindSpriteRendererArray(List<SpriteRenderer> array, string tag){

		GameObject[] goArray = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject go in goArray) {
			array.Add (go.GetComponent<SpriteRenderer>());
		}
	}

	void FindImageArray(List<Image> array, string tag){

		GameObject[] goArray = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject go in goArray) {
			array.Add (go.GetComponent<Image>());
		}
	}

	void Start(){

		FillSpriteArray (playerArray, car);
		FillSpriteArray (enemiesArray, car);
		FillSpriteArray (enemiesBackgroundArray, car);
		FillImageArray (batteryArray, battery);
		FillImageArray (batteryBackgroundArray, battery);
//		FillImageArray (livesArray, live);
	}

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

	public void EnemyEnable(string name, bool value){

		SpriteRenderer sr = FindName(name).GetComponent<SpriteRenderer>();
		sr.enabled = value;
	}

	public GameObject FindName(string name){  //Buscamos un GameObject por sunombre

		GameObject go = GameObject.Find (name);
		return go;
	}


	public void ObjectShown(GameObject gameObject, bool value){

		gameObject.SetActive (value);
	}

	public void Console(){
		return;
	}

	public void Lives(){
		return;
	}


}
