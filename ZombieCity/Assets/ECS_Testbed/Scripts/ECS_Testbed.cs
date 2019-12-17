using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class ECS_Testbed : MonoBehaviour
{
    private List<ZMoveableObject> _moveableObjects;

    [SerializeField]
    public Transform ObjectToMove;
    [SerializeField]
    public int NumberOfObjects = 1000;

    private void Start()
    {
        _moveableObjects = new List<ZMoveableObject>();
        
        for (var i = 0; i < NumberOfObjects; i++)
        {
            Transform instance = Instantiate(
                ObjectToMove,
                new Vector3(UnityEngine.Random.Range(-18f, 18f),
                            UnityEngine.Random.Range(-10f, 10f),
                            UnityEngine.Random.Range(-10f, 10f)), Quaternion.identity);

            _moveableObjects.Add(new ZMoveableObject
            {
                Transform = instance,
                Velocity = UnityEngine.Random.Range(0.1f, 1.0f)
            });
        }
    }

    private void Update()   
    {
        float startTime = Time.realtimeSinceStartup;

        var _velocities = new NativeArray<float>(_moveableObjects.Count, Allocator.TempJob);
        var _positions = new NativeArray<float3>(_moveableObjects.Count, Allocator.TempJob);

        for (var i = 0; i < _moveableObjects.Count; i++)
        {
            _positions[i] = _moveableObjects[i].Transform.position;
            _velocities[i] = _moveableObjects[i].Velocity;
        }

        var job = new ZTranslationJob
        {
            MoveSpeeds = _velocities,
            Positions = _positions,
            DeltaTime = Time.deltaTime,
            MinusDirection = true
        };

        var jobHandle = job.Schedule(_moveableObjects.Count, 10);
        jobHandle.Complete();

        for (var i = 0; i < _moveableObjects.Count; i++)
        {
            _moveableObjects[i].Transform.position = _positions[i];
            _moveableObjects[i].Velocity = _velocities[i];
        }

        _velocities.Dispose();
        _positions.Dispose();

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    public class ZMoveableObject
    {
        public Transform Transform;
        public float Velocity;
    }
}

