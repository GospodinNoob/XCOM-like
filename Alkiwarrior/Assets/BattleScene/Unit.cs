using UnityEngine;
using System.Collections;

public class Unit
{
    public MovebleObject unit;
    public int x;
    public int y;
    public int rotation;

    public Unit(int a, int b, bool tf)
    {
        this.x = a;
        this.y = b;
        this.rotation = 0;
        this.unit = new MovebleObject(1, tf);
        //this.unit.Generate(1);
    }
}
