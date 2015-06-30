// Copyright (C) 2015, Felix Kate; All rights reserved
//
// <Class content>
// Class for handling the generation of the arena and as interface between the different classes to
// communicate if there is an active running game etc.
//

using UnityEngine;
using System.Collections;

public class ProcedualArena : MonoBehaviour {

	public struct PlayerData{
		public Color Color;
		public int Vertices;
	}

	public int TimeLimit = 60;
	public float ChestSpawnChance = 0.5f;
	public bool DoubleSpeed = false;

	public bool GameStarted;

	public PlayerData[] ColoredVertices = new PlayerData[4];

	public GameObject PlayerPrefab;
	public GameObject ChestPrefab;
	
	public int SizeX, SizeY;

	public Material FloorMaterial;

	private Mesh mesh, edge;

	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uvs1;
	private Vector2[] uvs2;
	private Color[] colors;

	private GameObject[] players = new GameObject[4];

	private bool canClear;

	private bool recalculateColors;


	// Game related methods
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Get the mesh colors back to the color array
	/// </summary>
	void Start(){
		mesh = GetComponent<MeshFilter>().mesh;
		colors = mesh.colors;

		GameStarted = false;
	}


	/// <summary>
	/// Starts the game
	/// </summary>
	public void StartGame(){
		if(GameStarted)return;

		//Set up the players
		for(int i = 0; i < players.Length; i++){
			if(!players[i])continue;
			players[i].GetComponent<CubeMovement>().OnStart();

			ColoredVertices[i].Color = players[i].GetComponent<CubeMovement>().EntityColor;
		}

		//Set the game to be started
		GameStarted = true;

		canClear = true;
	}


	/// <summary>
	/// Update method to watch over the color array
	/// </summary>
	void Update(){
		//Recalculate all colors on the field
		if(recalculateColors){
			//Calculate the sum of the colors of the players and reset the colored vertices amount
			float[] playerColorSum = new float[players.Length];
			for(int i = 0; i < players.Length; i++){
				playerColorSum[i] = ColoredVertices[i].Color.r + ColoredVertices[i].Color.g + ColoredVertices[i].Color.b;
				ColoredVertices[i].Vertices = 0;
			}

			for(int i = 0; i < colors.Length; i++){
				//Decrease the alpha to make a fade out effect
				colors[i].a = Mathf.Clamp(colors[i].a - Time.deltaTime * 0.5f, 0, 1);

				//Count how many tiles are set to the player colors
				for(int j = 0; j < players.Length; j++){

					//Only compare sums because it would be 3 checks otherwise
					float tileColSum = colors[i].r + colors[i].g + colors[i].b;

					if(playerColorSum[j] == tileColSum){
						ColoredVertices[j].Vertices++;
					}
				}
			}		

			recalculateColors = false;

		//Only do the fade out
		}else{
			for(int i = 0; i < colors.Length; i++){
				//Decrease the alpha to make a fade out effect
				colors[i].a = Mathf.Clamp(colors[i].a - Time.deltaTime * 0.5f, 0, 1);
			}		
		}

		mesh.colors = colors;
	}

	
	/// <summary>
	/// Spawns a player
	/// </summary>
	public void SpawnPlayer(int playerIndex, Color playerColor, CubeMovement.InputStruct keys){
		if(GameStarted)return;

		GameObject spawnedPlayer = (GameObject) GameObject.Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
		
		players[playerIndex] = spawnedPlayer;
		CubeMovement cMove = spawnedPlayer.GetComponent<CubeMovement>();

		cMove.EntityColor = playerColor;
		cMove.PlayerNumber = playerIndex;
		cMove.InputKeys = keys;		
		cMove.UpdateColor();
		cMove.setStartPosition(this);
	}


	/// <summary>
	/// Remove player
	/// </summary>
	public void RemovePlayer(int playerIndex){
		if(GameStarted)return;
		
		Destroy(players[playerIndex]);
	}


	/// <summary>
	/// Spawn a chest
	/// </summary>
	public void SpawnChest(){
		//Spawn chest object with a chance
		if(Random.Range(0.0f, 1.0f) <= ChestSpawnChance){
			Vector3 spawnPos = new Vector3(Random.Range (0, SizeX), 0, Random.Range (0, SizeY));
			GameObject.Instantiate(ChestPrefab, transform.position + spawnPos + Vector3.one * 0.5f, Quaternion.identity);
		}
	}


