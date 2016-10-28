using UnityEngine;
using System.Collections;

public class Damage{

    public int damage;

    public Damage (int a)
    {
        this.damage = a;
    }

    public Damage ()
    {
        this.damage = 0;
    }

    public Damage (Damage dmg, Armour arm)
    {
        this.damage = dmg.damage - arm.armour;
    }

    public int Sum()
    {
        return this.damage;
    }

    public void Clear()
    {
        this.damage = 0;
    }
}
