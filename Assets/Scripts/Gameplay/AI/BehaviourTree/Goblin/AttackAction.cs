using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "Attack", category: "Action", id: "a364cd9d74f30a15c21f5d18b5eb5d9a")]
public partial class AttackAction : Action
{
    protected override Status OnStart()
    {
        var enemyAttack = GameObject.GetComponent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.StartAttack();
            return Status.Success;
        }
        return Status.Failure;
    }

}

