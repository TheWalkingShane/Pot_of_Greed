using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: convert this to a native C# class (passing by reference = much easier)
[Serializable]
public struct Card
{
    public enum CardType
    {
        BaseKit, 
        Special
    }

    public CardType type;
    public int health;
    public int damage;
    public Texture cardImage;
}
