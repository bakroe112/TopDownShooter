using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToPlayer", story: "[Target] Move To [Player]", category: "Action", id: "07f8b1c93693b63140f814f772a175ba")]
public partial class MoveToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    protected override Status OnStart()
    {
        Target.Value = GameObject.Find("Enemy").transform;
        Player.Value = GameObject.Find("Player");
        Target.Value.transform.position = Player.Value.transform.position;
        return Status.Running;
    }

}

