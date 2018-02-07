using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//SE PIENSA MODIFICAR ESTAS VARIABLES CON UN CONTROLLER PARA USAR DIFERENTES SPRITES
	public Sprite sideSprite, centerSprite;
	public SpriteRenderer leftPlayer, centerPlayer, rightPlayer;

	//Lista para la otra mecánica
	public SpriteRenderer[] playerList;
	private int index = 1;
	public bool m2;

	void Start(){

		leftPlayer.sprite = sideSprite;
		centerPlayer.sprite = centerSprite;
		rightPlayer.sprite = sideSprite;
	}
	//Las siguientes funciones mueven al player
	public void MoveToLeft(){
		if (m2) {
			ChangePlayerPosition (-1);
		} else {
			EnabledSprite (leftPlayer, centerPlayer, rightPlayer);
		}
	}

	public void MoveToRight(){
		if (m2) {
			ChangePlayerPosition (1);
		} else {
			EnabledSprite (rightPlayer, centerPlayer, leftPlayer);
		}
	}

	public void StayInCenter(){
		if (!m2) {
			EnabledSprite (centerPlayer, leftPlayer, rightPlayer);
		}
	}

	void EnabledSprite(SpriteRenderer enabledSprite, SpriteRenderer disableSprite, SpriteRenderer disableSprite2){
		
		if (GameController.gameController.startGame) {
			enabledSprite.enabled = true;
			disableSprite.enabled = false;
			disableSprite2.enabled = false;
		}
	}
	//Funciones para la otra mecánica
	void ChangePlayerPosition(int dir){

		if (index + dir < 0 || index + dir > playerList.Length - 1) {
			return;
		} else {
			if (GameController.gameController.startGame) {
				playerList [index].enabled = false;
				playerList [index + dir].enabled = true;
				index += dir;
			}
		}
	}
}
