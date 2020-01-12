using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehavior : MonoBehaviour
{
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Speed;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 force = new Vector3(x, 0, 0);
        rigidbody.AddForce(force);
    }
}
