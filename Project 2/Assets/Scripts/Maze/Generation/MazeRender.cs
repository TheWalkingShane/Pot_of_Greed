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

    public float coinSpawnProbability = 0.1f; // Starting probability of Coins (10%)
    public float coinProbabilityIncrease = 0.05f; // Increase probability by 5% each cell that doesn't have a coin
   
    public float cardSpawnProbability = 0.1f; // Starting probability for cards
    public float cardProbabilityIncrease = 0.05f; // Increase probability for cards (5%)

    
    // this te pysical size of our maze cells. getting this wrong will result in overlapping
    // or visible gaps between each cell
    public float CellSize = 1f;

    private void Start()
    {
        int AllCoinCount = 0; // to record how many coins spawned
        int AllCardCount = 0; // to record how many cards spawned
        int CardSpawnMax = 7; // set card spawn max to be 7 or less
        
        //get our mazeGenerator script to make us a make
        MazeCell[,] maze = mazeGenerator.GetMaze();
        
        //int cellCount = 0; // Keep track of the number of cells instantiated.

        
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
                
                bool coin = UnityEngine.Random.value < coinSpawnProbability;
                bool card = UnityEngine.Random.value < cardSpawnProbability;

                // If both a coin and a card are set to spawn, choose only one
                if (coin && card)
                {
                    // Randomly choose whether to spawn a coin or a card
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        card = false; // Only spawn the coin, not the card
                    }
                    else
                    {
                        coin = false; // Only spawn the card, not the coin
                    }
                }

                // Update counts and probabilities based on whether objects were spawned
                if (coin)
                {
                    AllCoinCount++;
                    coinSpawnProbability = 0.1f; // Reset the coin probability
                }
                else
                {
                    coinSpawnProbability += coinProbabilityIncrease; // Increase coin probability
                }

                if (AllCardCount != CardSpawnMax) //checks to see if the max number of cards is less than 7
                {
                    if (card)
                    {
                        AllCardCount++;
                        cardSpawnProbability = 0.1f; // Reset the card probability
                    }
                    else
                    {
                        cardSpawnProbability += cardProbabilityIncrease; // Increase card probability
                    }
                }
                //Debug.Log($"Cell {coinProbability}: Coin enabled = {coin}"); // testing the coin probability

                mazeCell.Init(top,bottom,right,left, coin, card);

            }
        }
        
        GameManager.Instance.SetTotalActiveCoins(AllCoinCount);
        GameManager.Instance.SetTotalActiveCards(AllCardCount);
        Debug.Log("coin count total is:" + AllCoinCount);
        Debug.Log("card count total is:" + AllCardCount);
        BakeNavMesh(); //calls it to be baked once finished going through every cell
    }
    
    void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();// Bakes the maze after it's finished,
        Debug.Log("it baked");
    }
}
