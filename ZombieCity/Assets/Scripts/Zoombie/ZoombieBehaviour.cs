using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZoombieBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject ZombiePrefab;
    [SerializeField]
    public GameObject PlayerCamera;

    public int RoadWidth = 30;
    public int NumberOfZombies = 100;

    private List<ZombieEntity> _zombieEntities;
    private EntityManager _entityManager;

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var zombieEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ZombiePrefab, settings);

        _zombieEntities = new List<ZombieEntity>();

        for (int index = 0; index < NumberOfZombies; index++)
        {
            var spawnZombie = _entityManager.Instantiate(zombieEntity);
            var initialPosition = GetSpawnPointInWorldSpace();
            var moveSpeed = UnityEngine.Random.Range(0.01f, 0.05f);

            _zombieEntities.Add(new ZombieEntity
            {
                Entity = spawnZombie,
                Position = initialPosition,
                MoveSpeed = moveSpeed
            });

            _entityManager.SetComponentData(spawnZombie, new Translation { Value = initialPosition });
        }
    }

    public void Update()
    {
        var startTime = Time.realtimeSinceStartup;

        var _moveSpeeds = new NativeArray<float>(NumberOfZombies, Allocator.TempJob);
        var _positions = new NativeArray<float3>(NumberOfZombies, Allocator.TempJob);

        for (var index = 0; index < NumberOfZombies; index++)
        {
            _moveSpeeds[index] = _zombieEntities[index].MoveSpeed;
            _positions[index] = _zombieEntities[index].Position;
        }

        var zombieMovementJob = new ZombieMovementJob
        {
            DeltaTime = Time.deltaTime,
            MoveSpeeds = _moveSpeeds,
            Positions = _positions
        };

        var jobHandle = zombieMovementJob.Schedule(NumberOfZombies, 10);
        jobHandle.Complete();

        for (var index = 0; index < NumberOfZombies; index++)
        {
            _zombieEntities[index].Position = _positions[index];
            _zombieEntities[index].MoveSpeed = _moveSpeeds[index];

            _entityManager.SetComponentData(_zombieEntities[index].Entity, new Translation { Value = _zombieEntities[index].Position });
        }

        _moveSpeeds.Dispose();
        _positions.Dispose();
    }

    private float3 GetSpawnPointInWorldSpace()
    {
        var x = transform.position.x + UnityEngine.Random.Range(-RoadWidth / 2, RoadWidth / 2);
        var z = transform.position.z + UnityEngine.Random.Range(-2f, 2f);
        var y = transform.position.y;

        return new float3(x, y, z);
    }
}

public class ZombieEntity
{
    public Entity Entity;
    public float3 Position;
    public float MoveSpeed;
}