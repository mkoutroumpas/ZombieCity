using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
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
    private NativeList<float> _moveSpeeds;
    private NativeList<float3> _positions;

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _zombieEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ZombiePrefab, settings);

        _zombieEntities = new List<ZombieEntity>();

        _moveSpeeds = new NativeList<float>();
        _positions = new NativeList<float3>();
    }

    private void Update()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

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

        var zombieMovementJob = new ZombieMovementJob()
        {
            DeltaTime = Time.deltaTime,
            MoveSpeeds = _moveSpeeds,
            Positions = _positions
        };


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