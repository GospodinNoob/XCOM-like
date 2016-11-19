using UnityEngine;
using System.Collections;

public class Unit
{
    public MovebleObject unit;
    public PointXY point;
    public PointXY rotation;

    public Unit(int a, int b, bool tf, PointXY rot)
    {
        this.point = new PointXY(a, b);
        this.rotation = new PointXY(rot);
        this.unit = new MovebleObject(1, tf);
        //this.unit.Generate(1);
    }

    public Unit(int a, int b, string s, bool tf, PointXY rot)
    {
        this.point = new PointXY(a, b);
        this.rotation = new PointXY(rot);
       // Debug.Log(s);
        this.unit = new MovebleObject(s);
        //this.unit.Generate(1);
    }

    public Unit()
    {
        
    }

    public void SetRotation(PointXY p)
    {
        this.rotation = new PointXY(p);
    }

    public Unit(PointXY p, bool tf, PointXY rot)
    {
        this.point = new PointXY(p);
        this.rotation = new PointXY(rot);
        this.unit = new MovebleObject(1, tf);
    }

}
