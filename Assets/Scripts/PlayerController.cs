using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//SE PIENSA MODIFICAR ESTAS VARIABLES CON UN CONTROLLER PARA USAR DIFERENTES SPRITES
	public Sprite sideSprite, centerSprite;
	public SpriteRenderer leftPlayer, centerPlayer, rightPlayer;

	void Start(){

		leftPlayer.sprite = sideSprite;
		centerPlayer.sprite = centerSprite;
		rightPlayer.sprite = sideSprite;
		StayInCenter ();
	}
	//Las siguientes funciones mueven al player
	public void MoveToLeft(){
		EnabledSprite (leftPlayer, centerPlayer, rightPlayer);
	}

	public void MoveToRight(){
		EnabledSprite (rightPlayer, centerPlayer, leftPlayer);
	}

	public void StayInCenter(){
		EnabledSprite (centerPlayer, leftPlayer, rightPlayer);
	}

	void EnabledSprite(SpriteRenderer enabledSprite, SpriteRenderer disableSprite, SpriteRenderer disableSprite2){
		if (GameController.gameController.inGame) {
			enabledSprite.enabled = true;
			disableSprite.enabled = false;
			disableSprite2.enabled = false;
		}
	}
}
