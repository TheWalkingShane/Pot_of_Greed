using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    public CardLookUp CLU;
    private List<Card> cards;
    private int index = 0;
    private void Start()
    {
        cards = CLU.cardList;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            index = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 2;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(cards[index].damage);
            Debug.Log(cards[index].health);
        }
    }
}
