using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZoombieBehaviour : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public GameObject PlayerCamera;

    [SerializeField]
    public float ZombieSpawnPeriod = 10f;

    public int RoadWidth = 15;

    private Entity _zombieEntity;
    private float _nextZombieSpawnTime = 0.0f;
    private List<ZombieEntity> _zombieEntities;

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _zombieEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ZombiePrefab, settings);

        _zombieEntities = new List<ZombieEntity>();
    }

    private void Update()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var _moveSpeeds = new NativeList<float>();
        var _positions = new NativeList<float3>();

        if (Time.time > _nextZombieSpawnTime)
        {
            var spawnZombie = entityManager.Instantiate(_zombieEntity);

            var initialPosition = GetSpawnPointInWorldSpace();

            _positions.Add(initialPosition);

            var moveSpeed = UnityEngine.Random.Range(1f, 5f);

            _moveSpeeds.Add(moveSpeed);

            entityManager.SetComponentData(spawnZombie, new Translation { Value = initialPosition });

            _zombieEntities.Add(new ZombieEntity
            {
                Entity = spawnZombie,
                Position = initialPosition,
                MoveSpeed = moveSpeed
            });

            _nextZombieSpawnTime += ZombieSpawnPeriod;
        }

        var zombieMovementJob = new ZombieMovementJob
        {
            DeltaTime = Time.deltaTime,
            MoveSpeeds = _moveSpeeds,
            Positions = _positions
        };

        var jobHandle = zombieMovementJob.Schedule();
        jobHandle.Complete();

        for (var index = 0; index < _zombieEntities.Count; index++)
        {
            entityManager.SetComponentData(_zombieEntities[index].Entity, new Translation { Value = _zombieEntities[index].Position });
        }

        _moveSpeeds.Dispose();
        _positions.Dispose();
    }
    
    private float3 GetSpawnPointInWorldSpace()
    {
        var x = transform.position.x + UnityEngine.Random.Range(-RoadWidth / 2, RoadWidth / 2);
        var z = transform.position.z + UnityEngine.Random.Range(-10f, 10f);
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