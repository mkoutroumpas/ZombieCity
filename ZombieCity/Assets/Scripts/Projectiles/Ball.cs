using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class Ball : MonoBehaviour//, IConvertGameObjectToEntity
{
    public TrailRenderer trail;

    //public Vector3 initialForce = Vector3.zero;
    //private bool addedForce = false;

    void Start()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    void Update()
    {
        //if (transform.position.y < 0)
        //    GetComponent<Rigidbody>().isKinematic = true;

        //if (!addedForce)
        //{
        //    GetComponent<Rigidbody>().AddForce(initialForce, ForceMode.Impulse);
        //    Debug.Log($"Ball at {transform.position.ToString()} with force {initialForce.ToString()}");
        //    addedForce = true;
        //}
    }

    //public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    //{
    //    dstManager.AddComponent(entity, typeof(InitialForce));
    //    dstManager.AddComponentData(entity, new InitialForce { Value = initialForce });
    //    Debug.Log($"Convert Ball to Entity");
    //}
}

//public struct InitialForce : IComponentData
//{
//    public Vector3 Value;
//}