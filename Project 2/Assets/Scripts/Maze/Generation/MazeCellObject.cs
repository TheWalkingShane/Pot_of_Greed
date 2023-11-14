using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{


    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject middleCoin;
    [SerializeField] private GameObject centerCard;

    public void Init(bool top, bool bottom, bool right, bool left, bool coin, bool card)
    {
        topWall.SetActive(top);
        bottomWall.SetActive(bottom);
        rightWall.SetActive(right);
        leftWall.SetActive(left);
        middleCoin.SetActive(coin);
        centerCard.SetActive(card);
    }
}