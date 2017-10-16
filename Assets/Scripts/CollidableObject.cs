using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollidableObject : MonoBehaviour {

    public Bounds bounds;
    private PhysicsObject physics;
    public static List<GameObject> CollidableObjects = new List<GameObject>();

    enum CollisionDirection { Down, Left, Back, Up, Right, Front }

    private void Start()
    {
        if (bounds == new Bounds())
        {
            bounds = GetComponent<Renderer>().bounds;
        }
        physics = GetComponent<PhysicsObject>();
        CollidableObjects.Add(gameObject);
    }

    private void FixedUpdate()
    {
        bounds = GetComponent<Renderer>().bounds;
        foreach (GameObject colObject in CollidableObjects)
        {
            // Check against every other collidable object.

            if (colObject == gameObject)
                continue; // Skip checking collision with self
            
            CollidableObject otherCollider = colObject.GetComponent<CollidableObject>();
            if (bounds.Intersects(otherCollider.bounds))
            {
                // If bounding boxes are intersecting get the direction of the collision
                CollisionDirection direction = GetDirection(bounds, otherCollider.bounds);
                // "Bump" out of the collider we're encountering.
                BumpOut(otherCollider.bounds,direction);
                // Apply force to other object and a normal force to ourselves.
                Vector3 force = new Vector3();
                // Only apply the force for the direction of the collision. (Because who needs friction?)
                if (!physics.LockPositionX && (direction == CollisionDirection.Left || direction == CollisionDirection.Right))
                    force.x = physics.Mass * physics.Velocity.x / Time.fixedDeltaTime;
                if (!physics.LockPositionY && (direction == CollisionDirection.Up || direction == CollisionDirection.Down))
                    force.y = physics.Mass * physics.Velocity.y / Time.fixedDeltaTime;
                if (!physics.LockPositionZ && (direction == CollisionDirection.Front || direction == CollisionDirection.Back))
                    force.z = physics.Mass * physics.Velocity.z / Time.fixedDeltaTime;
                colObject.GetComponent<PhysicsObject>().AddForceAndNormal(force,physics);
            }
        }
    }

    private void BumpOut(Bounds intersectBounds, CollisionDirection dir)
    {
        Vector3 bumpVector = new Vector3();
        PhysicsObject phys = GetComponent<PhysicsObject>();
        switch (dir)
        {
            case CollisionDirection.Up:
                bumpVector.y = intersectBounds.max.y - bounds.min.y;
                break;
            case CollisionDirection.Down:
                bumpVector.y = intersectBounds.min.y - bounds.max.y;
                break;
            case CollisionDirection.Left:
                bumpVector.x = intersectBounds.min.x - bounds.max.x;
                break;
            case CollisionDirection.Right:
                bumpVector.x = intersectBounds.max.x - bounds.min.x;
                break;
            case CollisionDirection.Front:
                bumpVector.z = intersectBounds.min.z - bounds.max.z;
                break;
            case CollisionDirection.Back:
                bumpVector.z = intersectBounds.max.z - bounds.min.z;
                break;
            default:
                break;
        }
        if (phys.LockPositionX) bumpVector.x = 0;
        if (phys.LockPositionY) bumpVector.y = 0;
        if (phys.LockPositionZ) bumpVector.z = 0;

        transform.position += bumpVector;
    }

    private CollisionDirection GetDirection(Bounds selfBounds, Bounds otherBounds)
    {
        CollisionDirection dir;

        Vector3 selfCenter = selfBounds.center;
        float distanceTop = Mathf.Abs(selfCenter.y - otherBounds.max.y);
        float distanceBot = Mathf.Abs(selfCenter.y - otherBounds.min.y);
        float distanceLeft = Mathf.Abs(selfCenter.x - otherBounds.min.x);
        float distanceRight = Mathf.Abs(selfCenter.x - otherBounds.max.x);
        float distanceFront = Mathf.Abs(selfCenter.z - otherBounds.min.z);
        float distanceBack = Mathf.Abs(selfCenter.z - otherBounds.max.z);
        float closest = new[] { distanceTop, distanceBot, distanceBack, distanceFront, distanceLeft, distanceRight }.Min();
        if (distanceTop == closest)
            dir = CollisionDirection.Up;
        else if (distanceBot == closest)
            dir = CollisionDirection.Down;
        else if (distanceLeft == closest)
            dir = CollisionDirection.Left;
        else if (distanceRight == closest)
            dir = CollisionDirection.Right;
        else if (distanceFront == closest)
            dir = CollisionDirection.Front;
        else
            dir = CollisionDirection.Back;

        return dir;
    }

}
