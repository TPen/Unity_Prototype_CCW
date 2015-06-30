// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Kill particle once its lifetime is over
//

using UnityEngine;
using System.Collections;

public class ParticleSelfKill : MonoBehaviour {
	/// <summary>
	/// Update loop
	/// </summary>	
	void Update () {
		if(GetComponent<ParticleSystem>().isStopped)Destroy (gameObject);
	}
}
