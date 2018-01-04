using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeToMain : MonoBehaviour {

	public float timeToChange = 5;

	void Start(){

		Invoke ("ChangeScene", timeToChange);
	}


	void ChangeScene(){

		SceneManager.LoadScene ("Main");
	}
}
