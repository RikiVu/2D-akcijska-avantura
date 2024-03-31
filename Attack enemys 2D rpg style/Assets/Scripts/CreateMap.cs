using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
[CreateAssetMenu(fileName = "New MapSpawn", menuName = "SpawnInfo")]

public class CreateMap : ScriptableObject
{
    public GameObject Enemy;
    new public int MaxCount = 0;
    new public float RespawnTime = 0;
}




