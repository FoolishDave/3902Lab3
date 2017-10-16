using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    const float Gravity = -9.8f;
    static Vector3 GravityDirection = Vector3.up;

    public Vector3 Velocity = new Vector3();
    public float Mass;
    public bool LockPositionX;
    public bool LockPositionY;
    public bool LockPositionZ;
    private List<Vector3> forces = new List<Vector3>();

    public void AddForce(Vector3 force)
    {
        forces.Add(force);
    }

    public void AddForceAndNormal(Vector3 force, PhysicsObject offender)
    {
        forces.Add(force);
        offender.AddForce(-force);
    }

    public void FixedUpdate()
    {
        AddForce(GravityDirection * Gravity * Mass);
    }

    public void LateUpdate()
    {
		Vector3 acceleration = AccelerationFromForces();
		Velocity += acceleration * Time.fixedDeltaTime;

        if (LockPositionX) Velocity.x = 0;
        if (LockPositionY) Velocity.y = 0;
        if (LockPositionZ) Velocity.z = 0;

		transform.position += Velocity * Time.fixedDeltaTime;
		forces.Clear();
    }

    private Vector3 AccelerationFromForces()
    {
        // sum(F) = ma
        // a = sum(F)/m
        Vector3 accel = new Vector3();
        foreach (Vector3 force in forces)
            accel += force;
        
        return accel /= Mass;
    }
}
