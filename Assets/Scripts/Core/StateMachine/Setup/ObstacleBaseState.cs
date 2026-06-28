using UnityEngine;

public abstract class ObstacleBaseState : BaseState<ObstacleController>
{
    protected ObstacleBaseState(ObstacleController _ctx) : base(_ctx)
    {
    }
}