	/// <summary>
	/// Force an update on the player color
	/// </summary>
	public void UpdatePlayer(int playerIndex, Color playerColor, CubeMovement.InputStruct keys){
		if(GameStarted)return;

		CubeMovement cMove = players[playerIndex].GetComponent<CubeMovement>();
		cMove.EntityColor = playerColor;
		cMove.InputKeys = keys;
		cMove.GetComponent<CubeMovement>().UpdateColor();
	}


	/// <summary>
	/// Force all players into their start positions
	/// </summary>
	public void UpdatePlayerStartPosition(){
		if(GameStarted)return;

		for(int i = 0; i < players.Length; i++){
			if(!players[i])continue;
			players[i].GetComponent<CubeMovement>().setStartPosition(this);
		}
	}




	// Coloring and collision checks
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Set color of specific tile
	/// </summary>
	public void setTile(Vector3 entityPosition, Color color){
		Vector3 posOnGrid = entityPosition - transform.position;

		int x = Mathf.FloorToInt (posOnGrid.x);
		int y = Mathf.FloorToInt (posOnGrid.z);

		setColor (x, y, color);

		recalculateColors = true;
	}


	/// <summary>
	/// Set colors in a radius
	/// </summary>
	public void setRadius(Vector3 entityPosition, int radius, Color color){
		Vector3 posOnGrid = entityPosition - transform.position;
		
		int x = Mathf.FloorToInt (posOnGrid.x);
		int y = Mathf.FloorToInt (posOnGrid.z);

		// Besham algorithm code taken from https://de.wikipedia.org/wiki/Bresenham-Algorithmus
		while(radius > 0){
			int f = 1 - radius;
			int ddF_x = 0;
			int ddF_y = -2 * radius;
			int i = 0;
			int j = radius;

			while(i < j){
				if(f >= 0){
					j--;
					ddF_y += 2;
					f += ddF_y;
				}

				i++;
				ddF_x += 2;
				f += ddF_x + 1;

				setColor (x + i, y + j, color);
				setColor (x - i, y + j, color);
				setColor (x + i, y - j, color);
				setColor (x - i, y - j, color);
				setColor (x + j, y + i, color);
				setColor (x - j, y + i, color);
				setColor (x + j, y - i, color);
				setColor (x - j, y - i, color);
			}
			radius--;
		}

		recalculateColors = true;
	}


	/// <summary>
	/// Clear all the tiles
	/// </summary>
	public void ClearArena(){
		if(!canClear)return;
		canClear = false;

		//Destroy all one round objects and clear old tiles
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("OneRound")){
			Destroy(obj);
		}

		//Clear all tiles
		for(int i = 0; i < colors.Length; i++){
			colors[i] = new Color(0.05f, 0.05f, 0.05f, 1);
		}

