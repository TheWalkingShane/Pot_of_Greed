using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public int health;
    public int damage;
    public void setCard(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }
    
}
