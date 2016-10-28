using UnityEngine;
using System.Collections;

public class PointXY
{

    public int x;
    public int y;

    public PointXY(int a, int b)
    {
        this.x = a;
        this.y = b;
    }

    public PointXY(PointXY p)
    {
        this.x = p.x;
        this.y = p.y;
    }
}
