using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour {

	public Battery bat;
	public Lives2 lives;
	public DataManager2 data;
	public ScoreManager score;
	public TimeManager time;
	public Player player;
	public Enemies enemies;
	public Display display;
	public Buttons buttons;
	public Pause pause;
	public Colision colision;
	public Sound sound;

	private string globalState;

	public float lowestSpeed = 16.6f; //16.6 m/s
	public int gameSpeed = 1; //Velocidad del juego donde 1 es normal
	public float racingTime = 0; //Tiempo en carrera

	//POR QUE TENGO REFERENCIA DE SPRITES ACÁ?
	private List<SpriteRenderer> arrayOfEnemies;

	//variables para controllar las recargas de batería
	public const int waitingTime = 10; //segundos de espera para obtener una batería(una vez perdida)
	private int timeForNextBat; //Variable para saber el tiempo que falta para una batería(mostrada en el display)
	private int remainingTimeForFullCharge; //Tiempo total para llenar la batería

	public int gameOverWait;
	public float timeToMainMenu;



	void Start(){
		
		AlreadyPlayed ();

		display.ShowSplashScreen ();

		display.MainMenu (bat.Get(),lives.Get());

		SetState ("mainMenu");
	}

	void Update(){		

		print (globalState);

		bat.Charging (time.totalTime, waitingTime, ref timeForNextBat);
		display.RealTime (time.hour, time.minute, time.amPm);
		//Temporal
		display.CountDown (true, timeForNextBat);

		//Temporal
		if (Input.GetKeyDown(KeyCode.A)) {
			timeForNextBat -= 1;
		}

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

		if (data.Get("AlreadyPlayed") == 0) {
			data.Set ("AlreadyPlayed", 1);
			data.Set ("CurrentBatteries", bat.maxBatteries);
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
			if (!bat.charging) {
				bat.timeToReachForBat = time.totalTime + waitingTime;
				bat.charging = true;
			}
			display.Battery (bat.Get());
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

		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

		racingTime += Time.deltaTime;

		if (buttons.PausePressed()) {
			SetState("paused");
		}
		display.CurrentScore(score.Distance (lowestSpeed, gameSpeed, racingTime));


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
