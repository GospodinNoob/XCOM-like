using UnityEngine;
using System.Collections;

public class EventGenerator : MonoBehaviour {


    int a;
    int b;
    float dx;

    MovebleObject[] units;
    bool[] onMission;

    // Use this for initialization
    void Start () {
        units = new MovebleObject[6];
        onMission = new bool[6];
        a = Random.Range(2, 5);
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
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height / 3 * 2 - dx), "Enemy units: " + a.ToString());
        for(int i = 0; i < 6; i++)
        {
            if (GUI.Button(new Rect(i * dx, Screen.height / 3 * 2 - dx, dx, dx), units[i].name))
            {
                onMission[i] = !onMission[i];
            }
        }
        if (GUI.Button(new Rect(0, Screen.height / 3 * 2, Screen.width / 2, Screen.height / 3), "Next Event"))
        {
            a = Random.Range(2, 5);
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 * 2, Screen.width / 2, Screen.height / 3), "Fight"))
        {
            int counter = 0;
            for(int i = 0; i < 6; i++)
            {
                if (onMission[i])
                {
                    counter++;
                }
            }
            if (counter > 0)
            {
                PlayerPrefs.SetInt("PlayerTeamCounter", counter);
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
}
