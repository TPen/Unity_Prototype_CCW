// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Small panel for selecting colors
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour {
	
	public GameObject PlayerSelect;


	/// <summary>
	/// Set a color to a player
	/// </summary>
	public void setColor(Image image){
		if(!PlayerSelect)return;
		PlayerSelect.transform.FindChild ("Color").GetComponent<Image>().color = image.color;
		PlayerSelect.transform.FindChild ("Player").GetComponent<Text>().color = image.color;
		PlayerSelect.GetComponent<PlayerSelect>().UpdatePlayer();
	}

}
