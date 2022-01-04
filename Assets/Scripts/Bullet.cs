using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static List<Bullet> AllBullets = new List<Bullet>();
    int damage;
    float velocity;
    Enemy target;

    void Awake() => AllBullets.Add(this);
    void OnDestroy() => AllBullets.Remove(this);

    public void Setup(int damage, float velocity)
    {
        this.damage = damage;
        this.velocity = velocity;
        if(Enemy.AllEnemies.Count > 0)
            target = Enemy.AllEnemies[Enemy.AllEnemies.Count-1];
    }

    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // Follow the target.. duh.
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime * 10f);
        if(target.transform.position != transform.position)
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.down);
        if(transform.position == target.transform.position)
        {
            target.GetDamage(damage);
            Destroy(gameObject);
        }
    }
}