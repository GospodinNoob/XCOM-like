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
    public float speed;
    public int maxAP;
    public int curAP;
    public bool player;
    public string name;

    public void Generate(int a)
    {
        this.id = a;
        int balance = Random.Range(4, 7);
        this.armour = new Armour(Random.Range(1, 4));
        this.damage = new Damage(Random.Range(1, 2) + balance - this.armour.armour - 1);
        this.maxHits = Random.Range(3, 6);
        this.maxAP = Random.Range(2, 4);
        if (this.maxAP == 3)
        {
            this.speed = Random.Range((float)1.5, (float)2.2);
        }
        else
        {
            this.speed = Random.Range((float)2.2, (float)2.8);
        }
       // Debug.Log(this.speed);
        this.curAP = this.maxAP;
        this.curHits = this.maxHits;
        this.name = "Human";
    }

    public void DealDamage(Damage dmg)
    {
        Damage reducedDamage = new Damage(dmg, this.armour);
        this.curHits -= Mathf.Max(0, reducedDamage.Sum());
        if (this.curHits <= 0)
        {
            this.id = 0;
        }
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

    public void doAction(int a)
    {
        this.curAP -= a;
        this.curAP = Mathf.Max(0, this.curAP);
    }
}
