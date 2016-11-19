using UnityEngine;
using System.Collections;

public class EventGenerator : MonoBehaviour {


    int a;
    int b;
    float dx;
    int curCheckUnit;

    MovebleObject[] units;
    bool[] onMission;
    int eventId;

    bool skillMenu;
    bool equipMenu;

    // Use this for initialization
    void Start () {
        skillMenu = false;
        equipMenu = false;
        curCheckUnit = 0;
        units = new MovebleObject[6];
        onMission = new bool[6];
        a = Random.Range(2, 5);
        eventId = Random.Range(0, 2);
        recrut = new MovebleObject(1, true);
        dx = Screen.width / 6;
        int cou = 0;
        for (int i = 0; i < 6; i++)
        {
            if (PlayerPrefs.GetInt("NewGame") == 0)
            {
                //units[i] = new MovebleObject(1, true);
                if (PlayerPrefs.GetInt("PlayerUnitBool" + i.ToString()) == 1)
                {
                    if (PlayerPrefs.GetInt("PlayerUnitOnMission" + cou.ToString()) == 1)  
                    {
                        units[i] = new MovebleObject("PlayerUnitOnBase" + i.ToString());
                    }
                    else
                    {
                        units[i] = new MovebleObject();
                    }
                    cou++;
                }
                else
                {
                    units[i] = new MovebleObject("PlayerUnitOnBase" + i.ToString());
                }
                onMission[i] = false;
            }
            else
            {
                units[i] = new MovebleObject(1, true);
            }
        }
        PlayerPrefs.SetInt("NewGame", 0);

    }

    MovebleObject recrut;
	
	// Update is called once per frame
	void Update () {
	
	}

    public Texture2D okTexture;

    void OnGUI()
    {
        if (eventId == 0)
        { 
            GUI.Label(new Rect(0, 0, Screen.width / 2, Screen.height / 3 * 2 - dx), "Enemy units: " + a.ToString());
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 * 2, Screen.width / 2, Screen.height / 3), "Fight"))
            {
                int counter = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (onMission[i])
                    {
                        counter++;
                    }
                }
                if (counter > 0)
                {
                    PlayerPrefs.SetInt("PlayerTeamCounter", counter);
                  //  Debug.Log(counter);
                    counter = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        units[i].Save("PlayerUnitOnBase" + i.ToString());
                        if (onMission[i])
                        {
                            PlayerPrefs.SetInt("PlayerUnitBool" + i.ToString(), 1);
                            units[i].Save("PlayerUnit" + counter.ToString());
                            counter++;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("PlayerUnitBool" + i.ToString(), 0);
                        }
                    }
                    PlayerPrefs.SetInt("MissionDifficultly", a);
                    Application.LoadLevel("BattleScene");
                }
            }
        }
        if (eventId == 1)
        {
            if (!recrut.isEmpty())
            {
                GUI.Label(new Rect(0, 0, Screen.width / 2, 20), recrut.name);
                GUI.Label(new Rect(0, 20, Screen.width / 2, 20), "Strength " + recrut.strength);
                GUI.Label(new Rect(0, 20 * 2, Screen.width / 2, 20), "Agility " + recrut.agility);
                GUI.Label(new Rect(0, 20 * 3, Screen.width / 2, 20), "Stamina " + recrut.stamina);
            }
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 * 2, Screen.width / 2, Screen.height / 3), "Gain new recrut") && (!recrut.isEmpty()))
            {
                for(int i = 0; i < 6; i++)
                {
                    if (units[i].isEmpty())
                    {
                        units[i] = recrut;
                        recrut = new MovebleObject();
                    }
                }
            }
        }
        for(int i = 0; i < 6; i++)
        {
            if (onMission[i])
            {
                GUI.DrawTexture(new Rect(i * dx, Screen.height / 3 * 2 - dx, dx, dx), okTexture);
            }
            if (GUI.Button(new Rect(i * dx, Screen.height / 3 * 2 - dx, dx, dx), units[i].name))
            {
                curCheckUnit = i;
            }
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 * 2 - dx / 2 * 3, Screen.width / 4, dx / 2), "Equip"))
        {
            equipMenu = !equipMenu;
            skillMenu = false;
        }
        if (GUI.Button(new Rect(Screen.width / 4 * 3, Screen.height / 3 * 2 - dx / 2 * 3, Screen.width / 4, dx / 2), "Skills"))
        {
            skillMenu = !skillMenu;
            equipMenu = false;
        }
        if (GUI.Button(new Rect(Screen.width / 4 * 3, Screen.height / 3 * 2 - 2 * dx, Screen.width / 4, dx / 2), "Delete Unit"))
        {
            units[curCheckUnit] = new MovebleObject();
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 * 2 - 2 * dx, Screen.width / 4, dx / 2), "Squad status"))
        {
            onMission[curCheckUnit] = !onMission[curCheckUnit];
        }
        if (!skillMenu && !equipMenu)
        {
            if (units[curCheckUnit].isEmpty())
            {
                GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, 20), "Empty unit slot");
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, 20), units[curCheckUnit].name);
                GUI.Label(new Rect(Screen.width / 2, 20, Screen.width / 2, 20), "Damage " + units[curCheckUnit].damage.piercingDamage + " " + units[curCheckUnit].damage.slashDamage + " " + units[curCheckUnit].damage.crashDamage + " " + units[curCheckUnit].damage.axeDamage);
                GUI.Label(new Rect(Screen.width / 2, 20 * 2, Screen.width / 2, 20), "Damage bonus " + units[curCheckUnit].bonusDamage);
                GUI.Label(new Rect(Screen.width / 2, 20 * 3, Screen.width / 2, 20), "Attack Range " + units[curCheckUnit].damage.attackRange);
                GUI.Label(new Rect(Screen.width / 2, 20 * 4, Screen.width / 2, 20), "Armour " + units[curCheckUnit].armour.piercingArmour + " " + units[curCheckUnit].armour.slashArmour + " " + units[curCheckUnit].armour.crashArmour + " " + units[curCheckUnit].armour.axeArmour);
                GUI.Label(new Rect(Screen.width / 2, 20 * 5, Screen.width / 2, 20), "Speed " + units[curCheckUnit].speed);
                GUI.Label(new Rect(Screen.width / 2, 20 * 6, Screen.width / 2, 20), "Hits " + units[curCheckUnit].maxHits);
                GUI.Label(new Rect(Screen.width / 2, 20 * 7, Screen.width / 2, 20), "AP " + units[curCheckUnit].maxAP);
            }
            if (GUI.Button(new Rect(0, Screen.height / 3 * 2, Screen.width / 2, Screen.height / 3), "Next Event"))
            {
                a = Random.Range(2, 5);
                eventId = Random.Range(0, 2);
                recrut = new MovebleObject(1, true);
            }
        }
        if (skillMenu)
        {
             
        }
        if (equipMenu)
        {
            if (GUI.Button(new Rect(Screen.width / 2, 0, Screen.width / 4, Screen.height / 6), units[curCheckUnit].damage.name))
            {

            }
            if (GUI.Button(new Rect(Screen.width / 4 * 3, 0, Screen.width / 4, Screen.height / 6), ""))
            {

            }
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 6, Screen.width / 4, Screen.height / 6), units[curCheckUnit].armour.name))
            {

            }
        }
    }
}
