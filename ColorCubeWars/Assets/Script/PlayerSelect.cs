// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// UI Class for new player entry
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour {

	public int PlayerNumber;

	public bool PlayerJoined;

	public GameObject ColorPanel;

	private CubeMovement.InputStruct InputKeys;

	private int setInput;

	private ProcedualArena arena;


	void Start(){
		arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<ProcedualArena>();

		//Set standart keys
		if(PlayerNumber == 1){
			InputKeys.UpKey = KeyCode.W;
			InputKeys.LeftKey = KeyCode.A;
			InputKeys.DownKey = KeyCode.S;
			InputKeys.RightKey = KeyCode.D;
		}else if(PlayerNumber == 2){
			InputKeys.UpKey = KeyCode.UpArrow;
			InputKeys.LeftKey = KeyCode.LeftArrow;
			InputKeys.DownKey = KeyCode.DownArrow;
			InputKeys.RightKey = KeyCode.RightArrow;
		}else if(PlayerNumber == 3){
			InputKeys.UpKey = KeyCode.I;
			InputKeys.LeftKey = KeyCode.J;
			InputKeys.DownKey = KeyCode.K;
			InputKeys.RightKey = KeyCode.L;
		}else if(PlayerNumber == 4){
			InputKeys.UpKey = KeyCode.Keypad8;
			InputKeys.LeftKey = KeyCode.Keypad4;
			InputKeys.DownKey = KeyCode.Keypad5;
			InputKeys.RightKey = KeyCode.Keypad6;
		}

		if(PlayerJoined)Activate();

		updateKeyText();
	}


	public void Update(){
		if(PlayerJoined || arena.GameStarted && !PlayerJoined)transform.FindChild ("Join").gameObject.SetActive(false);
		else transform.FindChild ("Join").gameObject.SetActive(true);
	}

	/// <summary>
	/// Use on gui only to set the keys with the event type (OnGUI really expensive otherwise)
	/// </summary>
	public void OnGUI(){
		if(! arena.GameStarted && setInput > 0){
			if(Event.current.type == EventType.keyDown){
				GetComponent<AudioSource>().Play();
				
				if(setInput == 1)InputKeys.UpKey = Event.current.keyCode;
				else if(setInput == 2)InputKeys.LeftKey = Event.current.keyCode;
				else if(setInput == 3)InputKeys.DownKey = Event.current.keyCode;
				else if(setInput == 4)InputKeys.RightKey = Event.current.keyCode;

				updateKeyText();

				UpdatePlayer();

				setInput = 0;
			}
		}
	}


	/// <summary>
	/// Updates the text on the keys
	/// </summary>
	private void updateKeyText(){
		transform.FindChild ("Controls").FindChild ("Up").GetComponent<Text>().text = setKeyText(InputKeys.UpKey);
		transform.FindChild ("Controls").FindChild ("Left").GetComponent<Text>().text = setKeyText(InputKeys.LeftKey);
		transform.FindChild ("Controls").FindChild ("Down").GetComponent<Text>().text = setKeyText(InputKeys.DownKey);
		transform.FindChild ("Controls").FindChild ("Right").GetComponent<Text>().text = setKeyText(InputKeys.RightKey);
	}


	/// <summary>
	/// Method for setting special symbols for certain input
	/// </summary>
	private string setKeyText(KeyCode key){
		if(key == KeyCode.UpArrow)return "▲";
		else if(key == KeyCode.LeftArrow)return "◄";
		else if(key == KeyCode.DownArrow)return "▼";
		else if(key == KeyCode.RightArrow)return "►";
		else if(key == KeyCode.Keypad0)return "0";
		else if(key == KeyCode.Keypad1)return "1";
		else if(key == KeyCode.Keypad2)return "2";
		else if(key == KeyCode.Keypad3)return "3";
		else if(key == KeyCode.Keypad4)return "4";
		else if(key == KeyCode.Keypad5)return "5";
		else if(key == KeyCode.Keypad6)return "6";
		else if(key == KeyCode.Keypad7)return "7";
		else if(key == KeyCode.Keypad8)return "8";
		else if(key == KeyCode.Keypad9)return "9";
		return key.ToString();
	}


	/// <summary>
	/// On activate player
	/// </summary>
	public void Activate(){
		if(arena.GameStarted)return;

		GetComponent<AudioSource>().Play();
		
		transform.FindChild ("Player").gameObject.SetActive(true);
		transform.FindChild ("Color").gameObject.SetActive(true);
		transform.FindChild ("Controls").gameObject.SetActive(true);

		arena.SpawnPlayer(PlayerNumber - 1, transform.FindChild("Color").GetComponent<Image>().color, InputKeys);

		PlayerJoined = true;
	}

	
	/// <summary>
	/// Remove player
	/// </summary>
	public void Deavtivate(){
		if(arena.GameStarted)return;

		GetComponent<AudioSource>().Play();
		
		transform.FindChild ("Player").gameObject.SetActive(false);
		transform.FindChild ("Color").gameObject.SetActive(false);
		transform.FindChild ("Controls").gameObject.SetActive(false);

		arena.RemovePlayer(PlayerNumber - 1);

		PlayerJoined = false;
	}


	/// <summary>
	/// Display the color panel
	/// </summary>
	public void ShowColorPanel(){
		if(arena.GameStarted)return;

		GetComponent<AudioSource>().Play();

		if(ColorPanel.activeSelf && ColorPanel.GetComponent<ColorPanel>().PlayerSelect == gameObject){
			ColorPanel.SetActive (false);
		}else{
			ColorPanel.SetActive (true);
			if(PlayerNumber % 2 == 1)ColorPanel.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + Vector3.right * Screen.width / 8.0f;
			else ColorPanel.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + Vector3.left * Screen.width / 8.0f;
			ColorPanel.GetComponent<ColorPanel>().PlayerSelect = gameObject;
		}
	}


	/// <summary>
	/// Force player update
	/// </summary>
	public void UpdatePlayer(){
		arena.UpdatePlayer(PlayerNumber - 1, transform.FindChild("Color").GetComponent<Image>().color, InputKeys);
	}


	/// <summary>
	/// Set specific input key
	/// </summary>
	public void SetInputKey(int number){
		if(arena.GameStarted)return;

		GetComponent<AudioSource>().Play();
		
		setInput = number;

		updateKeyText();

		if(setInput == 1)transform.FindChild ("Controls").FindChild ("Up").GetComponent<Text>().text = "";
		else if(setInput == 2)transform.FindChild ("Controls").FindChild ("Left").GetComponent<Text>().text = "";
		else if(setInput == 3)transform.FindChild ("Controls").FindChild ("Down").GetComponent<Text>().text = "";
		else if(setInput == 4)transform.FindChild ("Controls").FindChild ("Right").GetComponent<Text>().text = "";
	}

}
