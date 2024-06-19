using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
[CreateAssetMenu(fileName = "New MapSpawn", menuName = "SpawnInfo")]

public class CreateMap : ScriptableObject
{
    public GameObject[] Enemies;
    public GameObject[] EnemiesMedium;
    new public int MaxCount = 0;
    new public float RespawnTime = 0;
    new public string mapName ="";
    new public Vector2 maxPosition;
    new public Vector2 minPosition;
    new public AudioClip songToPlay;
}




