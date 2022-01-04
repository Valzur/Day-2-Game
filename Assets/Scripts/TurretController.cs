using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : Turret
{
    [SerializeField] Bullet bulletPrefab;
    float timeLeftTillShoot = 0;

    void FixedUpdate()
    {
        Shoot();
    }

    void Shoot()
    {
        timeLeftTillShoot -= Time.fixedDeltaTime;
        if(timeLeftTillShoot <= 0 && Enemy.AllEnemies.Count > 0)
        {
            timeLeftTillShoot = 1 / turretProperties.fireRate;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Setup(turretProperties.damage, turretProperties.bulletVelocity);
        }
    }
}
