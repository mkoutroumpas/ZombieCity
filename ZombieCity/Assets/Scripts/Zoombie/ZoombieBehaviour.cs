using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoombieBehaviour : MonoBehaviour{
    public GameObject Player;
    public float movementSpeed = 4;
    // public Transform player;
    // private Rigidbody zoombiemove;
    // Start is called before the first frame update
    void Start()
    {
       // zoombiemove = this.GetComponent<Ridigbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform);
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        //Vector3 direction = player.position - transform.position;
        //Debug.Log(direction);

    }
}