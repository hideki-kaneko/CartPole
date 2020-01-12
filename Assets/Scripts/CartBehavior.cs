using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehavior : MonoBehaviour
{
    // Public
    public float Speed = 1;
    public GameObject mObjPole;

    // Private
    private Vector3 initDir;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbodyPole = mObjPole.GetComponent<Rigidbody>();
        initDir = rigidbodyPole.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Speed;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 force = new Vector3(x, 0, 0);
        rigidbody.AddForce(force);
    }

    private void OnGUI()
    {
        Rigidbody rigidbodyPole = mObjPole.GetComponent<Rigidbody>();
        Vector3 currentDir = rigidbodyPole.transform.up;
        float rotDeg = Mathf.Abs(Vector3.Angle(currentDir, initDir));
        string textRot = string.Format("Rotation: {0:F2}", rotDeg);

        GUI.Box(new Rect(10, 10, 120, 90), "Menu");
        GUI.Label(new Rect(20, 30, 100, 90), textRot);
    }
}
