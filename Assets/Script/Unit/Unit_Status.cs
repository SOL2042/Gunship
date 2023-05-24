using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitSidetype
{
    Default = 0, team1, team2
}
public enum PlayerMode
{
    Default = 0, Hover = 1
}
public class Unit_Status
{
    public float maxHp;
    public float currentHp;
    public float def;

    public float unitSpeed;
    public float unitshootRange;

    public float misileAtk;
    public float rocketAtk;
    public float bulletAtk;

    public float bulletRPM;
    public float rocketRPM;
    public float misileRPM;

    public bool isSelected;
    public UnitSidetype unitSideType;
    public PlayerMode playerMode;
    public string code = "";
    public string unitName = "";
    public string unitDescripts = "";



    public static Unit_Status operator +(Unit_Status left, Unit_Status right)
    {
        left.maxHp += right.maxHp;
        left.currentHp += right.currentHp;
        left.def += right.def;
        left.unitSpeed += right.unitSpeed;
        left.unitshootRange += right.unitshootRange;
        
        left.misileAtk += right.misileAtk;
        left.rocketAtk += right.rocketAtk;
        left.bulletAtk += right.bulletAtk;
        left.bulletRPM += right.bulletRPM;
        left.rocketRPM += right.rocketRPM;
        left.misileRPM += right.misileRPM;
        return left;
    }

    public static Unit_Status operator -(Unit_Status left, Unit_Status right)
    {
        left.maxHp -= right.maxHp;
        left.currentHp -= right.currentHp;
        left.def -= right.def;
        left.unitSpeed -= right.unitSpeed;
        left.unitshootRange -= right.unitshootRange;

        left.misileAtk -= right.misileAtk;
        left.rocketAtk -= right.rocketAtk;
        left.bulletAtk -= right.bulletAtk;
        left.bulletRPM -= right.bulletRPM;
        left.rocketRPM -= right.rocketRPM;
        left.misileRPM -= right.misileRPM;
        return left;
    }
}
