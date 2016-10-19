using UnityEngine;
using System.Collections;

public class Chunk{
    public Object obj;
    public MovebleObject movebleObject;
    public string landCover;

    public Chunk()
    {
        this.obj = new Object();
        this.movebleObject = new MovebleObject();
        landCover = "Grass";
    }

    public Chunk(int a)
    {
        this.obj = new Object(a);
        this.movebleObject = new MovebleObject();
        landCover = "Grass";
    }

    public Chunk(int a, int b, bool tf)
    {
        this.obj = new Object(a);
        this.movebleObject = new MovebleObject(b, tf);
        landCover = "Grass";
    }

    public void GenerateObject(int a)
    {
        this.obj.Generate(a);
    }

    public void GenerateMovebleObject(int a)
    {
        this.movebleObject.Generate(a);
    }

    public void AddMovebleObject(MovebleObject move)
    {
        this.movebleObject = move;
    }
}
