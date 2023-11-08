using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MazeRender : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;
    [SerializeField] NavMeshSurface navMeshSurface; // Bakes the Floor of the maze.

    public float coinProbability = 0.1f; // Starting probability of 10%
    public float probabilityIncrease = 0.05f; // Increase probability by 5% each cell that doesn't have a coin
    // this te pysical size of our maze cells. getting this wrong will result in overlapping
    // or visible gaps between each cell
    public float CellSize = 1f;

    private void Start()
    {
        int AllCoinCount = 0;
        //get our mazeGenerator script to make us a make
        MazeCell[,] maze = mazeGenerator.GetMaze();
        
        int cellCount = 0; // Keep track of the number of cells instantiated.

        
        //loop through every cell in the maze
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                // instantiate a new maze cell prefab as a child of the MazeRenderer object
                GameObject newCell = Instantiate(MazeCellPrefab,
                    new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                // get a reference to the cell's mazecellprefab script
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                
                //determine which walls need to be active
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                
                //bottom and right walls are deactivated by default unless we are at the bottom or right
                // edge of the maze
                bool right = false;
                bool bottom = false;

                

                if (x == mazeGenerator.mazeWidth -1) {
                    right = true;
                }
                if (y == 0) {
                    bottom = true;
                }
                
                bool coin = UnityEngine.Random.value < coinProbability;
                
                if (coin)
                {
                    AllCoinCount++;
                    // If a coin is spawned, reset the probability
                    coinProbability = 0.1f;
                    
                }
                else
                {
                    // If a coin is not spawned, increase the probability for the next cell
                    coinProbability += probabilityIncrease;
                }
                //Debug.Log($"Cell {coinProbability}: Coin enabled = {coin}"); // testing the coin probability

                mazeCell.Init(top,bottom,right,left, coin);

            }
        }
        
        GameManager.Instance.SetTotalActiveCoins(AllCoinCount);
        Debug.Log("coin count total is:" + AllCoinCount);
        BakeNavMesh(); //calls it to be baked once finished going through every cell
    }
    
    void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();// Bakes the maze after it's finished,
        Debug.Log("it baked");
    }
}