		recalculateColors = true;
	}


	/// <summary>
	/// Check if object is in bounds
	/// </summary>
	public bool inBounds(Vector3 entityPosition){
		Vector3 posOnGrid = entityPosition - transform.position;
		
		int x = Mathf.FloorToInt (posOnGrid.x);
		int y = Mathf.FloorToInt (posOnGrid.z);

		return (x >= 0 && x < SizeX && y >= 0 && y < SizeY);
	}


	/// <summary>
	/// Check if object would hit another entity
	/// </summary>
	public bool wouldCollide(GameObject entity, Vector3 entityPosition){
		for(int i = 0; i < players.Length; i++){
			if(!players[i] || entity == players[i])continue;
			if(players[i].GetComponent<CubeMovement>().BlocksTile(entityPosition))return true;
		}
		
		return false;
	}


	/// <summary>
	/// Set the 4 vertex colors around a specific tile
	/// </summary>
	private void setColor(int x, int y, Color col){
		if(x >= 0 && x < SizeX && y >= 0 && y < SizeY){
			int vPos = (x + (SizeX * y));
			vPos += Mathf.FloorToInt(vPos / SizeX);

			colors[vPos] = col;
			colors[vPos + 1] = col;
			colors[vPos + SizeX + 1] = col;
			colors[vPos + SizeX + 2] = col;
		}
	}




	// Mesh Generation
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Generate the playground
	/// </summary>
	public void GenerateArena(){

		//Set the camera
		Camera.main.transform.position = transform.position + new Vector3(SizeX / 2.0f, 0, SizeY / 2.0f) + Camera.main.transform.forward * -(Mathf.Max(SizeX, SizeY) + 10);
		Camera.main.orthographicSize = Mathf.Max(SizeX, SizeY) * 0.75f;
		Camera.main.transform.GetChild(0).localScale = new Vector3(3.6f * Camera.main.orthographicSize, 2.025f * Camera.main.orthographicSize, 1);

		//Clean old and set new mesh
		mesh = new Mesh();
		mesh.name = "Arena";
		mesh.Clear();

		//Set up the arrays
		vertices = new Vector3[(SizeX + 1) * (SizeY + 1)];
		triangles = new int[SizeX * SizeY * 6];
		uvs1 = new Vector2[vertices.Length];
		uvs2 = new Vector2[vertices.Length];
		colors = new Color[vertices.Length];

		//Set the vertices and vertex colors and uvs
		for(int i = 0; i < vertices.Length; i++){
			int x = i % (SizeX + 1);
			int y = Mathf.FloorToInt(i / (SizeX + 1));

			vertices[i] = new Vector3(x, 0, y);
			colors[i] = new Color(0.05f, 0.05f, 0.05f, 1);

			uvs1[i] = new Vector2((float) x / SizeX, (float) y / SizeY); //Main UV over the full mesh
			uvs2[i] = new Vector2(x, y); //Sub uv over only one tile
		}

		//Set the triangles
		for(int i = 0; i < triangles.Length / 6; i++){
			int firstTri = i + Mathf.FloorToInt(i / SizeX); //Position of the first tri of the quad
			triangles[i * 6] = firstTri;
			triangles[i * 6 + 1] = firstTri + (SizeX + 1);
			triangles[i * 6 + 2] = firstTri + 1;
			triangles[i * 6 + 3] = firstTri + 1;
			triangles[i * 6 + 4] = firstTri + (SizeX + 1);
			triangles[i * 6 + 5] = firstTri + (SizeX + 2);
		}

		//Set mesh parts
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs1;
		mesh.uv2 = uvs2;
		mesh.colors = colors;

		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

		//Assign to components
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshRenderer>().material = FloorMaterial;

		generateEdge();

		Debug.Log("Arena generated");
	}


	/// <summary>
	/// Generate a mesh for the edge to let the arena look like a board
	/// </summary>
	private void generateEdge(){
		//Destroy old edge
		if(transform.FindChild("Edge"))DestroyImmediate(transform.FindChild("Edge").gameObject);

		GameObject edgeObj = new GameObject("Edge");
		edgeObj.transform.SetParent(transform);
		edgeObj.AddComponent<MeshFilter>();
		edgeObj.AddComponent<MeshRenderer>();

		//Clean old and set new mesh
		edge = new Mesh();
		edge.name = "ArenaEdge";
		edge.Clear();

		vertices = new Vector3[6];
		vertices[0] = new Vector3(0, 0, SizeY);
		vertices[1] = new Vector3(0, -0.5f, SizeY);
		vertices[2] = new Vector3(0, 0, 0);
		vertices[3] = new Vector3(0, -0.5f, 0);
		vertices[4] = new Vector3(SizeX, 0, 0);
		vertices[5] = new Vector3(SizeX, -0.5f, 0);

		triangles = new int[12];
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;
		triangles[3] = 2;
		triangles[4] = 3;
		triangles[5] = 1;
		triangles[6] = 2;
		triangles[7] = 4;
		triangles[8] = 3;
		triangles[9] = 4;
		triangles[10] = 5;
		triangles[11] = 3;

		edge.vertices = vertices;
		edge.triangles = triangles;

		edge.RecalculateBounds ();
		edge.RecalculateNormals ();

		edgeObj.GetComponent<MeshFilter>().mesh = edge;
		edgeObj.GetComponent<MeshRenderer>().material = new Material(Shader.Find ("Unlit/Color"));
		edgeObj.GetComponent<MeshRenderer>().sharedMaterial.SetColor ("_Color", new Color(0.1f, 0.1f, 0.1f, 1));
	}

}
