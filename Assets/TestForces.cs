using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForces : MonoBehaviour {

    public Vector3 TestForce;
    public string TestKey;
    private PhysicsObject physics;

    private void Start()
    {
        physics = GetComponent<PhysicsObject>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(TestKey))
            physics.AddForce(TestForce);
	}
}
