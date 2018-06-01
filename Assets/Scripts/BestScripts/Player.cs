using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int currentIndex = 1;
	private int maxIndex = 2;

	public bool CanLeft(){

		if (currentIndex > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool CanRight(){

		if (currentIndex < maxIndex) {
			return true;
		} else {
			return false;
		}
	}

	public int Movement(int dir){
		currentIndex += dir;
		return currentIndex;
	}
}
