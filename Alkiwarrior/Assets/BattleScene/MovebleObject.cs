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
        this.name = "H" + Random.Range(1, 100).ToString();
    }

    public void Save(string s)
    {
        PlayerPrefs.SetInt(s + "_maxHits", this.maxHits);
        PlayerPrefs.SetInt(s + "_id", this.id);
        PlayerPrefs.SetInt(s + "_maxAP", this.maxAP);
        PlayerPrefs.SetString(s + "_name", this.name);
        PlayerPrefs.SetFloat(s + "_speed", this.speed);
        this.damage.Save(s);
        this.armour.Save(s);
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

    public MovebleObject(string s)
    {
        this.id = PlayerPrefs.GetInt(s + "_id");
        this.maxAP = PlayerPrefs.GetInt(s + "_maxAP");
        this.maxHits = PlayerPrefs.GetInt(s + "_maxHits");
        this.curAP = this.maxAP;
        this.curHits = this.maxHits;
        this.player = true;
        this.damage = new Damage(s);
        this.armour = new Armour(s);
        this.speed = PlayerPrefs.GetFloat(s + "_speed");
        this.name = PlayerPrefs.GetString(s + "_name");
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
