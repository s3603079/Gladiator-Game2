using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//マウスの位置など、視点の向きを取得するもの
//

public class FocusPoint : MonoBehaviour {

    private Vector3 _focusPoint;
    private Vector3 _screenToWorldPointPosition;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		// Vector3でマウス位置座標を取得する
        _focusPoint = Input.mousePosition;
        // Z軸修正
        _focusPoint.z = 10f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        _screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(_focusPoint);
        // ワールド座標に変換されたマウス座標を代入
        gameObject.transform.position = _screenToWorldPointPosition;
	}

    public Vector3 GetPoint()
    {
        return _focusPoint;
    }
}