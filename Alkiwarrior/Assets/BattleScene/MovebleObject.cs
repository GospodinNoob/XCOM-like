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
    public int strength;
    public int agility;
    public int stamina;
    public int bonusDamage;

    public int sumPoundEquip;

    public void SetWeapon(int a)
    {
        this.damage.Generate(a);
    }

    public void SetArmour(int a)
    {
        this.armour.Generate(a);
    }

    public int sumPound;

    public void ReсalcSecondaryParam()
    {
        this.speed = 2;
        this.sumPoundEquip = this.damage.pound + this.armour.pound;
        this.sumPound = this.stamina + (int) (this.stamina / 5);
        int pou = this.sumPound - this.sumPoundEquip;
        this.maxAP = 2;
        if (pou > 0)
        {
            this.speed += 0.2f * pou;
            this.maxAP += pou / 5;
        }
        else
        {
            this.speed -= 0.4f * pou;
        }
        this.speed = Mathf.Max(0.7f, this.speed);
        this.bonusDamage = strength / 5;
        this.maxHits = this.stamina;
    }

    public void ReсalcSecondaryParam(int a, int b)
    {
        this.speed = 2;
        this.sumPoundEquip = this.damage.GetPound(a) + this.armour.GetPound(b);
        this.sumPound = this.stamina + (int)(this.stamina / 5);
        int pou = this.sumPound - this.sumPoundEquip;
        this.maxAP = 2;
        if (pou > 0)
        {
            this.speed += 0.2f * pou;
            this.maxAP += pou / 5;
        }
        else
        {
            this.speed -= 0.4f * pou;
        }
        this.speed = Mathf.Max(0.7f, this.speed);
        this.bonusDamage = strength / 5;
        this.maxHits = this.stamina;
    }

    public void Generate(int a)
    {
        this.id = a;
        this.stamina = 5;
        this.strength = 5;
        this.agility = 5;
        this.armour = new Armour(1);
        this.damage = new Damage(1);
        ReсalcSecondaryParam();
        this.curAP = this.maxAP;
        this.curHits = this.maxHits;
        this.name = "H" + Random.Range(1, 100).ToString();
    }

    public void Save(string s)
    {
        PlayerPrefs.SetInt(s + "_strength", this.strength);
        PlayerPrefs.SetInt(s + "_id", this.id);
        PlayerPrefs.SetInt(s + "_agility", this.agility);
        PlayerPrefs.SetString(s + "_name", this.name);
        PlayerPrefs.SetInt(s + "_stamina", this.stamina);
        //Debug.Log(this.stamina);
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

    public void DealDamage(Damage dmg, PointXY vectorTarget, PointXY vectorAttack)
    {
        Damage reducedDamage = new Damage(dmg, vectorTarget, vectorAttack, this.armour);
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
        this.strength = PlayerPrefs.GetInt(s + "_strength");
        this.agility = PlayerPrefs.GetInt(s + "_agility");
        this.stamina = PlayerPrefs.GetInt(s + "_stamina");
        //Debug.Log(this.stamina);
        this.player = true;
        this.damage = new Damage(s);
        this.armour = new Armour(s);
        ReсalcSecondaryParam();
        this.curAP = this.maxAP;
        this.curHits = this.maxHits;
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
