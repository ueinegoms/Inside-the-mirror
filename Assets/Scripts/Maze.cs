using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
	[System.Serializable]
	public class Cell{
		public bool visited;
		public GameObject north;//1
		public GameObject east;//2
		public GameObject west;//3
		public GameObject south;//4
	}

	public GameObject wall;
	public GameObject wall2;


	public float wallLenght;
	public int xSize = 5;
	public int ySize = 5;
	private Vector3 initialPos;
	private GameObject wallHolder;
	public Cell[] cells;
	public int currentCell = 0;
	private int totalCells;
	private int visitedCells = 0;
	private bool startedBuilding = false;
	private int currentNeighbour = 0;
	private List<int> lastCells;
	private int backingUp = 0;
	private int wallToBreak = 0;

	// Use this for initialization
	void Start () {
		wallLenght = wall.transform.localScale.z;
		CreateWalls ();
	}

	void CreateWalls(){
		lastCells = new List<int> ();
		lastCells.Clear();
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";

		initialPos = new Vector3 ((-xSize / 2) + wallLenght / 2, 0, (-ySize / 2) + wallLenght / 2);
		Vector3 myPos = initialPos;
		GameObject tempWall;

		//for x Axis
		for(int i = 0; i < ySize; i++){
			for(int j = 0; j <= xSize; j++){
				myPos = new Vector3 (initialPos.x + (j * wallLenght) - wallLenght / 2, wall.transform.localScale.y/2, initialPos.z + (i * wallLenght) - wallLenght / 2);

				//RANDOM SELECTING A WALL
				int paredeEscolhida = Random.Range(0,2); //random de 0 a 1
				if (paredeEscolhida == 0){
					tempWall = Instantiate (wall, myPos, Quaternion.identity) as GameObject;
					tempWall.transform.parent = wallHolder.transform;
				}
				if (paredeEscolhida == 1){
					tempWall = Instantiate (wall2, myPos, Quaternion.identity) as GameObject;
					tempWall.transform.parent = wallHolder.transform;
				}
			}
		}

		//for y Axis
		for(int i = 0; i <= ySize; i++){
			for(int j = 0; j < xSize; j++){
				myPos = new Vector3 (initialPos.x + (j * wallLenght), wall.transform.localScale.y/2, initialPos.z + (i * wallLenght) - wallLenght);
				tempWall = Instantiate (wall, myPos, Quaternion.Euler(0, 90, 0)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}

		CreateCells ();
	}

	void CreateCells(){
		totalCells = xSize * ySize;
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		cells = new Cell[xSize*ySize];
		int eastWestProcess = 0;
		int childProcess = 0;
		int termCount = 0;


		//gets all the children
		for (int i = 0; i < children; i++){
			allWalls [i] = wallHolder.transform.GetChild (i).gameObject;
		}

		//assigns walls to the cells
		for (int cellprocess = 0; cellprocess < cells.Length; cellprocess ++){
			
			if (termCount == xSize) {
				eastWestProcess ++;
				termCount = 0;
			}

			cells [cellprocess] = new Cell ();
			cells [cellprocess].east = allWalls [eastWestProcess];
			cells [cellprocess].south = allWalls [childProcess + (xSize + 1) * ySize];
				eastWestProcess++;

			termCount++;
			childProcess++;
			cells [cellprocess].west = allWalls [eastWestProcess];
			cells [cellprocess].north = allWalls [(childProcess + (xSize + 1) * ySize) + xSize - 1];
		}

		CreateMaze ();
	}

	void CreateMaze(){
		while (visitedCells < totalCells) {
			if (startedBuilding) {
				GiveMeNeighbour ();
				if (cells [currentNeighbour].visited == false && cells [currentCell].visited == true) {
					BreakWall ();
					cells [currentNeighbour].visited = true;
					visitedCells++;
					lastCells.Add (currentCell);
					currentCell = currentNeighbour;
					if (lastCells.Count > 0) {
						backingUp = lastCells.Count - 1;
					}
				}
			} else {
				currentCell = Random.Range (0, totalCells);
				cells [currentCell].visited = true;
				visitedCells++;
				startedBuilding = true;
			}

			//Invoke ("CreateMaze", 00f);

		}
		Debug.Log("Finished");
		//GiveMeNeighbour ();
	}

	void BreakWall(){
		switch (wallToBreak) {
		case 1:
			Destroy (cells [currentCell].north);
			break;
		case 2:
			Destroy (cells [currentCell].east);
			break;
		case 3:
			Destroy (cells [currentCell].west);
			break;
		case 4:
			Destroy (cells [currentCell].south);
			break;
		}
	}

	void GiveMeNeighbour(){
		
		int lenght = 0;
		int[] neighbours = new int[4];
		int[] connectingWall = new int[4];
		int check = 0;
		check = ((currentCell + 1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;
		//west
		if (currentCell + 1 < totalCells && (currentCell+1) != check){
			if (cells [currentCell + 1].visited == false) {
				neighbours [lenght] = currentCell + 1;
				connectingWall [lenght] = 3;
				lenght++;
			}
		}

		//east
		if (currentCell - 1 >= 0 && currentCell != check){
			if (cells [currentCell - 1].visited == false) {
				neighbours [lenght] = currentCell - 1;
				connectingWall [lenght] = 2;
				lenght++;
			}
		}

		//north
		if (currentCell + xSize < totalCells){
			if (cells [currentCell + xSize].visited == false) {
				neighbours [lenght] = currentCell + xSize;
				connectingWall [lenght] = 1;
				lenght++;
			}
		}

		//south
		if (currentCell - xSize >= 0){
			if (cells [currentCell - xSize].visited == false) {
				neighbours [lenght] = currentCell - xSize;
				connectingWall [lenght] = 4;
				lenght++;
			}
		}

		if (lenght != 0) {
			int theChosenOne = Random.Range (0, lenght);
			currentNeighbour = neighbours [theChosenOne];
			wallToBreak = connectingWall [theChosenOne];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells [backingUp];
				backingUp--;
			}
		}

		/*
		 * debugging
		for (int i = 0; i < lenght; i++) {
			Debug.Log (neighbours[i]);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
