﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

	[SerializeField] AudioClip menuBip;

	[SerializeField] List<GameObject> pointers;
	private int pointer = 0;
	private int pointerLimit;

	private float verInput;
	private bool verPressed = false;
	private float fireInput;
	private bool firePressed = false;

	void Start () {
		pointerLimit = pointers.Count - 1;
		pointer = pointerLimit;

		UpdateDisplay ();

		if (Input.GetAxisRaw ("Fire1") == 1) {
			firePressed = true;
		}
	}


	void Update () {
		InputDetection ();

		Confirm ();
		MovePointer ();
	}

	//General functions of menus______________________________________________________________
	void InputDetection () {
		verInput = Input.GetAxisRaw ("Vertical");

		//To allow Enter as a confirmation key, as well as fire
		fireInput = Input.GetAxisRaw ("Fire1");
		if (fireInput == 0) {
			firePressed = false;
		}
		if (Input.GetKeyDown (KeyCode.Return)) {
			fireInput = 1;
		}
	}

	void MovePointer () {
		if (verInput == 0) {
			verPressed = false;
		}
		if (verInput == 1) {
			if (!verPressed) {
				PointerUp ();
				UpdateDisplay ();
			}
			verPressed = true;
		}
		if (verInput == -1) {
			if (!verPressed) {
				PointerDown ();
				UpdateDisplay ();
			}
			verPressed = true;
		}		
	}

	void PointerUp () {
		pointer++;
		SoundManager.Instance.PlaySFX (menuBip);
		if (pointer > pointerLimit) {
			pointer = 0;
		}
	}
	void PointerDown () {
		pointer--;
		SoundManager.Instance.PlaySFX (menuBip);
		if (pointer < 0) {
			pointer = pointerLimit;
		}
	}

	void UpdateDisplay() {
		for (int i = 0; i <= pointerLimit; i++) {
			if (pointer == i) {
				pointers [i].SetActive (true);
			} else {
				pointers [i].SetActive (false);
			}
		}
	}


	//Depends on menu__________________________________________________________________________
	void Confirm () {
		if (!firePressed && fireInput == 1) {
			SoundManager.Instance.PlaySFX (menuBip);
			switch (pointer) {
			case 0:
				MainMenu ();
				break;
			case 1: 
				Retry ();
				break;
			default:
				Debug.Log ("Pointer out of bounds");
				break;
			}
		}
	}

	void MainMenu () {
		GameManager.Instance.MainMenu ();
	}

	void Retry () {
		GameManager.Instance.DragunScene ();
	}
}
