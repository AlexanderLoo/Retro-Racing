using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour {

	public Battery bat;
	public Lives2 lives;
	public Player player;
	public Display display;
	public Buttons buttons;
	public Pause pause;
	public Colision colision;
	public Sound sound;

	public float timeToMainMenu;

	private string globalState;

	void Start(){

		//Display.ShowSplashScreen ();

		//Display.MainMenu (lives.GetLives (), bat.GetBatteries ());

		SetState ("mainMenu");
	}

	void Update(){		

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

	//states: mainMenu, startGame, playing, crashed, paused, gameover
	void SetState(string state){

		globalState = state;
	}

	string GetState(){

		return globalState;
	}

	void MainMenuState(){

		if (buttons.StartPressed()) {
			SetState ("startGame");
		}
	}

	void StartGameState(){

		if (bat.Left ()) {

			bat.Remove (1);
			//timeToNextbat = bat.timeToNextCharge();
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
		//display.batTimeFor(timeToNextbat);
		display.CurrentScore();
		if (buttons.Left() && player.CanLeft()) {
			display.PlayerMove(player.Movement());
		}
		if (buttons.Right() && player.CanRight()) {
			display.PlayerMove(player.Movement());
		}

//		if (Time.OtherCarsMoveNow()) {
//			arrayOfCars = otherCars.moveDown();
//			display.otherCars(arrayOfCars);
//		}

		if(colision.Crashed()){

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
		//otherCars.reset();
		if (lives.Left ()) {
			SetState ("playing");
		} else {
			//bat.GameOver();
			display.GameOver();
			sound.GameOver();
			SetState("gameOver");
			//timeForMainMenu = now () + gameOverWait;
		}
	}

	void GameOverState(){

		if (buttons.StartPressed()) {
			SetState ("mainMenu");
		}
//		else if (now() < timeToMainMenu) {
//			SetState ("mainMenu");
//		}
	}
}
