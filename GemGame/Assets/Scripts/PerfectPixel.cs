using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectPixel : MonoBehaviour {
    [SerializeField]
    private float PPUScale = 1;
    [SerializeField]
    private float PPU = 32;

    private Camera thisCamera;

	void Start () {
        thisCamera = GetComponent<Camera>();
        thisCamera.orthographicSize = ((Screen.width) / (PPUScale * PPU)) * 0.5f;
	}
}
