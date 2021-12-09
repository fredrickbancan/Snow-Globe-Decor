using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringHookes : MonoBehaviour
{
    private Rigidbody rb1 = null;
    private Rigidbody rb2 = null;
    private float elasticity = 0.0f;
    private float damping = 0.0f;
    private float restLength = 0.0f;

    private void FixedUpdate()
    {
        Vector3 pos1 = rb1.transform.position;
        Vector3 pos2 = rb2.transform.position;
        Vector3 distance = pos2 - pos1;
        float length = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y + distance.z * distance.z);
        //Apply damping
        Vector3 relativeVelocity = rb2.velocity - rb1.velocity;
        //F = -kX - bv
        Vector3 force = distance * elasticity * (restLength - length) - damping * relativeVelocity;
        rb1.AddForce(-force * Time.fixedDeltaTime);
        rb2.AddForce(force * Time.fixedDeltaTime);
    }

    public void CreateSpring(Rigidbody rb1, Rigidbody rb2, float elasticity, float damping, float restLength)
    {
        this.rb1 = rb1;
        this.rb2 = rb2;
        this.elasticity = elasticity;
        this.damping = damping;
        this.restLength = restLength;
    }
}
