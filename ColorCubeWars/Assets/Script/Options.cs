// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Small properties window to change game settings
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	public GameObject ResultScreen;

	private ProcedualArena arena;


	/// <summary>
	/// Start method
	/// </summary>	
	void Start(){
		arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<ProcedualArena>();
	}


	/// <summary>
	/// Toogle the options menu
	/// </summary>	
	public void ToogleOptions(){
		GetComponent<AudioSource>().Play();

		ResultScreen.GetComponent<ResultScreen>().ResetResults();
		arena.ClearArena();
		arena.UpdatePlayerStartPosition();
		transform.FindChild("Content").gameObject.SetActive(! transform.FindChild("Content").gameObject.activeSelf);
	}


	/// <summary>
	/// Sets the options menu
	/// </summary>	
	public void SetOptions(bool value){
		transform.FindChild("Content").gameObject.SetActive(value);
	}


	/// <summary>
	/// Change tilesize x
	/// </summary>	
	public void ChangeTileSizeX(InputField sender){
		GetComponent<AudioSource>().Play();

		if(sender.text.Length > 0){
			arena.SizeX = (int) Mathf.Clamp(int.Parse(sender.text), 10, 25);
			arena.GenerateArena();
		}

		//Set the text to the new size
		sender.text = arena.SizeX.ToString();

		arena.UpdatePlayerStartPosition();
	}


	/// <summary>
	/// Change tilesize y
	/// </summary>	
	public void ChangeTileSizeY(InputField sender){
		GetComponent<AudioSource>().Play();

		if(sender.text.Length > 0){
			arena.SizeY = (int) Mathf.Clamp(int.Parse(sender.text), 10, 25);
			arena.GenerateArena();
		}
		
		sender.text = arena.SizeY.ToString();

		arena.UpdatePlayerStartPosition();
	}


	
	/// <summary>
	/// Change spawn rate
	/// </summary>	
	public void ChangeSpawnRate(InputField sender){
		GetComponent<AudioSource>().Play();

		if(sender.text.Length > 0){
			arena.ChestSpawnChance = Mathf.Clamp(int.Parse(sender.text) / 100.0f, 0.0f, 1.0f);
		}
		
		sender.text = Mathf.RoundToInt(arena.ChestSpawnChance * 100).ToString();;
	}


	/// <summary>
	/// Change time limit
	/// </summary>	
	public void ChangeTimeLimit(InputField sender){
		GetComponent<AudioSource>().Play();

		if(sender.text.Length > 0){
			arena.TimeLimit = (int) Mathf.Clamp(int.Parse(sender.text), 30, 300);
		}
		
		sender.text = arena.TimeLimit.ToString();;
	}


	/// <summary>
	/// Change double speed
	/// </summary>	
	public void ChangeDoubleSpeed(bool value){
		GetComponent<AudioSource>().Play();

		arena.DoubleSpeed = value;
	}

}
