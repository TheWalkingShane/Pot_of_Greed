using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public int health;
    public int damage;
    private TextMeshProUGUI textH;
    private TextMeshProUGUI textD;

    public void Start()
    {
        textH = this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        textD = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textH.text = $"HP: {this.health}";
        textD.text = $"ATK: {this.damage}";
    }

    public void setCard(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }

    public void displayInfo()
    {
        textH.text = $"HP: {this.health}";
        textD.text = $"ATK: {this.damage}";
    }

    public void dealDamage(int damage)
    {
        health -= damage;
        textH.text = $"HP: {this.health}";
    }
}
