using UnityEngine;
using System.Collections;

public class Armour : MonoBehaviour {

    public int armour;

    public Armour (int a)
    {
        this.armour = a;
    }

    public Armour()
    {
        this.armour = 0;
    }

    public void Clear()
    {
        this.armour = 0;
    }
}
