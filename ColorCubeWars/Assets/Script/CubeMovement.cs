// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// The player movement input
//

using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {

	public struct InputStruct{		
		public KeyCode UpKey;
		public KeyCode LeftKey;
		public KeyCode DownKey;
		public KeyCode RightKey;
	}

	public InputStruct InputKeys;

	public Color EntityColor;

	public int PlayerNumber;

	public float Speed = 1;

	public GameObject PlayerArrowPrefab;

	private Vector3 lastPosition, newPosition;
	private Quaternion lastRotation, newRotation;

	private float stepTime;

	private ProcedualArena arena;


	/// <summary>
	/// Get the mesh colors back to the color array
	/// </summary>
	void Start(){
		arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<ProcedualArena>();
	}


	/// <summary>
	/// On starting the game
	/// </summary>
	public void OnStart(){
		GameObject PArrow = (GameObject) GameObject.Instantiate (PlayerArrowPrefab, transform.position, Quaternion.identity);
		PArrow.GetComponent<PlayerArrow>().PlayerEntity = gameObject;
		
		UpdateColor();

		Speed = 1;

		//The positions will be zero / no rotation at the beginning
		newPosition = transform.position;
		newRotation = Quaternion.identity;
						
		setNext();
	}


	/// <summary>
	/// Update the movement
	/// </summary>
	void Update(){
		if(!arena.GameStarted)return;

		//Lerp the position and rotation for the step
		transform.rotation = Quaternion.Lerp (lastRotation, newRotation, stepTime);
		transform.position = Vector3.Lerp(lastPosition, newPosition, stepTime);

		if(newRotation != lastRotation)transform.position += Vector3.up * Mathf.Sin(stepTime * Mathf.PI) * 0.25f;

		if(arena.DoubleSpeed)stepTime += Time.deltaTime * Speed * 2;
		else stepTime += Time.deltaTime * Speed * 2;

		//If step time is bigger than one calculate the next step
		if(stepTime >= 1){
			stepTime = 0;
			setNext ();
		}
	}


	/// <summary>
	/// Set player to start position
	/// </summary>
	public void setStartPosition(ProcedualArena arenaComponent){
		arena = arenaComponent;

		Vector3 spawnPosition = arena.transform.position + Vector3.one * 0.5f;
		
		if(PlayerNumber == 0)spawnPosition += new Vector3(1, 0, arena.SizeY - 2);
		else if(PlayerNumber == 1)spawnPosition += new Vector3(arena.SizeX - 2, 0, arena.SizeY - 2);
		else if(PlayerNumber == 2)spawnPosition += new Vector3(1, 0, 1);
		else if(PlayerNumber == 3)spawnPosition += new Vector3(arena.SizeX - 2, 0, 1);
		
		transform.position = spawnPosition;
		transform.rotation = Quaternion.identity;
	}


	/// <summary>
	/// Calculate next step
	/// </summary>
	private void setNext(){
		Vector3 dir = Vector3.zero;

		//Get inputs
		if(Input.GetKey (InputKeys.UpKey))dir.z = 1;
		else if(Input.GetKey (InputKeys.LeftKey))dir.x = -1;
		else if(Input.GetKey (InputKeys.DownKey))dir.z = -1;
		else if(Input.GetKey (InputKeys.RightKey))dir.x = 1;
		else Speed = 1;

		if(Speed < 5)Speed = Mathf.Clamp (Speed + 0.2f, 1, 5);
		else Speed = Speed - 0.2f;

		lastPosition = newPosition;
		newPosition = lastPosition + dir;

		lastRotation = newRotation;
		newRotation = Quaternion.AngleAxis (90, new Vector3(dir.z, 0, -dir.x)) * lastRotation;

		//If moving set the color of the tile below if not wait for new input
		if(dir != Vector3.zero){
			arena.setTile(transform.position, EntityColor);
			GetComponent<AudioSource>().Play();
		}else{
			stepTime = 1;
		}

		//If object would hit the bounds or would collide with other entity reset speed and stop movement
		if(! arena.inBounds (newPosition) || arena.wouldCollide(gameObject, newPosition)){
			Speed = 1;
			newPosition = lastPosition;
			return;
		}
	}


	/// <summary>
	/// Check if object is in bounds
	/// </summary>
	public bool BlocksTile(Vector3 field){
		return field == lastPosition || field == newPosition;
	}


	/// <summary>
	/// Updates the color
	/// </summary>
	public void UpdateColor(){
		GetComponent<MeshRenderer>().material.SetColor("_Color", EntityColor);
	}
	
}
