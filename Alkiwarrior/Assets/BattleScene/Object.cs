using UnityEngine;
using System.Collections;

public class Object{

    public int id;
    public Armour armour;
    public int curHits;
    public int maxHits;
    public string name;

    public Object (int a)
    {
        if (a != 0)
        {
            Generate(a);
}
        else
        {
            this.id = 0;
            this.armour = new Armour();
        }
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

    public void Generate(int a)
    {
        this.id = a;
        this.armour = new Armour(0);
        this.maxHits = 10;
        this.curHits = this.maxHits;
        this.name = "Box";
    }

    public Object()
    {
        this.id = 0;
        this.armour = new Armour();
    }

    public bool isEmpty()
    {
        return id == 0;
    }

    public void Clear()
    {
        this.id = 0;
    }
}
