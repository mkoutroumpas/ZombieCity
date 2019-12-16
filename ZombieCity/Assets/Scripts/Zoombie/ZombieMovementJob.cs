using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct ZombieMovementJob : IJobParallelFor
{
    public NativeList<float> MoveSpeeds;
    public NativeList<float3> Positions;

    [ReadOnly]
    public float DeltaTime;

    public void Execute(int index)
    {
        Positions[index] += new float3(Positions[index].x, Positions[index].y, Positions[index].z - MoveSpeeds[index] * DeltaTime);
    }
}
