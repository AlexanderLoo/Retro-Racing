using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public BatteryDisplay batteryDisplay;
	public BatteryManager batteryManager;
	public Lives lives;
	private ScoreDisplay scoreDisplay;
	//Lista de los sprites de enemigos que estan en la misma fila que el player
	public SpriteRenderer[] enemyCollision;
	public SpriteRenderer[] player;
	//Lista de sprites del choque
	public SpriteRenderer[] collision;
	//Velocidad 1 que equivale a 16.6 m/s
	public float speed = 1;
	public GameObject startButton;
	[HideInInspector]
	public bool startGame;

	void Awake(){
		//Singleton
		if (gameController == null) {
			gameController = this;
		} else if (gameController != this) {
			Destroy (gameObject);
		}
		scoreDisplay = GetComponent<ScoreDisplay> ();
	}

	void Start(){
		
		batteryDisplay.ShowCurrentLives ();
		if (lives.GetCurrentLives() <= 0) {
			CanPlay (false);
		} else {
			CanPlay (true);
		}
		speed = 1;
		Time.timeScale = 0;
	}
	//Función que se llama cuando salimos del juego
	void OnDisable(){

		batteryManager.SaveLastTotalTime ();
		batteryManager.SaveLastTimeRemaining ();
	}

	void Update(){

		if (startGame) {
			CarCollision ();
			scoreDisplay.Distance ();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}
	}

	public void CanPlay(bool b){

		startButton.SetActive (b);
	}

	//Lógica de colisión
	void CarCollision(){
		//Si el índice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
		for (int i = 0; i < player.Length; i++) {
			if (enemyCollision[i].enabled && player[i].enabled) {
				startGame = false;
				//mostramos la colisión
				//collision[i].enabled = true;
				//removemos una vida
				batteryDisplay.RemoveLife ();
				Time.timeScale = 0;

				//**********
				enemyCollision[i].enabled = false;
				player[i].GetComponent<Animator>().enabled = true;
				collision [i].GetComponent<Animator> ().enabled = true;
			}
		}
	}
}


/*void Start(){

	FirstTimePlaying(); //Funciòn que se encarga de verificar si es la primera vez que se juega
	battery.GetCurrentBatteries(); //Verificamos cuantas vidas tenemos en la memoria(Quizas mejor este dentro de display)
	display.DisplayLives(); //mostramos las vidas en el UI;
}

void Update(){

	if (buttons.StartButtonPressed() && !inGame) { //Si podemos jugar y ser presiono el boton de 'Start' y no estamos en otro juego
		battery.RemoveBattery(); //removemos una baterìa
		UIanimations.StartCountDown(); //Iniciamos una animaciòn para el conteo, al terminar la animaciòn se llama la funciòn 'GameStart'
	}
//	if (inGame && PauseButtonPressed()) {
//		PauseGame();
//		inGame = false;
//	}
//	else if (!inGame && UnPauseButtonPressed()) {
//		StarGame();
//		inGame = true;
//	}
	if (inGame) {
		CollisionDetector(); //Funciòn para detectar si hay colisiòn
		scoreManager.GetScore(); //Funciòn para calcular el puntaje(Quizas este mejor dentro de display)
		display.ShowScore(); //mostramos el score
	}
	if (buttons.BackButtonPressed()) {
		PauseGame();          //Si se presiona la tecla de atras, pausamos el juego y mostramos la opciòn de salida
		display.ShowExit();
	}
}

public void StartGame(){ //Esta funciòn se llama desde afuera al final de una animaciòn de 'CountDown'

	inGame = true; //activamos el estado 'En Juego'
	enemiesController.SpawnEnemies(); //Comenzamos a crear enemigos
	player.canControl = true; //Podemos controlar al jugador
}

void CanIPlay(){ //Funciòn que pregunta si podemos jugar

	if (battery.GetCurrentBatteries() > 0) {  //La siguiente lògica verifica si se puede jugar segùn si tenemos vidas suficientes
		buttons.ShowStartButton.SetActive(true); //CanPlay(true);
	}else{
		buttons.ShowStartButton.SetActive(false);//CanPlay(false);
	}

}

public void PauseGame(){ //Esta funciòn pùblica esta atada en los botones de pausa y restablecer el juego

	inGame = !inGame; //cambiamos el estado de juego
	time.timeScale = (time.timeScale == 0)?1:0; //si estaba en pausa, restablecemos el juego; caso contrario, pausamos el juego
	player.canControl = !player.canControl; //bloqueamos el control del player o desbloqueamos segùn el estado anterior
	enemies.StopSpawningEnemies(); //Quizas no sea necesario para la salida de màs enemigos por que al pausar el juego, los enemigos no se mueven
}

void CollisionDetector(){ //Funciòn que maneja la colisiòn

	for (int i = 0; i < playerSprite.length; i++) {
		if (playerSprite[i].enable == enemySprite[i].enable) {
			Crash(1);//Funciòn para avisar al juego que hubo colisiòn
		}
	}
}

void Crash(int value){

	scoreManager.SaveScore();
	player.lives -= value; //Restamos vida al player
	if (player.lives <= 0) { //Si nos quedamos sin vida se acaba el juego
		GameOver();
	}else{
		PauseGame(0); //pausamos el juego por un momento
		Invoke(PauseGame(1),2); //reanudamos automàticamente el juego despuès de un breve periodo de tiempo
	}
}

void GameOver(){
				//Al acabar el juego preguntamos si nos queda baterìa para volver a jugar, y asì activar el botòn de inicio de juego
	CanIPlay();
	}
}*/

