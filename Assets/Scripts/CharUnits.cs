using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharUnits 
{
    public string Name;
    public int id;
    public float Health;
    public float Speed;
}
public class Knight : CharUnits
{
    public Knight()
    {
        Name = "Knight";
        id = 1;
        Health = 100;
        Speed = 30;
    }
}
public class MainHero : CharUnits
{
    public MainHero()
    {
        Name = "Hero";
        id = 0;
        Health = 100;
        Speed = 40;
    }
}
public class Connect_Characters
{
    public Knight knight = new Knight();
    public MainHero hero = new MainHero();
}
