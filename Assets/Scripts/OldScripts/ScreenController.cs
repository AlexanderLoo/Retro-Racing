using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {

	public static ScreenController screen;
	//IMPORTANTE: NO es la longitud del ancho y del alto de la pantalla, es la posición de los extremos positivos(para obtener la longitud total multiplicar x2)
	public float screenWidth, screenHeight;

	void Awake(){
		//Singleton
		if (screen == null) {
			screen = this;
		}
		else if (screen != this) {
			Destroy (gameObject);
		}
		//Convertimos el tamaño de la pantalla de pixeles a medidas de Unity
		Vector2 screenSizeInPixels = new Vector2 (Screen.width, Screen.height);
		Vector2 screenSize = Camera.main.ScreenToWorldPoint (screenSizeInPixels);
		screenWidth = screenSize.x;
		screenHeight = screenSize.y;
	}
}
