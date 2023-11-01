using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Card
{
    public enum CardType
    {
        BaseKit, 
        Special
    }
    
    //Type is more for visual documentation for programmer
    public CardType type;
    public int health;
    public int damage;
    public Texture cardImage;
}
