using UnityEngine;
using System.Collections;

/*
 * id
 * 0 - empty
 * 1 - human
 */

public class MovebleObject{

    public int id;
    public Armour armour;
    public Damage damage;
    public int curHits;
    public int maxHits;
    public int speed;
    public int maxAP;
    public int curAP;
    public bool player;
    public string name;

    public void Generate(int a)
    {
        this.id = a;
        int balance = Random.Range(4, 7);
        this.armour = new Armour(Random.Range(1, 4));
        this.damage = new Damage(Random.Range(0, 2) + balance - this.armour.armour - 1);
        this.maxHits = Random.Range(3, 6);
        this.speed = Random.Range(1, 3);
        this.maxAP = Random.Range(2, 4);
        this.curAP = this.maxAP;
        this.curHits = this.maxHits;
        this.name = "Human";
    }

    public MovebleObject (int a, bool pl)
    {
        this.player = pl;
        this.Generate(a);
    }

    public MovebleObject()
    {
        this.player = false;
        this.id = 0;
        this.armour = new Armour(0);
        this.damage = new Damage(0);
    }

    public void Clear()
    {
        this.id = 0;
    }

    public bool isEmpty()
    {
        return id == 0;
    }

    public void calculateNewTurn()
    {
        this.curAP = this.maxAP;
    }
}
