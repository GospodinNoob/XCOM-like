using UnityEngine;
using System.Collections;

public class SpriteActive : MonoBehaviour {

    public GameObject grass;
    public GameObject box;
    public GameObject top, bot, left, right;

	// Use this for initialization
	void Start () {
	
	}

    public void setIdActive(int id, bool tf)
    {
        if (id == -1)
        {
            top.active = tf;
        }
        if (id == -2)
        {
            bot.active = tf;
        }
        if (id == -3)
        {
            left.active = tf;
        }
        if (id == -4)
        {
            right.active = tf;
        }

        if (id == 0)
        {
            grass.active = tf;
        }
        if (id == 1)
        {
            box.active = tf;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
