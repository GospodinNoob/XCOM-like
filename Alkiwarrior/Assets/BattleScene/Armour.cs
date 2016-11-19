using UnityEngine;
using System.Collections;

/*
 * 1 - стёганый доспех (1, 2, 1, 1) 1
 * 2 - кожаный доспех (2, 2, 1, 0) 1
 * 3 - полудоспех(2, 4, 2, 3) 3
 * 4 - бригантный доспех (4, 6, 3, 4) 5
 * 5 - полные латы (6, 8, 5, 5), 7
 * 6 - кольчуга (1, 5, 0, 0) 2
 * */

public class Armour{

    public int armour;
    public int id;
    public int piercingArmour;
    public int slashArmour;
    public int crashArmour;
    public int axeArmour;
    public int pound;
    public string name;

    public Armour (int a)
    {
        this.id = a;
        if (a == 1)
        {
            this.name = "Quilted armor";
            this.piercingArmour = 1;
            this.slashArmour = 2;
            this.axeArmour = 1;
            this.crashArmour = 1;
            this.pound = 1;
        }
        Generate(a);
    }

    public Armour()
    {
        this.armour = 0;
    }

    public void Clear()
    {
        this.armour = 0;
    }

    public void Generate(int a)
    {
        if (a == 1)
        {
            this.name = "Quilted armor";
            this.piercingArmour = 1;
            this.slashArmour = 2;
            this.axeArmour = 1;
            this.crashArmour = 1;
            this.pound = 1;
        }
        if (a == 0)
        {
            this.name = "Empty";
            this.piercingArmour = 0;
            this.slashArmour = 0;
            this.axeArmour = 0;
            this.crashArmour = 0;
            this.pound = 0;
        }
    }

    public Armour(string s)
    {
        this.id = PlayerPrefs.GetInt(s + "_Armour_id");
        Generate(this.id);
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
        PlayerPrefs.SetInt(s + "_Armour_id", this.id);
    }
}
