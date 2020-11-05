using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISpells
{
    string name { get; }
    float[] damage { get; }
    string[] spellForUnit { get; }
    float[] radius { get; }
    float[] speed { get; }

}
public class Wind : ISpells
{
    Connect_Characters connect = new Connect_Characters();
    public enum SpellForUnit { Hero, Knight };
    public string[] spellForUnit { get { return Enum.GetNames(typeof(SpellForUnit)); } }
    protected string Name = "Wind";
    protected float[] Damage = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    protected float[] Radius = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    protected float[] Speed = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    public string name { get { return Name; } }
    public float[] damage
    {
        get
        {
            if (spellForUnit.Contains(connect.hero.Name)) Damage[connect.hero.id] = 60;
            return Damage;
        }
    }
    public float[] radius
    {
        get
        {
            if (spellForUnit.Contains(connect.hero.Name)) Radius[connect.hero.id] = 50;
            return Radius;
        }
    }
    public float[] speed
    {
        get
        {
            if (spellForUnit.Contains(connect.hero.Name)) Speed[connect.hero.id] = 1.25f * Time.fixedDeltaTime;
            return Speed;
        }
    }
}
public class Strong_Punch : ISpells
{
    Connect_Characters connect = new Connect_Characters();
    public enum SpellForUnit { Hero, Knight };
    public string[] spellForUnit { get { return Enum.GetNames(typeof(SpellForUnit)); } }
    protected string Name = "Strong Punch";
    protected float[] Damage = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    protected float[] Radius = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    protected float[] Speed = new float[Enum.GetNames(typeof(SpellForUnit)).Length];
    public string name { get { return Name; } }
    public float[] damage
    {
        get
        {
            if (spellForUnit.Contains(connect.knight.Name)) Damage[connect.knight.id] = 35;
            return Damage;
        }
    }
    public float[] radius
    {
        get
        {
            if (spellForUnit.Contains(connect.knight.Name)) Radius[connect.knight.id] = 12.5f;
            return Radius;
        }
    }
    public float[] speed
    {
        get
        {
            if (spellForUnit.Contains(connect.hero.Name)) Speed[connect.knight.id] = 3;
            return Speed;
        }
    }

}
public class Spells_Connect
{
    public Wind wind = new Wind();
    public Strong_Punch punch = new Strong_Punch();
}
