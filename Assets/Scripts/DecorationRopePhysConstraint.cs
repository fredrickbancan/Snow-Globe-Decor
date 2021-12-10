using UnityEngine;

public class DecorationRopePhysConstraint : MonoBehaviour
{
    public Rigidbody rbA = null;
    public Vector3 rbAPrevPos;
    public Rigidbody rbB = null;
    public Vector3 rbBPrevPos;
    public float restingDist = 0.02F;
    public float currentDist = 0.0F;
}
