using System;
using Unity.Behavior;
using UnityEngine;
using Pathfinding;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Run Away",
    story: "[Target] runs away from [Player] using [Speed] and [MassiveSpeed] and [TargetDestinationDistance] and [RunAwayDistance] and [MaxAllowedPlayerDistance]",
    category: "Action/AI",
    id: "3bbf3b18-13ff-4c57-b9a8-b7c671234567")]   
public partial class RunAwayAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> MassiveSpeed;
    [SerializeReference] public BlackboardVariable<float> TargetDestinationDistance;
    [SerializeReference] public BlackboardVariable<float> RunAwayDistance;

    // thay cho BehaviorTree.GetVariable("maxAllowedPlayerDistance")
    [SerializeReference] public BlackboardVariable<float> MaxAllowedPlayerDistance;

    private Vector3 _direction;
    private Vector3 _runAwayDirection;
    private Vector3 _runAwayPosition;
    private bool _isReverseOver;

    protected override Status OnStart()
    {
        //Debug.Log($"RunAway OnStart frame={Time.frameCount}");
        var self = GameObject;
        _direction = Player.Value.transform.position - self.transform.position;
        _runAwayDirection = -_direction.normalized;
        _runAwayPosition = self.transform.position + _runAwayDirection * RunAwayDistance.Value;
        Target.Value = GameObject.Find("Goblin").transform;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var self = GameObject;
        var origin = self.transform.position;
        float rayDistance = RunAwayDistance.Value + 1f;

        RaycastHit2D wallCheck = Physics2D.Raycast(
            origin,
            _runAwayDirection,
            rayDistance,
            1 << 3
        );
        //Debug.Log($"_direction = {_direction}");
        //Debug.Log($"Player.Value.transform.position = {Player.Value.transform.position}");
        //Debug.Log($"self.transform.position = {self.transform.position}");


        Debug.DrawRay(origin, _runAwayDirection * rayDistance, Color.red);

        //Debug.Log($"Target = {(Target.Value ? Target.Value.name : "NULL")} | id={(Target.Value ? Target.Value.GetInstanceID() : 0)}");
        //Debug.Log($"Self   = {self.name} | id={self.transform.GetInstanceID()}");
        //Debug.Log($"TargetIsSelf = {Target.Value == self.transform}");


        var aiLerp = self.GetComponent<AILerp>();

        if (wallCheck.collider != null || _isReverseOver)
        {
            float extra = RunAwayDistance.Value + MaxAllowedPlayerDistance.Value;

            _runAwayPosition = Player.Value.transform.position
                               - _runAwayDirection * extra;

            Target.Value.position = _runAwayPosition;
                aiLerp.speed = MassiveSpeed.Value;

            _isReverseOver = true;
        }
        else
        {
            Target.Value.position = _runAwayPosition;
                aiLerp.speed = Speed.Value;
        }

        if (Vector3.Distance(self.transform.position, _runAwayPosition)
            < TargetDestinationDistance.Value)
        {
            _isReverseOver = false;
            return Status.Success;
        }

        return Status.Running;
    }

}
