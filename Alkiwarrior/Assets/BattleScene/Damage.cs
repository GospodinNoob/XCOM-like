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

    public void Clear()
    {
        this.damage = 0;
    }
}
