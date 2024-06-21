using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyState : Action
{
    /*
     *  ALARM,      //警戒状态，
     *  STAND,      //站立状态              
     *  WALK,       //移动状态
     *  CHASE,      //追击状态
     *  ATTACK,     //攻击状态
     *  HIT,        //被攻击状态
     */

    public SharedEnemyState resultEnemyState;

    public SharedInt ALARM_Weight;
    public SharedInt STAND_Weight;
    public SharedInt WALK_Weight;
    public SharedInt CHASE_Weight;
    public SharedInt ATTACK_Weight;
    public SharedInt HIT_Weight;

    public override void OnStart()
    {
        base.OnStart();
        List<int> array = new List<int>() { ALARM_Weight.Value, STAND_Weight.Value, WALK_Weight.Value, CHASE_Weight.Value, ATTACK_Weight.Value, HIT_Weight.Value };
        for (int i = 1; i < array.Count; i++) 
        {
            array[i] = array[i] + array[i - 1];
        }
        int randomResult = Random.Range(0, array[5]);
        if (randomResult >= 0 && randomResult < array[0])
            resultEnemyState.Value = EnemyState.ALARM;
        else if(randomResult>= array[0] && randomResult < array[1])
            resultEnemyState.Value = EnemyState.STAND;
        else if (randomResult >= array[1] && randomResult < array[2])
            resultEnemyState.Value = EnemyState.WALK;
        else if (randomResult >= array[2] && randomResult < array[3])
            resultEnemyState.Value = EnemyState.CHASE;
        else if (randomResult >= array[3] && randomResult < array[4])
            resultEnemyState.Value = EnemyState.ATTACK;
        else if (randomResult >= array[4] && randomResult < array[5])
            resultEnemyState.Value = EnemyState.BEHIT;
    }
    public override TaskStatus OnUpdate()
    {
        return base.OnUpdate();
    }
}