//--> import bat from batteryManager
//--> import lives from livesManager
//
//void Start(){
//
//	display.showSplashScreen(); // aunque este vacio y no haga nada
//
//	// generalmente aqui se cargan lo assets (en muchos frameworks)    
//	// incluyendo el que se viene.
//
//	display.mainmenu( lives.left(), bat.left() );
//
//	setState(‘mainMenu’);
//
//}
//
//void display.mainmenu( lives, bateries ){
//
//	draw.console();
//	draw.lives(lives);
//	draw.bateries(bateries);
//
//}
//
//
//void setState(state) {
//	GlobalState = state
//}
//
//void getState() {
//	return GlobalState
//	}
//
//
//
//// states: mainmenu, startgame, playing, crashed, paused, gameover
//
//
//void Update(){
//
//	if  (state == ‘mainMenu’) {
//		if (buttons.StartButtonPressed()) {
//			setState(‘startGame’); 
//		}
//	}
//
//	if (state == ‘startGame’) {
//		if (bat.left?() == true) {
//			bat.remove();
//			timeToNextBattery = bat.timeToNextCharge();
//			display.startingGame();
//			setState(playing);
//			pause.beforeStart();
//			return
//		} else {
//			display.notEnoughtBattery();
//			sound.badLuck();
//			setState(‘mainMenu’);
//		}
//	}
//
//
//
//	if  (state == ‘playing’) {
//		if (buttons.StartButtonPressed()) { 
//			setState(‘paused);
//			return; //  <---- para no continuar lo de abajo
//		}
//
//		display.batteryTimerFor(timeToNextBattery);
//
//		display.currentScore;
//
//		if (keys.left() && player.canleft?()) {
//			display.player( player.moveLeft() );
//			// donde player.moveLeft retorna (de 1 a 3) la nueva posición del player y display.player se encarga de dibujar.
//
//		}
//
//		if (keys.right() && player.canright?()) {
//			display.player( player.moveRight() );
//			// donde player.moveLeft retorna (de 1 a 3) la nueva posición del player y display.player se encarga de dibujar.
//
//		}
//
//		if (time.OtherCarsMoveNow()) {
//			arrayOfCars = otherCars.moveDown();
//			// donde otherCars.moveDown retorna un array con la posicion de los otros autos DESPUES de haberlos hecho bajar a todos.
//
//			display.otherCars(arrayOfCars)
//		}
//
//		if (colision.crashed()) {
//			setState(‘crashed’)
//		}
//	}
//
//	if (state == ‘paused’) {
//		display.ShowExit();
//
//		if (buttons.StartButtonPressed()) {
//			state == ‘playing’;
//			return
//			}
//
//		return
//		}
//
//
//	if (state == ‘crashed’) {
//		if (lives.left()) {
//			lives.decrease();
//			display.crashed();
//			sound.crashed();
//			pause.crashed();
//			otherCars.reset();
//			setState(‘playing’)
//			return;
//		}else{
//			bat.gameover();
//			display.gameover();
//			sound.gameover();
//			setState(‘gameoverStart’);
//			timeForMainMenu = now() + gameOverWait;
//		}
//	}
//
//	if (state == ‘gameover’) {
//		if (buttons.StartButtonPressed()) {
//			setState(‘mainmenu’);
//			return
//			}
//		if (now() < timeForMainMenu) {
//			setState(‘mainmenu’);
//			return
//
//			}
//	}


