using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ZoombieBehaviour : MonoBehaviour
{
    public GameObject ZombiePrefab;
    
    private Entity ZombieEntity;

    public void Start()
    {
        QualitySettings.vSyncCount = 0;

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        ZombieEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ZombiePrefab, settings);
    }

    private void Update()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var spawnZombie = entityManager.Instantiate(ZombieEntity);

        entityManager.SetComponentData(spawnZombie, new Translation { Value = GetSpawnPointInWorldSpace() });
    }

    private float3 GetSpawnPointInWorldSpace()
    {
        return new float3(0f, 0f, 0f);
    }
}
