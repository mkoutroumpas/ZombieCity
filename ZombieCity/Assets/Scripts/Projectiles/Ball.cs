using UnityEngine;

public class Ball : MonoBehaviour
{
    public TrailRenderer trail;

    void Update()
    {
        if (transform.position.y < 0)
            GetComponent<Rigidbody>().isKinematic = true;
    }
}
