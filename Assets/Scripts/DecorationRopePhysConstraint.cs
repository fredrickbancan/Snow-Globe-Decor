using UnityEngine;

public struct DecorationRopePhysConstraint
{
    public Rigidbody rbA;
    public Vector3 rbAPrevPos;
    public Rigidbody rbB;
    public Vector3 rbBPrevPos;
    public float restingDist;
    public float currentDist;
}
