using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class CreateEnemy : ScriptableObject
{
        public string enemyName ="EnemyName";
        public float MaxHealth = 4;
        public float chaseRadius = 21f;
        public float attackRadius = 8f;
}



