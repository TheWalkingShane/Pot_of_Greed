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


    // this te pysical size of our maze cells. getting this wrong will result in overlapping
    // or visible gaps between each cell
    public float CellSize = 1f;

    private void Start()
    {
        //get our mazeGenerator script to make us a make
        MazeCell[,] maze = mazeGenerator.GetMaze();
        
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

                bool coin = true;

                if (x == mazeGenerator.mazeWidth -1) {
                    right = true;
                }
                if (y == 0) {
                    bottom = true;
                }
                
                mazeCell.Init(top,bottom,right,left, coin);
            }
        }
        
        
        BakeNavMesh(); //calls it to be baked once finished going through every cell
    }
    
    void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();// Bakes the maze after it's finished,
        Debug.Log("it baked");
    }
}
