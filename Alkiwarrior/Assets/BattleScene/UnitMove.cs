using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour {

    ArrayList newWay;
    GameObject unit;

    bool flag;

	// Use this for initialization
	void Start () {
        flag = false;
        newWay = new ArrayList();
	}

    int segment = 0;
    float timer;

    float dx;

    public void SetDx(float a)
    {
        dx = a;
    }

    void Rotation(int vx, int vy)
    {
        int angle = 0;
        if (vx == 1)
        {
            if (vy == 1)
            {
                angle = 180 + 45;
            }
            if (vy == 0)
            {
                angle = 180;
            }
            if (vy == -1)
            {
                angle = 180 - 45;
            }
        }
        if (vx == 0)
        {
            if (vy == 1)
            {
                angle = 360 - 90;
            }
            if (vy == -1)
            {
                angle = 90;
            }
        }
        if (vx == -1)
        {
            if (vy == 1)
            {
                angle = 90 - (45 + 90);
            }
            if (vy == 0)
            {
                angle = 0;
            }
            if (vy == -1)
            {
                angle = 45;
            }
        }
        unit.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void SetWay(GameObject go, ArrayList way)
    {
        newWay = new ArrayList();
        for (int i = way.Count - 1; i >= 0; i--)
        {
            newWay.Add(way[i]);
        }
        unit = go;
        segment = 1;
        timer = 0;
        flag = true;
    }
    float len;
	
	// Update is called once per frame
	void Update () {
	    if (flag)
        {
            //Debug.Log(newWay.Count);
            PointXY a = (PointXY)newWay[segment - 1];
            PointXY b = new PointXY(0, 0);
            if (segment < newWay.Count)
            {
                b = (PointXY)newWay[segment];
            }
            if (segment < newWay.Count)
            {
                
                if (Time.time - timer > len)
                {
                    Transform tr = this.gameObject.transform;
                    Rotation(b.x - a.x, b.y - a.y);
                    len = Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
                    unit.gameObject.transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * a.x, tr.position.y - dx / 2 - dx + dx * a.y, tr.position.z);
                    segment++;
                    timer = Time.time;
                    Vector2 speed = new Vector2((b.x - a.x) / len, (b.y - a.y) / len) * dx;
                    unit.gameObject.GetComponent<Rigidbody2D>().velocity = speed;
                }
            }
            else
            {
                if (Time.time - timer > len)
                {
                    flag = false;
                    Transform tr = this.gameObject.transform;
                    Vector2 speed = new Vector2(0, 0);
                    unit.gameObject.GetComponent<Rigidbody2D>().velocity = speed;
                    unit.gameObject.transform.position = new Vector3(tr.position.x - dx / 2 - 2 * dx + dx * a.x, tr.position.y - dx / 2 - dx + dx * a.y, tr.position.z);
                    this.gameObject.GetComponent<BattleSceneControl>().SetBlock(false);
                }
                }
        }
	}
}
