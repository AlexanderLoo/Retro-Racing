using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
	
	public bool CanLeft(int currentIndex){

		if (currentIndex > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool CanRight(int currentIndex, int maxIndex){

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
