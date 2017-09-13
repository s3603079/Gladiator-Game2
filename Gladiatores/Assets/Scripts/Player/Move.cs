using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    Rigidbody2D rb2d;
	
    // Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        var x = Input.GetAxis("Horizontal");
        //transform.Translate(new Vector3(x, 0.0f, 0.0f) * Time.deltaTime);
        rb2d.velocity = transform.right * x * 100.0f * Time.deltaTime;
    }
}
