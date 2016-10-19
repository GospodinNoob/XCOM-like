using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    private Camera cameraGO;

	// Use this for initialization
	void Start () {
        cameraGO = this.gameObject.GetComponent<Camera>();
        float orthographicSize = cameraGO.pixelHeight / 2;
        if (orthographicSize != cameraGO.orthographicSize)
        {
            cameraGO.orthographicSize = orthographicSize;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
