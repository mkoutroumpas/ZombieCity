using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color[] colors;

    public GameObject projectile;
    public float launchForce = 50;
    public float launchAngle = 20;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(-launchAngle, transform.right) * transform.forward * launchForce, ForceMode.Impulse);

            Color c = colors[Random.Range(0, colors.Length)];
            bullet.GetComponent<Ball>().trail.startColor = c;
            bullet.GetComponent<Ball>().trail.endColor = new Color(c.r, c.g, c.b, 0);
        }

        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * moveSpeed;
        float rotation = Input.GetAxis("Horizontal") * turnSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
    }

}
