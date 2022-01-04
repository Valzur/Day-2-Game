using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Enemy
{
    int nextWayPoint = 1;
    Vector2Int [] path;

    public void Setup(Vector2Int[] path)
    {
        base.Setup();
        this.path = path;
        transform.rotation = Quaternion.LookRotation(Utility.GetVector3(path[1] - path[0]), Vector3.back);
    }

    void FixedUpdate()
    {
        FollowPath();
    }

    void FollowPath()
    {
        Vector3 target = Utility.GetVector3(path[nextWayPoint]);
        target.z = (float)-0.5;
        transform.position = Vector3.MoveTowards(transform.position, target, velocity * Time.fixedDeltaTime);
        Vector3 noOffsetPos = transform.position;
        noOffsetPos.z = 0;
        if(noOffsetPos == Utility.GetVector3(path[nextWayPoint]))
        {
            nextWayPoint++;
            print(nextWayPoint + "/" + path.Length);
            if(nextWayPoint >= path.Length)
            {
                print("Attempted to Destroy");
                GameManager.Instance.LoseHP(this);
                Destroy(gameObject);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Utility.GetVector3(path[nextWayPoint] - path[nextWayPoint-1]), Vector3.back);
            }
        }
    }
}
