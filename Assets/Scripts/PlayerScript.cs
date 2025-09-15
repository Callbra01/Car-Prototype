using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOT NEEDED, REMOVE NEXT COMMIT
public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    private float currentSpeed;
    public float topSpeed;

    private float h;
    private float v;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pInput();
    }

    private void pInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

    }
}
