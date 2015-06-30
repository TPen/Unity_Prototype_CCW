// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Class handling the timer and the display of how many percent of the arena consists of the different colors;
// Also in charge of starting and ending the game
//

using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerButton : MonoBehaviour {

	public GameObject ColorPanel;
	public GameObject ResultScreen;
	public GameObject Options;
	public GameObject Background;
	public Material PercentageRing;

	private int timeLimit;
	private int vertexCount;

	private ProcedualArena arena;
	private StringBuilder timeString;
	private Material material;


	/// <summary>
	/// Start method
	/// </summary>	
	void Start(){
		arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<ProcedualArena>();
		timeString = new StringBuilder(10);
		material = new Material(PercentageRing);
		GetComponent<Image>().material = material;
	}


	/// <summary>
	/// Update the color rings material with input of the values given by the arena
	/// </summary>
	void Update(){
		//Bring back the outline to the default value
		material.SetFloat("_Outline", Mathf.MoveTowards (material.GetFloat("_Outline"), 0.9f, Time.deltaTime));

		if(!arena.GameStarted)return;

		ProcedualArena.PlayerData[] data = arena.ColoredVertices;

		bool[] skip = new bool[data.Length];

		Color backCol = Color.black;

		for(int i = 0; i < data.Length; i++){
			if(data[i].Color.a == 0 || skip[i])continue;
			Color sCol = data[i].Color;

			for(int j = i + 1; j < data.Length; j++){
				if(sCol == data[j].Color)skip[j] = true;
			}

			sCol.a = (float) data[i].Vertices / vertexCount;

			backCol += sCol * sCol.a;

			//If instead of string appending (it produces nothing for the gc this way)
			if(i == 0){
				material.SetColor("_Col0", sCol);
			}else if(i == 1){
				material.SetColor("_Col1", sCol);
			}else if(i == 2){
				material.SetColor("_Col2", sCol);
			}else if(i == 3){
				material.SetColor("_Col3", sCol);
			}
		}

		Background.GetComponent<MeshRenderer>().material.SetColor("_Color", backCol);
	}


	/// <summary>
	/// On beeing clicked
	/// </summary>
	public void onClick(){
		material.SetFloat("_Outline", 0.5f);

		if(arena.GameStarted){
			PauseGame();
		}else{
			if(IsInvoking("startTimer"))return;

			arena.ClearArena();
			arena.UpdatePlayerStartPosition();

			//Reset the material
			material = new Material(PercentageRing);
			GetComponent<Image>().material = material;

			Background.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 0, 0, 0));

			//Reset the result screen
			ResultScreen.GetComponent<ResultScreen>().ResetResults();

			vertexCount = (arena.SizeX + 1) * (arena.SizeY + 1);
			ColorPanel.SetActive(false);
			Options.SetActive(false);
			Options.GetComponent<Options>().SetOptions(false);

			timeLimit = 4;
			InvokeRepeating("startTimer", 0, 1.0f);
		}

	}


	/// <summary>
	/// Set the timescale to pause everything (monobehaviours particles etc.)
	/// </summary>
	private void PauseGame(){
		if(Time.timeScale == 0){
			Time.timeScale = 1;
			updateTime();
		}else{
			Time.timeScale = 0;
			transform.GetChild(0).GetComponent<Text>().text = "Stop";
		}
	}


	/// <summary>
	/// Count down time until it reaches zero
	/// </summary>
	private void countDown(){
		timeLimit --;

		arena.SpawnChest();
		
		updateTime();

		if(timeLimit == 0){
			CancelInvoke("countDown");
			ResultScreen.GetComponent<ResultScreen>().DrawResults();
			arena.GameStarted = false;
			Options.SetActive(true);
			transform.GetChild(0).GetComponent<Text>().text = "Start";
		}
	}


	/// <summary>
	/// Start timer
	/// </summary>
	private void startTimer(){
		GetComponent<AudioSource>().Play();
		
		timeLimit --;
		
		timeString.Length = 0;

		if(timeLimit > 0){
			timeString.Append(timeLimit);
		}else{
			timeString.Append("Go");
		}
		transform.GetChild(0).GetComponent<Text>().text = timeString.ToString();

		if(timeLimit == 0){
			arena.StartGame();
			timeLimit = arena.TimeLimit + 1;

			CancelInvoke("startTimer");
			InvokeRepeating("countDown", 0.5f, 1.0f);
		}
	}


	/// <summary>
	/// Updates the time string on demand
	/// </summary>
	public void updateTime(){
		timeString.Length = 0;
		timeString.Append(timeLimit / 60);
		timeString.Append(":");
		if(timeLimit % 60 < 10){
			timeString.Append("0");
		}
		timeString.Append (timeLimit % 60);
		transform.GetChild(0).GetComponent<Text>().text = timeString.ToString();
	}


}
