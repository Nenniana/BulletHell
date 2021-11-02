using UnityEngine;
using System;
using UnityEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectiles/Projectile")]
public class ProjectileObject : ScriptableObject
{
    [Header("General Settings")]
    public Sprite projectileSprite;
    public GameObject explosionPrefab;
}