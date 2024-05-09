using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class CreateEnemy : ScriptableObject
{
        public string enemyName ="EnemyName";
        public bool isBoss = false;
        public float MaxHealth = 4;
        public float chaseRadius = 21f;
        public float attackRadius = 8f;
        public float prepareToAttack = 0.2f;
        public float attackCooldown = 2f;
        public float speed = 500f;
        public float gold = 0;
}



