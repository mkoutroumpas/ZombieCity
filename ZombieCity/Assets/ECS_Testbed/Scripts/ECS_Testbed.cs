using System.Collections.Generic;
using UnityEngine;

public class ECS_Testbed : MonoBehaviour
{
    [SerializeField]
    private Transform ObjectToMove;
    private List<ZMoveableObject> moveableObjects;

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;



        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }



    public class ZMoveableObject
    {
        public Transform Transform;
        public float moveZ;
    }
}

