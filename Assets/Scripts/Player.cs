using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
	private int maxIndex = 2;

	public bool CanLeft(int currentIndex){

		if (currentIndex > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool CanRight(int currentIndex){

		if (currentIndex < maxIndex) {
			return true;
		} else {
			return false;
		}
	}

	public int Movement(ref int currentIndex, int dir){
		currentIndex += dir;
		return currentIndex;
	}
}
