using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject 
{
    public int life = 50;
    public int damage = 5;
    public float speed = 5.0f;
    public float radiusDetection = 3.0f;
}
