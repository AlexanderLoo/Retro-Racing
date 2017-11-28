using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingPlayer : MonoBehaviour {

	public Transform leftPlayer, centerPlayer, rightPlayer;

	void Start(){

		Vector3 leftPosition = new Vector3 (-ScreenController.screen.screenWidth/2, -ScreenController.screen.screenHeight/3, 0);
		Vector3 centerPosition = new Vector3 (0, -ScreenController.screen.screenHeight/3, 0);
		Vector3 rightPosition = new Vector3 (ScreenController.screen.screenWidth/2, -ScreenController.screen.screenHeight/3, 0);

		leftPlayer.position = leftPosition;
		centerPlayer.position = centerPosition;
		rightPlayer.position = rightPosition;
	}
}
