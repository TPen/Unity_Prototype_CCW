// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Class for handling the output of the results and drawing them onto the screen
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour {

	private bool showResults;

	private RectTransform box;
	private RectTransform resultLine;
	private RectTransform[ ]playerBars;
	private Image winningColor;

	private int step;
	private float stepProgress;

	private ProcedualArena arena;


	/// <summary>
	/// Get components and reference them into variables
	/// </summary>
	void Start () {
		arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<ProcedualArena>();

		box = GetComponent<RectTransform>();
		Transform content = transform.GetChild(0);
		resultLine = content.FindChild("Heading").GetChild (0).GetComponent<RectTransform>();
		playerBars = new RectTransform[4];
		playerBars[0] = content.FindChild("Player_1").GetChild (0).GetComponent<RectTransform>();
		playerBars[1] = content.FindChild("Player_2").GetChild (0).GetComponent<RectTransform>();
		playerBars[2] = content.FindChild("Player_3").GetChild (0).GetComponent<RectTransform>();
		playerBars[3] = content.FindChild("Player_4").GetChild (0).GetComponent<RectTransform>();
		winningColor = content.FindChild("Message").GetChild (0).GetComponent<Image>();

		step = 0;
		stepProgress = 0;
	}


	/// <summary>
	/// Update the score table
	/// </summary>
	void Update(){
		if(showResults){
			//Count up the step progress
			stepProgress = Mathf.Clamp (stepProgress + Time.deltaTime, 0, 1);

			if(step == 0){
				//Scale base box
				box.sizeDelta = Vector2.Lerp (new Vector2(0, 200), new Vector2(400, 200), stepProgress);
			}else if(step == 1){
				//Show result headline
				resultLine.parent.gameObject.SetActive(true);
				resultLine.sizeDelta = Vector2.Lerp (new Vector2(0, 1), new Vector2(300, 1), stepProgress);
			}else if(step > 1 && step < 6){
				//Draw the player results
				ProcedualArena.PlayerData data = arena.ColoredVertices[step - 2];
				if(data.Color.a != 0){
					float percent = (float) data.Vertices / ((arena.SizeX + 1) * (arena.SizeY + 1));

					playerBars[step - 2].parent.gameObject.SetActive(true);
					playerBars[step - 2].GetComponent<Image>().color = data.Color;
					playerBars[step - 2].sizeDelta = Vector2.Lerp (new Vector2(0, 20), new Vector2(280 * percent, 20), stepProgress);
					playerBars[step - 2].GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(Mathf.Lerp (0, percent, stepProgress) * 100).ToString () + "%";
				}else{
					stepProgress = 1;
				}
			}else if(step == 6){
				//Draw the winning message
				winningColor.transform.parent.gameObject.SetActive(true);
				winningColor.color = calculateTeamColor();

				stepProgress = 1;
			}

			//Reset progress and jump to the next step
			if(stepProgress == 1){
				stepProgress = 0;
				step ++;
			}
		}
	}


	/// <summary>
	/// Calculate the color of the winning team
	/// </summary>
	private Color calculateTeamColor(){
		ProcedualArena.PlayerData[] data = arena.ColoredVertices;
		int targetScore = 0;
		int targetData = 0;

		//Just go through all players scince teams share a score
		for(int i = 0; i < data.Length; i++){
			int actScore = data[i].Vertices;

			if(actScore > targetScore){
				targetScore = actScore;
				targetData = i;
			}
		}

		return data[targetData].Color;
	}


	/// <summary>
	/// Draw the results
	/// </summary>
	public void DrawResults(){
		GetComponent<AudioSource>().Play();

		showResults = true;
	}


	/// <summary>
	/// Resets the result screen
	/// </summary>
	public void ResetResults(){
		showResults = false;

		box.sizeDelta = Vector2.zero;
		resultLine.parent.gameObject.SetActive(false);
		resultLine.sizeDelta = Vector2.zero;

		for(int i = 0; i < playerBars.Length; i++){
			playerBars[i].parent.gameObject.SetActive(false);
			playerBars[i].sizeDelta = Vector2.zero;
			playerBars[i].GetChild(0).GetComponent<Text>().text = "0%";
		}

		winningColor.transform.parent.gameObject.SetActive(false);

		stepProgress = 0;
		step = 0;
	}

}
