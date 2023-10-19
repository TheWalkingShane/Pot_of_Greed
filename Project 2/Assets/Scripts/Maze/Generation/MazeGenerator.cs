using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;
public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]

    public int mazeWidth = 5, mazeHeight = 5; //The Dimensions of the maze.
    public int startX, startY;//The position our algorithm will start from.

    private MazeCell[,] maze;  //An array of maze cells representing the maze grid.
    private Vector2Int currentCell; //The maze cell we are currently looking at.

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++) {
            for (int y = 0; y < mazeHeight; y++) {
                maze[x, y] = new MazeCell(x, y);
            }
        }
         CarvePath(startX, startY);
         return maze;
    }
    
    List<Direction> directions = new List<Direction> { //list of directions that we can go for the maze
        Direction.Up, Direction.Down, Direction.Left, Direction.Right
    };

    List<Direction> getRandomDirections() {
        //makes a copy of our directions list that we can mess around with
        List<Direction> dir = new List<Direction>(directions);

        // makes a directions list to put our randomised directions into
        List<Direction> rndDir = new List<Direction>();

        while (dir.Count > 0) //loop until our rndDir list is empty
        {
            int rnd = Random.Range(0, dir.Count); //get random index in list
            rndDir.Add(dir[rnd]); // add the random diretion to our list
            dir.RemoveAt(rnd); // remove that direction so we can't choose it again
        }

        return rndDir; //when we've got all four directions in a random orde, return the queue.
    }

    bool IsCellValid(int x, int y)
    {
        //if the cell is outside of the map or has already been visited, we consider it not valid.
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false;
        else return true;
    }

    Vector2Int checkNeighbours()
    {
        List<Direction> rndDir = getRandomDirections();

        for (int i = 0; i < rndDir.Count; i++) {
            //set neighbour coordinates to current cell for now.
            Vector2Int neighbour = currentCell;

            //modify neighbour coordinates based on the random directions we've currently trying.
            switch (rndDir[i]) {
                case Direction.Up:
                    neighbour.y++;
                    break; 
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
            }

            //if the neighbour we just tried is valid, we can return that neighbor. if not, we go again.
            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }
        // if we tried all directions and didn't find a valid neighbour. we return the currentCell values.
        return currentCell;
    }

    //takes in two maze positions and sets the cells accordingly 
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        // we can only go in one direction at a time so we can handle this using if else statements
        if (primaryCell.x > secondaryCell.x) { //primary cell's left wall
            
            maze[primaryCell.x, primaryCell.y].leftWall = false;
            
        } else if (primaryCell.x < secondaryCell.x) { //secondary cell's left wall
                     
                     maze[primaryCell.x, primaryCell.y].leftWall = false;
                     
        } else if (primaryCell.y < secondaryCell.y) { // primary cell's top wall
            
            maze[primaryCell.x, primaryCell.y].topWall = false;
            
        } else if (primaryCell.y > secondaryCell.y) { // secondary cell's top wall
            
            maze[primaryCell.x, primaryCell.y].topWall = false;
            
        }
    }

    // starting at the x,y passed in carves a path through the maze until it encounters a dead end
    // a dead end is a call with no valid neighbours
    void CarvePath(int x, int y) {
        //perform a quick check to make sure our start position is within the boundaries of the map,
        // if not set them to a default using 0 and throw a warning
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1) {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0,0");
        }
        
        // set a current cell to the starting position we were passed
        currentCell = new Vector2Int(x, y);
        
        //a list to keep track of our current path
        List<Vector2Int> path = new List<Vector2Int>();
        
        //loop until we encounter a dead end
        bool deadEnd = false;
        while (!deadEnd) {
            
            //get the next cell we are going to try
            Vector2Int nextCell = checkNeighbours();
    
            // if that cell has no valid neighbours, set deadend to true so we break out of the loop
            if (nextCell == currentCell) {
                // if that cell has no valud neighbours, set deadend to true so we break out of the loop
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];                      //set currentcell to the next step along out path
                    path.RemoveAt(i);                           // remove this step from the path
                    nextCell = checkNeighbours();               // check that cell to see if any other neighbour are valid
                    
                    // if we find a valid neighbour break out of the loop
                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell) {
                    deadEnd = true;
                }
            } else {
                
                BreakWalls(currentCell, nextCell);  //set wall flags on these two cells
                maze[currentCell.x, currentCell.y].visited = true;      //set cell to visited before moving on
                currentCell = nextCell;                                 //set the current cell to the valid neighbour we found
                path.Add(currentCell);                                  //add this cell to our path
                
            }
        }
    }
}


public enum Direction 
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    // Return x and y as a vector2int for convenience sake.
    public Vector2Int Position
    {
        get {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell(int x, int y)
    {
        //The Coordiantes of this cell in the maze grid.
        this.x = x;
        this.y = y;
        
        // whether the algorithm has visited this cell or not - false to start
        visited = false;
        
        // all walls are present until the algorithm removes them.
        topWall = leftWall = true;
    }
    
}