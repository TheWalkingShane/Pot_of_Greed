using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// <para>Used as storage for the balanced, available cards</para>
/// <para>Values should be initialized in Unity editor for either a scene object or prefab</para>
/// </summary>
public class CardLookUp : MonoBehaviour
{
    // TODO: cardList rename to defaultCardList
    [Header("Default card data")] public int cardCapacity = 5;
    public List<Card> cardList;

    [Header("Special card data")] public int specialCardCapacity = 10;
    public List<Card> specialCardList;

    void Start()
    {
        // TODO: init all of the cards
    }
}