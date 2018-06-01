using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour {

	public Battery bat;
	public Lives2 lives;
	public TimeManager time;
	public Player player;
	public Enemies enemies;
	public Display display;
	public Buttons buttons;
	public Pause pause;
	public Colision colision;
	public Sound sound;
	//POR QUE TENGO REFERENCIA DE SPRITES ACÁ?
	private List<SpriteRenderer> arrayOfEnemies;

	private int timeToNextbat;

	public int gameOverWait;
	public float timeToMainMenu;

	private string globalState;

	void Start(){

		display.ShowSplashScreen ();

		display.MainMenu (bat.Get(),lives.Get());

		SetState ("mainMenu");
	}

	void Update(){		

		print (globalState);

		switch (globalState) {
			
		case "mainMenu":
			MainMenuState ();
			break;
		case "startGame":
			StartGameState ();
			break;
		case "playing":
			PlayingState ();
			break;
		case "crashed":
			CrashedState ();
			break;
		case "paused":
			PausedState ();
			break;
		case "gameOver":
			GameOverState ();
			break;
		default:
			Debug.Log ("No se encuentra en ningún estado");	
			break;
		}
	}

	private void AlreadyPlayed(){

		/*PENDIENTE EN BUSCAR UNA MEJOR SOLUCIÓN(NO DEBERÍA USAR LA FUNCIÓN 'SetDataInMemory' EN BATTERY?,
		 * Y SI CREO LA FUNCIÓN ACÁ, TENDRIA QUE TENER UNA REFERENCIA DE 'GameController' EN 'Battery'
		 * Y SE ROMPERÍA LA ESTRUTURA)*/
		//POSIBLE SOLUCIÓN: USAR UNA NUEVA CLASE 'DATAMANAGER'
		 
		if (PlayerPrefs.GetInt("AlreadyPlayed") == 0) {
			bat.SetDataInMemory ("AlreadyPlayed", 1);
			bat.SetDataInMemory ("CurrentBatteries", bat.maxBatteries);
		}
	}

	//states: mainMenu, startGame, playing, crashed, paused, gameover
	void SetState(string state){

		globalState = state;
	}

	string GetState(){

		return globalState;
	}

	void MainMenuState(){

		if (buttons.StartPressed ()) {
			SetState ("startGame");
		} 
	}

	void StartGameState(){

		if (bat.Left ()) {

			bat.Add (-1);
			timeToNextbat = bat.TimeToNextCharge();
			display.StartingGame();
			SetState ("playing");
			pause.BeforeStart();
		} else {
			display.NotEnoughtBat ();
			sound.BadLuck();
			SetState("mainMenu");
		}
	}

	void PlayingState(){

		if (buttons.PausePressed()) {
			SetState("paused");
		}
		display.BatTimeFor(timeToNextbat);
		display.CurrentScore();

		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

		if (buttons.Left() && player.CanLeft()) {
			display.PlayerMove(player.Movement(-1));
		}
		if (buttons.Right() && player.CanRight()) {
			display.PlayerMove(player.Movement(1));
		}

		if (time.EnemiesMoveNow()) {
			arrayOfEnemies = enemies.MoveDown();
			display.Enemies(arrayOfEnemies);
		}

		if(colision.Crashed(player.currentIndex, arrayOfEnemies )){

			SetState("crashed");
		}
	}

	void PausedState(){

		display.ShowExit ();

		if (buttons.StartPressed()) {
			SetState ("playing");
		}
	}

	void CrashedState(){

		lives.Decrease (1);
		display.Crashed ();
		sound.Crashed();
		pause.Crashed();
		enemies.Reset();
		if (lives.Left ()) {
			SetState ("playing");
		} else {
			bat.GameOver();
			display.GameOver();
			sound.GameOver();
			SetState("gameOver");
			timeToMainMenu = time.Now () + gameOverWait;
		}
	}

	void GameOverState(){

		if (buttons.StartPressed()) {
			SetState ("mainMenu");
		}
		else if (time.Now() < timeToMainMenu) {
			SetState ("mainMenu");
		}
	}
}
