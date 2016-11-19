using UnityEngine;
using System.Collections;

/*
 * 1 - фальшион (0, 2, 2, 4) 2, 1
 * 2 - меч (0, 3, 2, 1) 2, 2
 * 3 - короткий меч (3, 3, 0, 0) 1, 1
 * 4 - Гаста (4, 1, 0, 0) 2, 2
 * */

public class Damage{

    public int id;
    public int damage;
    public int piercingDamage;
    public int slashDamage;
    public int crashDamage;
    public int axeDamage;
    public int pound;
    public int attackRange;
    public string name;

    public void Generate(int a)
    {
        if (a == 0)
        {
            this.name = "Empty";
            this.piercingDamage = 0;
            this.slashDamage = 0;
            this.crashDamage = 0;
            this.axeDamage = 0;
            this.pound = 0;
            this.attackRange = 0;
        }
        if (a == 1)
        {
            this.name = "Falchion";
            this.piercingDamage = 0;
            this.slashDamage = 2;
            this.crashDamage = 2;
            this.axeDamage = 4;
            this.pound = 2;
            this.attackRange = 1;
        }
    }

    public Damage (int a)
    {
        this.id = a;
        Generate(a);
    }

    public Damage ()
    {
        this.damage = 0;
    }

    public Damage(string s)
    {
        this.id = PlayerPrefs.GetInt(s + "_Damage_id");
        Generate(this.id);
    }

    public Damage(Damage dmg, Armour arm)
    {
        this.damage = Mathf.Max(0, dmg.piercingDamage - arm.piercingArmour) + Mathf.Max(0, dmg.slashDamage - arm.slashArmour) +
            Mathf.Max(0, dmg.axeDamage - arm.axeArmour) + Mathf.Max(0, dmg.crashDamage - arm.crashArmour);
    }

    public Damage (Damage dmg, PointXY vectorTarget, PointXY vectorAttack, Armour arm)
    {
        this.damage = Mathf.Max(0, dmg.piercingDamage - arm.piercingArmour) + Mathf.Max(0, dmg.slashDamage - arm.slashArmour) +
            Mathf.Max(0, dmg.axeDamage - arm.axeArmour) + Mathf.Max(0, dmg.crashDamage - arm.crashArmour);
        double a1 = Mathf.Atan2(vectorAttack.x, vectorAttack.y);
        if (a1 < 0)
        {
            a1 += Mathf.PI * 2;
        }
        double a2 = Mathf.Atan2(vectorTarget.x, vectorTarget.y);
        if (a2 < 0)
        {
            a2 += Mathf.PI * 2;
        }
        double resultAngle = (a2 - a1);
        if (resultAngle < 0)
        {
            resultAngle += Mathf.PI * 2;
        }
        if (resultAngle > Mathf.PI)
        {
            resultAngle = Mathf.PI * 2 - resultAngle;
        }
        resultAngle = Mathf.PI - resultAngle;
        resultAngle /= Mathf.PI;
        this.damage = this.damage + (int)(this.damage * resultAngle);
    }

    public int Sum()
    {
        return this.damage;
    }

    public void Clear()
    {
        this.damage = 0;
    }

    public int GetPound(int a)
    {
        if (a == 1)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    public void Save(string s)
    {
        PlayerPrefs.SetInt(s + "_Damage_id", this.id);
    }
}
