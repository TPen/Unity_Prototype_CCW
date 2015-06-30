// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Chest object doing different effects on beeing touched by a player
//

using UnityEngine;
using System.Collections;

public class ChestObject : MonoBehaviour {

	public GameObject ExplosionPrefab;
	public GameObject BuffPrefab;
	public GameObject StunPrefab;

	private int type;

	private float spawnTime;

	private Color triggerColor;


	/// <summary>
	/// Start method
	/// </summary>
	void Start(){
		//Face camera
		transform.FindChild("Content").rotation = Camera.main.transform.rotation;

		//Start at zero scale for the spawn scaling effect
		transform.localScale = Vector3.zero;

		//Make the item a random type on spawn
		type = Mathf.RoundToInt(Random.Range(0.0f, 2.0f));

		Color col = Color.black;

		//Set color depending on type
		if(type == 0){
			col = new Color(1, 0.6f, 0.2f, 1);
		}else if(type == 1){
			col = new Color(0, 0.6f, 1, 1);
		}else if(type == 2){
			col = new Color(0.2f, 1, 0.6f, 1);
		}

		transform.FindChild("Content").GetComponent<MeshFilter>().mesh.colors =  new Color[4]{col, col, col, col};
		transform.FindChild("Content").GetChild(0).GetComponent<ParticleSystem>().startColor = col;
	}


	/// <summary>
	/// Do scaling as spawn effect
	/// </summary>
	void Update(){
		transform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, spawnTime);
		if(spawnTime < 1)spawnTime = Mathf.Clamp(spawnTime + Time.deltaTime * 2, 0, 1);
	}


	/// <summary>
	/// On beeing touched by a player
	/// </summary>
	void OnTriggerEnter(Collider other){
		if(other.tag.Equals ("Player")){
			GameObject arena = GameObject.FindGameObjectWithTag("Arena");
			triggerColor = other.GetComponent<CubeMovement>().EntityColor;

			//If bomb type
			if(type == 0){
				GameObject explosion = (GameObject) GameObject.Instantiate (ExplosionPrefab, transform.position, Quaternion.identity);
				explosion.GetComponent<ParticleSystem>().startColor = triggerColor;

				arena.GetComponent<ProcedualArena>().setRadius(transform.position, 3, triggerColor);
			//If speed buff type
			}else if(type == 1){
				GameObject buff = (GameObject) GameObject.Instantiate (BuffPrefab, transform.position, Quaternion.identity);
				buff.GetComponent<ParticleSystem>().startColor = triggerColor;

				other.GetComponent<CubeMovement>().Speed += 5;
			//If stun type
			}else if(type == 2){
				GameObject stun = (GameObject) GameObject.Instantiate (StunPrefab, transform.position, Quaternion.Euler (90, 0, 0));
				stun.GetComponent<ParticleSystem>().startColor = triggerColor;

				Collider[] entitiesInRange = Physics.OverlapSphere (transform.position, 5);

				for(int i = 0; i < entitiesInRange.Length; i++){
					if(entitiesInRange[i] == other)continue;
					if(entitiesInRange[i].GetComponent<CubeMovement>())entitiesInRange[i].GetComponent<CubeMovement>().Speed = 1;
				}
			}

			Destroy (this.gameObject);
		}
	}

}
