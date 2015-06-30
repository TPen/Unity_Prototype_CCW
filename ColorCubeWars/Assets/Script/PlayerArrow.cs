// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Small arrow identifying the player
//

using UnityEngine;
using System.Collections;

public class PlayerArrow : MonoBehaviour {

	public GameObject PlayerEntity;

	/// <summary>
	/// Set color and number string of the arrow
	/// </summary>
	void Start () {
		//Face camera
		transform.rotation = Camera.main.transform.rotation;

		Color col = PlayerEntity.GetComponent<CubeMovement>().EntityColor;

		GetComponent<MeshRenderer>().material.SetColor("_Color", col);
		transform.GetChild(0).GetComponent<TextMesh>().color = col;

		transform.GetChild(0).GetComponent<TextMesh>().text = "P" + (PlayerEntity.GetComponent<CubeMovement>().PlayerNumber + 1).ToString();
	}


	/// <summary>
	/// Update the position
	/// </summary>
	void Update () {
		if(!PlayerEntity){
			Destroy(gameObject);
		}else{
			transform.position = new Vector3(PlayerEntity.transform.position.x, 2.5f, PlayerEntity.transform.position.z);
		}
	}

}
