// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Small class to display the generate button for the arena in the editor
//

#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ProcedualArena))]
public class ProcedualArenaEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		//Make a button for generating the arena floor
		ProcedualArena arena = (ProcedualArena)target;
		if(GUILayout.Button("Generate")){
			arena.GenerateArena();
		}
	}
}

#endif
