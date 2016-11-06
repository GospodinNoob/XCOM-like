using UnityEngine;
using System.Collections;

public class BattleSceneControl : MonoBehaviour {


    public GameObject humanGO;
    GameObject[][] chunks;
    public GameObject chunkPrefab;
    int n, k;
    float deltaX;
    int magicConstant = 53;

    ArrayList way;

    bool enemyTurn;


    Chunk[][] map;

    Chunk chosenChunk;
    PointXY chosenPoint;
    bool move = false;
    bool attack = false;

    float dx;
    float dy;

    public GUIStyle coverButtonStyle;

    GameObject[] playerUnitsGO;
    Unit[] playerUnits;

    GameObject[] enemyUnitsGO;
    Unit[] enemyUnits;

    float[][] dist;
    PointXY[][] parents;
    bool[][] flags;

    bool Valid(int a, int b)
    {
        if (a < 0)
        {
            return false;
        }
        if (b < 0)
        {
            return false;
        }
        if (a >= 6)
        {
            return false;
        }
        if (b >= 6)
        {
            return false;
        }
        if (map[a][b].obj.isEmpty() && map[a][b].movebleObject.isEmpty())
        {
            return true;
        }
        return false;
    }

    float Len(int a, int b, int c, int d)
    {
        return Mathf.Sqrt((a - c) * (a - c) + (b - d) * (b - d));
    }

    void DealDamage(Damage dmg, int x, int y)
    {
        if (!map[x][y].obj.isEmpty())
        {
            map[x][y].obj.DealDamage(dmg);
            if (map[x][y].obj.id == 0)
            {
                chunks[x][y].GetComponent<SpriteActive>().setIdActive(1, false);
            }
        }
        if (!map[x][y].movebleObject.isEmpty())
        {
            map[x][y].movebleObject.DealDamage(dmg);
        }
    }

