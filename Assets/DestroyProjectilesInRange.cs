using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectilesInRange : MonoBehaviour
{
    public float explosionRadius;
    void ExplosionDamage(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<ProjectileControl>().ExplodeDisable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //print("O key was pressed");
            ExplosionDamage(this.gameObject.transform.position, explosionRadius);
        }
    }
}
