using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// <para> </para>
/// </summary>
public class CardInventory : MonoBehaviour
{
    public static CardInventory Instance;
    public static List<Card> cards;
    public CardLookUp cardSelection;

    void Awake()
    {
        Instance = this;
        InitInventory();
        DontDestroyOnLoad(this);
    }

    // Fills the inventory with the default cards
    void InitInventory()
    {
        cards.AddRange(cardSelection.cardList);
    }
}