using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneController : MonoBehaviour
{

    [Range(-1, 1)]
    public float Thrust, Tilt, Lift, Rotate;

    [Space(10)]
    public Rigidbody rb;
    public Animator anim;

    [Space(10)]
    public float lift = 5;
    public float speed = 5;

    public float rotationSpeed = 5;
    public float blendSpeed = 2;

    [Space(10)]
    public float angle;

    private Quaternion rotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Lift = Input.GetAxis("Lift");
        Rotate = Input.GetAxis("Rotate");
        Thrust = Input.GetAxis("Vertical");
        Tilt = Input.GetAxis("Horizontal");

        var v = anim.GetFloat("Vertical");
        var h = anim.GetFloat("Horizontal");
        var r = anim.GetFloat("Rotate");

        anim.SetFloat("Vertical", Mathf.Clamp(Mathf.Lerp(v, Thrust, blendSpeed * Time.deltaTime), -1, 1));
        anim.SetFloat("Horizontal", Mathf.Clamp(Mathf.Lerp(h, Tilt, blendSpeed * Time.deltaTime), -1, 1));
        anim.SetFloat("Rotate", Mathf.Clamp(Mathf.Lerp(r, Rotate, blendSpeed * Time.deltaTime), -1, 1));

        var dir = new Vector3(Tilt * speed, Lift * lift, Thrust * speed);

        rb.AddRelativeForce(dir, ForceMode.Impulse);

        var cv3 = rb.velocity;

        cv3.x = Mathf.Clamp(cv3.x, -speed, speed);
        cv3.y = Mathf.Clamp(cv3.y, -lift, lift);
        cv3.z = Mathf.Clamp(cv3.z, -speed, speed);

        rb.velocity = cv3;

        angle += Input.GetAxis("Rotate") * rotationSpeed;

        if (angle >= 360)
            angle = 0;
        else if (angle <= -360)
            angle = 0;

        rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
