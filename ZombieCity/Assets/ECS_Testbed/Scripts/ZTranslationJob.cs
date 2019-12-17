using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class ZTranslationJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> MoveSpeeds;
    public NativeArray<float3> Positions;

    public float DeltaTime;

    private bool _minusDirection;

    public ZTranslationJob(bool minusDirection = false)
    {
        _minusDirection = minusDirection;
    }

    public void Execute(int index)
    {
        Positions[index] += (_minusDirection ? -1 : 1) * new float3(0f, 0f, MoveSpeeds[index] * DeltaTime);
    }
}
