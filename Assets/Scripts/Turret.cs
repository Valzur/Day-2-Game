using UnityEngine;

[System.Serializable]
public struct TurretProperties
{
    public string name;
    public int price;
    public int damage;
    public float fireRate;
    public float bulletVelocity;
}

public class Turret : TileObject
{
    public TurretProperties turretProperties;
}
