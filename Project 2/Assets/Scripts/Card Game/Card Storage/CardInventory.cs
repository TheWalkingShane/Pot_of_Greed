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
    public static CardInventory Instance;
    private static List<Card> _cards;
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
        _cards.AddRange(cardSelection.cardList);
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