using UnityEngine;

public interface Ipool
{
    bool active { get; }
    void Initialize();
    void SpawnObj(Projectile projectile, Vector3 position);
}
