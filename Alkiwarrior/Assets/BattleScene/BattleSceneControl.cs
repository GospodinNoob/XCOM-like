using UnityEngine;
using System.Collections;

public class BattleSceneControl : MonoBehaviour {


    public GameObject humanGO;
    GameObject[][] chunks;
    public GameObject chunkPrefab;
    int n, k;
    float deltaX;
    int magicConstant = 53;

    GameObject[] playerUnitsGO;
    Unit[] playerUnits;

    // Use this for initialization
    void Start () {
        n = 6;
        k = 8;
        dx = Screen.width / n;
        dy = Screen.height / k;
        if (dx < dy)
        {
            dy = dx;
        }
        if (dy < dx)
        {
            dx = dy;
        }
       // Debug.Log(dx);
        deltaX = Screen.width - 6 * dx;
        deltaX /= 2;

        map = new Chunk[6][];
        chunks = new GameObject[6][];
        for (int i = 0; i < 6; i++)
        {
            map[i] = new Chunk[6];
            chunks[i] = new GameObject[6];
            for(int j = 0; j < 6; j++)
            {

                map[i][j] = new Chunk();
                Transform tr = this.gameObject.transform;
                chunks[i][j] = (GameObject) GameObject.Instantiate(chunkPrefab, tr);
                chunks[i][j].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * i, tr.position.y - dx / 2 - dx + dx * j, tr.position.z);
                float scale = chunks[i][j].transform.localScale.x * Screen.width / 6 / 2 * (float)0.74;
                chunks[i][j].transform.localScale = new Vector3(scale, scale, scale);
                chunks[i][j].GetComponent<SpriteActive>().setIdActive(1, false);
            }
        }

        for(int i = 0; i < 5; i++)
        {
            int a = Random.Range(0, 6);
            int b = Random.Range(0, 6);
            map[a][b].obj.Generate(1);
            chunks[a][b].GetComponent<SpriteActive>().setIdActive(1, true);
        }

        playerUnits = new Unit[3];
        playerUnitsGO = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(0, 6);
            int b = Random.Range(0, 6);
            while (!map[a][b].obj.isEmpty() || !map[a][b].movebleObject.isEmpty())
            {
                a = Random.Range(0, 6);
                b = Random.Range(0, 6);
            }
            playerUnits[i] = new Unit(a, b, true);
            Transform tr = this.gameObject.transform;
            playerUnitsGO[i] = (GameObject) GameObject.Instantiate(humanGO, tr);
            playerUnitsGO[i].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * a, tr.position.y - dx / 2 - dx + dx * b, tr.position.z);
            float scale = playerUnitsGO[i].transform.localScale.x * Screen.width / 6 / 2 * (float)0.74;
            playerUnitsGO[i].transform.localScale = new Vector3(scale, scale, scale);
            map[a][b].movebleObject = playerUnits[i].unit;
        }
        chosenChunk = map[0][0];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    Unit GetUnit(int a, int b)
    {
        Unit un = playerUnits[0];
        for(int i = 0; i < 3; i++)
        {
            if ((playerUnits[i].x == a) && (playerUnits[i].y == b))
            {
                un = playerUnits[i];
                break;
            }
        }
        return un;
    }

    void Swap(int a, int b, int x, int y)
    {
        Unit playerUnit = GetUnit(a, b);
        playerUnit.x = x;
        playerUnit.y = y;
        map[a][b].movebleObject = map[x][y].movebleObject;
        map[x][y].movebleObject = playerUnit.unit;

        for(int i = 0; i < 3; i++)
        {
            Transform tr = this.gameObject.transform;
          //  Debug.Log(playerUnits[i].x);
            playerUnitsGO[i].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * playerUnits[i].x, tr.position.y - dx / 2 - dx + dx * playerUnits[i].y, tr.position.z);
        }
    }

    Chunk[][] map;

    Chunk chosenChunk;
    int chx;
    int chy;
    bool move = false;

    float dx;
    float dy;

    public GUIStyle coverButtonStyle;

    void OnGUI()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                    if (GUI.Button(new Rect(i * dx + deltaX, j * dy, dx, dy), "", coverButtonStyle))
                    {
                        if (!move)
                        {
                            chosenChunk = map[i][5 - j];
                            chx = i;
                            chy = 5 - j;
                        }
                        else
                        {
                            if (map[i][5 - j].obj.isEmpty() && map[i][5 - j].movebleObject.isEmpty() && (GetUnit(i, 5 - j).unit.curAP > 0))
                            {
                                Swap(chx, chy, i, 5 - j);
                                chx = i;
                                chy = 5 - j;
                            }
                            move = false;
                        }
                    }
                }
        }
        if (chosenChunk != null)
        {
            if (chosenChunk.movebleObject.id != 0)
            {
                if (GUI.Button(new Rect(Screen.width / 3, dy * 6, Screen.width / 6, dy), "Move"))
                {
                    move = !move;
                }
                if (GUI.Button(new Rect(Screen.width / 3, dy * 7, Screen.width / 6, dy), "Attack"))
                {

                }
            }
            GUI.Label(new Rect(0, dy * 6, Screen.width / 3, dy * 2), chosenChunk.landCover);
            if (chosenChunk.obj.id != 0)
            {
                GUI.Label(new Rect(0, dy * 6 + dx * 2 / 3, Screen.width / 3, dy * 2), chosenChunk.obj.name + " " + chosenChunk.obj.curHits + "/" + chosenChunk.obj.maxHits);
            }
            if (chosenChunk.movebleObject.id != 0)
            {
                GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 2, Screen.width / 3, dy * 2), chosenChunk.movebleObject.name);
                GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 3, Screen.width / 3, dy * 2), "Hits " + chosenChunk.movebleObject.curHits + "/" + chosenChunk.movebleObject.maxHits + " AP " + chosenChunk.movebleObject.curAP + "/" + chosenChunk.movebleObject.maxAP);
                GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 4, Screen.width / 3, dy * 2), "Damage " + chosenChunk.movebleObject.damage.damage + " Armor " + chosenChunk.movebleObject.armour.armour);
            }
        }
    }
}
