using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public int Health;
    public Stats Stats;
    public Faction Faction;
}

[Serializable]
public class Stats
{
    public int Damage;
    public float AttackRange;
    public int Strength;
    public int Wisdom;
}

public enum Faction
{
    Orc,
    Human,
    Goblin,
    Elf
}
