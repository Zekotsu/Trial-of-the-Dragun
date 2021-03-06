﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragunBody : MonoBehaviour {

	[SerializeField] float wiggleAmp = 0.5f; //head = 3
	//for phase 2
	[SerializeField] float wiggleAmp2 = 1; //head = 3
	private float amplitude;

	[SerializeField] float wiggleTime = 1; //head = 3
	//for phase 2, head only
	[SerializeField] float wiggleTime2 = 1; //head = 2
	float wiggleTimeFactor;
	[SerializeField] float wiggleOffSet;

	[SerializeField] private float vibrateAmplitude = 0.1f;
	private Vector3 vibrateDeviation;
	private Vector3 initialVibratePosition;
	[SerializeField] private GameObject boom;

	[SerializeField] private Dragun dragun;
	[SerializeField] private Transform parent;

	[SerializeField] private int damage = 1;
	[SerializeField] private float blinkTime = 0.1f;
	private SpriteRenderer sr;
	//For the head
	[SerializeField] private bool isHead = false;
	[SerializeField] private List<SpriteRenderer> parts;
	private Shader shaderGUItext;
	private Shader shaderSpritesDefault;


	private float time;


	void Start () {
		amplitude = wiggleAmp;
		time = wiggleOffSet * Mathf.PI;
		wiggleTimeFactor = 2 * Mathf.PI / wiggleTime;

		vibrateDeviation = new Vector3 (0, vibrateAmplitude, 0);
		if (boom != null) {
			boom.SetActive (false);
		}

		sr = this.GetComponent<SpriteRenderer> ();
		shaderGUItext = Shader.Find("GUI/Text Shader");
		shaderSpritesDefault = Shader.Find("Sprites/Default");
	}
	
	//Movement___________________________________________________________________________________
	void Update () {
		MoveWiggle ();
		time += Time.deltaTime;
	}

	void MoveWiggle ()
	{
		float newY = parent.position.y + amplitude * Mathf.Sin (time * wiggleTimeFactor);
		float x = this.transform.position.x;
		float z = this.transform.position.z;
		Vector3 pos = new Vector3 (x, newY, z);

		this.transform.position = pos; 
	}

	public void Phase2 () {
		amplitude = wiggleAmp2;
		if (wiggleTime2 != 0) {
			wiggleTimeFactor = 2 * Mathf.PI / wiggleTime2;
		} else {
			Debug.Log (this.name);
		}
	}

	public void Vibrate () {
		initialVibratePosition = this.transform.position;
		StartCoroutine (VibrateCoroutine ());
	}
	public void VibrateStop () {
		StopAllCoroutines ();
		AntiBlink ();
		this.transform.position = initialVibratePosition;
	}
	IEnumerator VibrateCoroutine () {
		int vib = 1;
		while (true) {
			this.transform.position = initialVibratePosition + vib * vibrateDeviation;
			yield return new WaitForSeconds (0.05f);
			vib = -1 * vib;
		}
	}
	public void VibrateFall (float movement) {
		initialVibratePosition -= new Vector3 (0, movement, 0);
	}


	public void Explode () {
		if (boom != null) {
			boom.SetActive (true);
		}
	}

	//Interactions_______________________________________________________________________________
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "PlayerBullet") {
			dragun.TakeDamage (damage);
			if (isHead) {
				StartCoroutine (BlinkHead ());
			} else {
				StartCoroutine (Blink ());
			}
		}
	}

	IEnumerator Blink () {
		//Turns the sprite white
		sr.material.shader = shaderGUItext;
		sr.color = Color.white;

		yield return new WaitForSeconds (blinkTime);

		//returns the sprite to normal
		sr.material.shader = shaderSpritesDefault;
		sr.color = Color.white;
	}
	public void ClawBlink () {
		//To be called by a claw
		StartCoroutine (Blink ());
	}

	IEnumerator BlinkHead () {
		foreach (SpriteRenderer render in parts) {
			render.material.shader = shaderGUItext;
			render.color = Color.white;
		}
		yield return new WaitForSeconds (blinkTime);
		foreach (SpriteRenderer render in parts) {
			render.material.shader = shaderSpritesDefault;
			render.color = Color.white;
		}
	}
	void AntiBlink (){
		if (isHead) {
			foreach (SpriteRenderer render in parts) {
				render.material.shader = shaderSpritesDefault;
				render.color = Color.white;
			}
		} else {
			sr.material.shader = shaderSpritesDefault;
			sr.color = Color.white;
		}
	}


	public void Stop () {
		this.enabled = false;
	}
	public void Restart () {
		this.enabled = true;
	}

}
