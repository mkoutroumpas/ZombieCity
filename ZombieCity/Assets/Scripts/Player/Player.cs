using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color[] colors;

    public GameObject projectile;
    public float launchForce = 50;
    public float launchAngle = 20;
    public float launchHorzCount = 10;
    public float launchHorzSpread = 5;
    public float launchVertCount = 1;
    public float launchVertSpread = 0;
    public float launchVariability = 1;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            float vHalf = Mathf.Max(0.5f, launchVertCount / 2f);
            float vStep = (launchVertSpread / launchVertCount);
            float hHalf = Mathf.Max(0.5f, launchHorzCount / 2f);
            float hStep = (launchHorzSpread / launchHorzCount);
            int count = 0;
            for (float v = -vHalf; v < vHalf; v++)
            {
                for (float h = -hHalf; h < hHalf; h++)
                {
                    GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
                    bullet.transform.Rotate(bullet.transform.up, h * hStep + Random.Range(-0.5f * launchVariability, 0.5f * launchVariability));
                    bullet.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(-(launchAngle + v * vStep) + Random.Range(-0.5f * launchVariability, 0.5f * launchVariability), bullet.transform.right) * bullet.transform.forward * (launchForce + Random.Range(-5f * launchVariability, 5f * launchVariability)), ForceMode.Impulse);
                    Color c = colors[Random.Range(0, colors.Length)];
                    bullet.GetComponent<Ball>().trail.startColor = c;
                    bullet.GetComponent<Ball>().trail.endColor = new Color(c.r, c.g, c.b, 0);
                    count++;
                }
            }
            Debug.Log($"Spawned {count} bullets!");
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
