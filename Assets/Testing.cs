using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour {

	public SpriteRenderer[] background;

	public void DeleteMemory(){

		PlayerPrefs.DeleteAll ();
	}

	public void ActiveBackground(){

		foreach (SpriteRenderer item in background) {
			item.enabled = !item.enabled;
		}
	}

}