    void Dijkstra(PointXY p1)
    {
        PointXY p = new PointXY(p1);
        int INF = 10000000;
        int n = 6;
        int k = 6;

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < k; j++)
            {
                dist[i][j] = INF;
                flags[i][j] = false;
            }
        }

        dist[p.x][p.y] = 0;
        int step = 0;
        while ((dist[p.x][p.y] < INF - 1) && (step < 50))
        {
            step++;
           // Dep.yug.Log(dist[p.x][p.y]);
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                   // Dep.yug.Log(Vp.xlid(p.x + i, p.y + j));
                    if (Valid(p.x + i, p.y + j) && (dist[p.x + i][p.y + j] > dist[p.x][p.y] + Len(p.x, p.y, p.x + i, p.y + j)))
                    {

                        dist[p.x + i][p.y + j] = dist[p.x][p.y] + Len(p.x, p.y, p.x + i, p.y + j);
                        //Debug.Log(p.x + " " + p.y);
                        parents[p.x + i][p.y + j] = new PointXY(p);
                    }
                }
            }

            flags[p.x][p.y] = true;

            float min = INF;
            for(int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 6; j++)
                {
                    if ((dist[i][j] < min) && (!flags[i][j]))
                    {
                        p.x = i;
                        p.y = j;
                        min = dist[i][j];
                    }
                }
            }

        }


    }

    void GenerateEnemies()
    {

        enemyUnits = new Unit[3];
        enemyUnitsGO = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(0, 6);
            int b = Random.Range(0, 6);
            while (!map[a][b].obj.isEmpty() || !map[a][b].movebleObject.isEmpty())
            {
                a = Random.Range(0, 6);
                b = Random.Range(0, 6);
            }
            enemyUnits[i] = new Unit(a, b, false);
            Transform tr = this.gameObject.transform;
            enemyUnitsGO[i] = (GameObject)GameObject.Instantiate(humanGO, tr);
            enemyUnitsGO[i].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * a, tr.position.y - dx / 2 - dx + dx * b, tr.position.z);
            float scale = enemyUnitsGO[i].transform.localScale.x * Screen.width / 6 / 2 * (float)0.785 * (53 / dx);
            enemyUnitsGO[i].transform.localScale = new Vector3(scale, scale, scale);
            map[a][b].movebleObject = enemyUnits[i].unit;
            //Debug.Log(1);
        }
    }

    // Use this for initialization
    void Start () {
        enemyTurn = false;
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

        dist = new float[6][];
        parents = new PointXY[6][];
        flags = new bool[6][];
        map = new Chunk[6][];
        chunks = new GameObject[6][];
        for (int i = 0; i < 6; i++)
        {
            map[i] = new Chunk[6];
            dist[i] = new float[6];
            parents[i] = new PointXY[6];
            flags[i] = new bool[6];
            chunks[i] = new GameObject[6];
            for(int j = 0; j < 6; j++)
            {

                map[i][j] = new Chunk();
                Transform tr = this.gameObject.transform;
                chunks[i][j] = (GameObject) GameObject.Instantiate(chunkPrefab, tr);
                chunks[i][j].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * i, tr.position.y - dx / 2 - dx + dx * j, tr.position.z);
                //Debug.Log(dx);
                float scale = chunks[i][j].transform.localScale.x * Screen.width / 6 / 2 * (float)0.787 * (53/dx);
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
            float scale = playerUnitsGO[i].transform.localScale.x * Screen.width / 6 / 2 * (float)0.787 * (53 / dx);
            playerUnitsGO[i].transform.localScale = new Vector3(scale, scale, scale);
            map[a][b].movebleObject = playerUnits[i].unit;
        }

        GenerateEnemies();

        chosenChunk = map[0][0];
        this.gameObject.GetComponent<UnitMove>().SetDx(dx);
    }

    int enemyUnitNow;

    
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < 3; i++)
        {
            if (playerUnits[i].unit.id == 0)
            {
                playerUnitsGO[i].active = false;
            }
            if (enemyUnits[i].unit.id == 0)
            {
                enemyUnitsGO[i].active = false;
            }
        }

        if (enemyTurn && !block)
        {
            int i = enemyUnitNow;
            if (enemyUnits[i].unit.id != 0)
            {
                Dijkstra(enemyUnits[i].point);
                Debug.Log(enemyUnits[i]);
                bool tf = false;
                Unit targetUnit = new Unit();
                for (int j = 0; j < 3; j++)
                {
                    if ((Mathf.Abs(playerUnits[j].point.x - enemyUnits[i].point.x) <= 1) && (Mathf.Abs(playerUnits[j].point.y - enemyUnits[i].point.y) <= 1))
                    {
                        tf = true;
                        targetUnit = playerUnits[j];
                    }
                }
                if (tf)
                {
                    enemyUnits[i].unit.curAP = 0;
                    targetUnit.unit.DealDamage(enemyUnits[i].unit.damage);
                }
                else
                {
                    PointXY p = new PointXY(1, 1);
                    int cost = 1000;
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = 0; k < 6; k++)
                        {
                            for (int r = 0; r < 3; r++)
                            {
                                if ((map[j][k].movebleObject.isEmpty()) && (map[j][k].obj.isEmpty()) && ((Mathf.Abs(playerUnits[r].point.x - j) <= 1) && (Mathf.Abs(playerUnits[r].point.y - k)) <= 1))
                                {
                                    if (!tf)
                                    {
                                        tf = true;
                                        p = new PointXY(j, k);
                                        targetUnit = playerUnits[r];
                                        cost = Mathf.CeilToInt(dist[j][k] / enemyUnits[i].unit.speed);
                                    }
                                    else
                                    {
                                        if (Mathf.CeilToInt(dist[j][k] / enemyUnits[i].unit.speed) < cost)
                                        {
                                            tf = true;
                                            targetUnit = playerUnits[r];
                                            cost = Mathf.CeilToInt(dist[j][k] / enemyUnits[i].unit.speed);
                                            p = new PointXY(j, k);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Debug.Log(p.x.ToString() + " " + p.y.ToString());
                    if (tf)
                    {
                        enemyUnits[i].unit.doAction(cost);
                        //chunks[p.x][p.y].GetComponent<SpriteActive>().setIdActive(0, false);
                        Swap(enemyUnits[i].point, p, false);
                        if (enemyUnits[i].unit.curAP > 0)
                        {
                            targetUnit.unit.DealDamage(enemyUnits[i].unit.damage);
                            enemyUnits[i].unit.curAP = 0;
                        }
                    }
                    else
                    {
                        enemyUnits[i].unit.curAP = 0;
                    }
                }
            }
            enemyUnitNow++;
            if (enemyUnitNow == 3)
            {
                enemyTurn = false;
            }
        }
        

	}

    Unit GetUnit(PointXY p)
    {
        Unit un = playerUnits[0];
        for(int i = 0; i < 3; i++)
        {
            if ((playerUnits[i].point.x == p.x) && (playerUnits[i].point.y == p.y))
            {
                un = playerUnits[i];
                break;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if ((enemyUnits[i].point.x == p.x) && (enemyUnits[i].point.y == p.y))
            {
                un = enemyUnits[i];
                break;
            }
        }
        return un;
    }

    ArrayList GetWay(PointXY p)
    {
        ArrayList newWay = new ArrayList();
        newWay.Add(p);
        int step = 0;
        Debug.Log(p.x + " " + p.y + " " + dist[p.x][p.y]);
        while (dist[p.x][p.y] > 0)
        {
           // step++;
            //if (step > 50)
            //{
              //  break;
            //}
            p = parents[p.x][p.y];
            Debug.Log(p.x + " " + p.y + " " + dist[p.x][p.y]);
            newWay.Add(p);
        }
        //if (step > 50)
        //{
          //  Debug.Log(dist[p.x][p.y]);
        //}
        return newWay;
    }

    bool block = false;

    public void SetBlock(bool tf)
    {
        block = tf;
    }

    void Swap(PointXY p1, PointXY p2, bool tf)
    {
        Unit playerUnit = GetUnit(p1);
        playerUnit.point = new PointXY(p2);
        map[p1.x][p1.y].movebleObject = map[p2.x][p2.y].movebleObject;
        map[p2.x][p2.y].movebleObject = playerUnit.unit;

        way = GetWay(p2);

       // Debug.Log(p2.x.ToString() + " " + p2.y.ToString());

        if (tf)
        {
            for (int i = 0; i < 3; i++)
            {
                if (playerUnits[i] == playerUnit)
                {
                    Transform tr = this.gameObject.transform;
                    //  Debug.Log(1);
                    block = true;
                    this.GetComponent<UnitMove>().SetWay(playerUnitsGO[i], way);
                }
                //  Debug.Log(playerUnits[i].x);
                //playerUnitsGO[i].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * playerUnits[i].point.x, tr.position.y - dx / 2 - dx + dx * playerUnits[i].point.y, tr.position.z);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (enemyUnits[i] == playerUnit)
                {
                    Transform tr = this.gameObject.transform;
                    //  Debug.Log(playerUnits[i].x);
                    block = true;
                    enemyUnitNow = 0;
                    this.GetComponent<UnitMove>().SetWay(enemyUnitsGO[i], way);
                    //enemyUnitsGO[i].transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * enemyUnits[i].point.x, tr.position.y - dx / 2 - dx + dx * enemyUnits[i].point.y, tr.position.z);
                }
            }
        }
    }

    void CheckTextures()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                if (move && (dist[i][5 - j] <= GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed))
                {
                    if (!Valid(i - 1, 5 - j) || (dist[i - 1][5 - j] > GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed))
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-3, true);
                    }
                    else
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-3, false);
                    }
                    if (!Valid(i + 1, 5 - j) || (dist[i + 1][5 - j] > GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed))
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-4, true);
                    }
                    else
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-4, false);
                    }
                    if (!Valid(i, 5 - j - 1) || (dist[i][5 - j - 1] > GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed))
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-2, true);
                    }
                    else
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-2, false);
                    }
                    if (!Valid(i, 5 - j + 1) || (dist[i][5 - j + 1] > GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed))
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-1, true);
                    }
                    else
                    {
                        chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-1, false);
                    }
                }
                else
                {
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-1, false);
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-2, false);
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-3, false);
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-4, false);
                }
                if (!map[i][5 - j].movebleObject.isEmpty() && (map[i][5 - j].movebleObject.player))
                {
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-6, true);
                }
                else
                {
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-6, false);
                }
                if (attack && (Mathf.Abs(chosenPoint.x - i) <= 1) && (Mathf.Abs(chosenPoint.y - (5 - j)) <= 1) && ((!map[i][5 - j].obj.isEmpty()) || (!map[i][5 - j].movebleObject.isEmpty() && !map[i][5 - j].movebleObject.player)))
                {
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-5, true);
                }
                else
                {
                    chunks[i][5 - j].GetComponent<SpriteActive>().setIdActive(-5, false);
                }
            }
        }
    }

    void CheckNextTurn()
    {
        Unit un = playerUnits[0];
        bool tf = true;
        for (int i = 0; i < 3; i++)
        {
            if ((playerUnits[i].unit.id != 0) && (playerUnits[i].unit.curAP > 0))
            {
                tf = false;
                un = playerUnits[i];
                chosenPoint = new PointXY(playerUnits[i].point);
            }
        }
        if (tf)
        {
            Debug.Log(tf);
            enemyTurn = true;
            for (int i = 0; i < 3; i++)
            {
                playerUnits[i].unit.calculateNewTurn();
                enemyUnits[i].unit.calculateNewTurn();
            }
            chosenPoint = new PointXY(playerUnits[0].point);
        }
    }

    void OnGUI()
    {
        if ((!enemyTurn) && (!block))
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    string s;
                    if ((!move) || (dist[i][5 - j] == 0))
                    {
                        s = "";
                    }
                    else
                    {
                        int k = Mathf.CeilToInt(dist[i][5 - j] / GetUnit(chosenPoint).unit.speed);
                        if (k > GetUnit(chosenPoint).unit.curAP)
                        {
                            s = "";
                        }
                        else
                        {
                            s = (k).ToString();
                            //s = parents[i][5 - j].x.ToString() + parents[i][5 - j].y.ToString();
                        }
                    }
                    if (GUI.Button(new Rect(i * dx + deltaX, j * dy, dx, dy), s, coverButtonStyle))
                    {
                        if (!move && !attack)
                        {
                            chosenChunk = map[i][5 - j];
                            chosenPoint = new PointXY(i, 5 - j);
                        }
                        else
                        {
                            if (move)
                            {
                                if (map[i][5 - j].obj.isEmpty() && map[i][5 - j].movebleObject.isEmpty() && (GetUnit(chosenPoint).unit.curAP * GetUnit(chosenPoint).unit.speed >= dist[i][5 - j]))
                                {
                                    int k = Mathf.CeilToInt(dist[i][5 - j] / GetUnit(chosenPoint).unit.speed);
                                    GetUnit(chosenPoint).unit.doAction(k);
                                    Swap(chosenPoint, new PointXY(i, 5 - j), true);
                                    chosenPoint = new PointXY(i, 5 - j);
                                    if (GetUnit(chosenPoint).unit.curAP == 0)
                                    {
                                        CheckNextTurn();
                                    }
                                }
                                move = false;
                            }
                            if (attack)
                            {
                                if ((GetUnit(chosenPoint).unit.curAP > 0) && (Mathf.Abs(chosenPoint.x - i) <= 1) && (Mathf.Abs(chosenPoint.y - (5 - j)) <= 1) && ((!map[i][5 - j].obj.isEmpty()) || (!map[i][5 - j].movebleObject.isEmpty() && !map[i][5 - j].movebleObject.player)))
                                {
                                    DealDamage(GetUnit(chosenPoint).unit.damage, i, 5 - j);
                                    GetUnit(chosenPoint).unit.curAP = 0;
                                    CheckNextTurn();
                                }
                                attack = false;
                            }
                        }
                    }
                }
            }
            CheckTextures();
            if (chosenChunk != null)
            {
                if ((chosenChunk.movebleObject.id != 0) && (chosenChunk.movebleObject.player))
                {
                    if (GUI.Button(new Rect(Screen.width / 3 - dx, dy * 6, Screen.width / 6, dy), "Move"))
                    {
                        move = !move;
                        if (move)
                        {
                            Dijkstra(chosenPoint);
                        }
                    }
                    if (GUI.Button(new Rect(Screen.width / 3 - dx, dy * 7, Screen.width / 6, dy), "Attack"))
                    {
                        attack = !attack;
                    }
                }
                GUI.Label(new Rect(0, dy * 6, Screen.width / 3, dy * 2), chosenChunk.landCover);
                if (chosenChunk.obj.id != 0)
                {
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 3, Screen.width / 3, dy * 2), chosenChunk.obj.name);
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 3 + dx * 2 / 6, Screen.width / 3, dy * 2), "Hits " + chosenChunk.obj.curHits + "/" + chosenChunk.obj.maxHits);
                }
                if (chosenChunk.movebleObject.id != 0)
                {
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6, Screen.width / 3, dy * 2), chosenChunk.movebleObject.name);
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 2, Screen.width / 3, dy * 2), "Hits " + chosenChunk.movebleObject.curHits + "/" + chosenChunk.movebleObject.maxHits);
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 3, Screen.width / 3, dy * 2), "AP " + chosenChunk.movebleObject.curAP + "/" + chosenChunk.movebleObject.maxAP);
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 4, Screen.width / 3, dy * 2), "Damage " + chosenChunk.movebleObject.damage.damage);
                    GUI.Label(new Rect(0, dy * 6 + dx * 2 / 6 * 5, Screen.width / 3, dy * 2), "Armor " + chosenChunk.movebleObject.armour.armour);
                }
            }
        }
    }
}
