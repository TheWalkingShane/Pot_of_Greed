using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// <para>Holds the active inventory of the player through the maze and card game</para>
/// <para>NOTE: Should be initialized from the main menu rather than game scene(s)</para>
/// </summary>
public class CardInventory : MonoBehaviour
{
    public static CardInventory instance = null;
    private static List<Card> _cards;
    public CardLookUp cardSelection;

    public int testCounter;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            testCounter = 0;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        InitInventory();
    }

    // Fills the inventory with the default cards
    void InitInventory()
    {
        _cards = new List<Card>();
        _cards.AddRange(cardSelection.cardList);
    }

    public void DestroyInstance()
    {
        Destroy(this);
    }

    public static List<Card> GetCards()
    {
        return _cards;
    }

    // TODO: will be used to try and add another card to the inventory
    public static bool TryAddCard(Card card)
    {
        // TODO: return whether the card could be successfully added to inventory
        return true;
    }
}