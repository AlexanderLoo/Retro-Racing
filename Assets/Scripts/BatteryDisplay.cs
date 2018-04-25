using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryDisplay : MonoBehaviour {

	private BatteryManager batteryManager;
	private Lives lives;
	//las vidas que se muestran en el UI
	public Image[] livesImage;
	//Referencia al contador dentro del UI
	public GameObject batteryCountDown;
	public Text minutesText, secondsText;

	void Awake(){

		batteryManager = GetComponent<BatteryManager> ();
		lives = GetComponent<Lives> ();
	}

	//Función para mostrar las vidas disponibles al iniciar el juego
	public void ShowCurrentLives(){

		for (int i = livesImage.Length -1; i > livesImage.Length - 1 - lives.GetCurrentLives() ; i--) {
			livesImage [i].enabled = true;
		}
	}

	//Función para remover vida
	public void RemoveLife(){
		
		for (int i = 0; i < livesImage.Length; i++) {
			if (livesImage[i].enabled) {
				lives.LivesManager (-1);
				livesImage[i].GetComponent<Animator>().enabled = true;
				//livesImage [i].enabled = false;
				batteryManager.WaitForLife (batteryManager.timeToWait);
				break;	
			}
		}
	}

	//Función para agregar vida
	public void AddLife(){

		for (int i = livesImage.Length -1; i > -1 ; i--) {
			if (!livesImage[i].enabled) {
				livesImage[i].enabled = true;
				lives.LivesManager (1);
				if (i == 0) {
					batteryManager.BatteryFilled ();
				}
				else if(i == livesImage.Length -1){
					GameController.gameController.CanPlay (true);
				}
				break;
			}
		}
	}
	//Mostramos el conteo regresivo para adquerir una vida
	public void ShowCountDown(Text text,int counter){

		if (counter < 10) {
			text.text = "0" + counter.ToString ();
		} else {
			text.text = counter.ToString ();
		}
	}
	// MARTÍN, EL SIGUIENTE COMENTARIO ES TU PLACEHOLDER QUE ANTES ESCRIBISTE , HE PUESTO UNOs COMENTARIOS..LO GUARDO POR SEACASO
	//	private DateTime time = DateTime.Now;
	//	private int batteryPacksLeft = 3;
	//
	//
	//
	//	// Use this for initialization
	//	void Start () {
	//		
	//	}
	//	
	//	// Update is called once per frame
	//	void Update () {
	//		
	//	}
	//
	//
	//	// Llamar esto para ver cuanto de tiempo le queda de espera al jugador para recien poder empezar a jugar(ESTA FUNCIÓN TODAVÍA NO EXISTE, QUEDA PENDIENTE...)
	//	public Text timeLeft() {
	//
	//	}	
	//
	//	// Llamar a esto cuando el player se estrelle(ESTA FUNCIÓN SE ENCUENTRA EN GAMECONTROLLER 'CarCollision()')
	//	public void playerLost() {
	//		batteryPacksLeft--;
	//	}
	//
	//	// Llamar esto para ver si hay bateria para que el jugador pueda jugar o no(ESTA FUNCIÓN YA EXISTE, SE ENCUENTRA EN GAMECONTROLLER EN UNA VARIABLE LLAMADA 'canPlay')
	//	public bool batteryAvailable() {
	//		return true;
	//	}
	//
	//	// Llamar esto para ver cuanta carga le queda(ESTA FUNCIÓN YA EXISTE, SE ACCEDE CON LA FUNCIÓN 'GetCurrentLife()')
	//	public int batteryLeft() {
	//		return batteryPacksLeft;
	//	}
}
