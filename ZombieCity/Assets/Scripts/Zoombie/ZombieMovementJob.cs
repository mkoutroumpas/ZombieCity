using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct ZombieMovementJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> MoveSpeeds;
    public NativeArray<float3> Positions;

    public float DeltaTime;

    public void Execute(int index)
    {
        Positions[index] -= new float3(0f, 0f, MoveSpeeds[index] * DeltaTime);
    }
}
