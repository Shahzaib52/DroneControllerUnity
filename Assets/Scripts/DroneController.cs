using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneController : MonoBehaviour
{

    [Range(-1, 1)]
    public float Thrust, Tilt, Lift;

    [Space(10)]
    public Rigidbody rb;
    public Animator anim;

    [Space(10)]
    public float lift = 5;
    public float speed = 5;

    public float stablize = 5;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Thrust = Time.fixedDeltaTime * Input.GetAxis("Vertical") * speed;
        Tilt = Time.fixedDeltaTime * Input.GetAxis("Horizontal") * speed;
        Lift = Time.fixedDeltaTime * Input.GetAxis("Lift") * lift;

        var v = anim.GetFloat("Vertical");
        var h = anim.GetFloat("Horizontal");

        anim.SetFloat("Vertical", Mathf.Clamp(Mathf.Lerp(v, Thrust, 2 * Time.deltaTime), -1, 1));
        anim.SetFloat("Horizontal", Mathf.Clamp(Mathf.Lerp(h, Tilt, 2 * Time.deltaTime), -1, 1));

        rb.AddForce(new Vector3(Tilt, Lift, Thrust), ForceMode.Impulse);

        var cv3 = rb.velocity;

        cv3.x = Mathf.Clamp(cv3.x, -speed, speed);
        cv3.y = Mathf.Clamp(cv3.y, -lift, lift);
        cv3.z = Mathf.Clamp(cv3.z, -speed, speed);

        rb.velocity = cv3;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 5);
    }
}
