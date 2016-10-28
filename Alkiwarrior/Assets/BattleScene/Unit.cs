using UnityEngine;
using System.Collections;

public class Unit
{
    public MovebleObject unit;
    public PointXY point;
    public int rotation;

    public Unit(int a, int b, bool tf)
    {
        this.point = new PointXY(a, b);
        this.rotation = 0;
        this.unit = new MovebleObject(1, tf);
        //this.unit.Generate(1);
    }

    public Unit()
    {
        
    }

    public Unit(PointXY p, bool tf)
    {
        this.point = new PointXY(p);
    }
}
