using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndFall : MonoBehaviour
{
   public Vector3 jump;
    public float jumpForce = 20.0f;
     public float rotateForce=50f;

    Rigidbody rb;
    void Awake(){
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        rb.AddTorque(transform.up * rotateForce * rotateForce);
        rb.AddTorque(transform.right * jumpForce * jumpForce);
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    }
}
