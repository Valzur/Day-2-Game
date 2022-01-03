using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Utility : MonoBehaviour
{
    public static Vector3 GetVector3(Vector2Int vector2) => new Vector3(vector2.x, vector2.y, 0);
    public static bool RandomBool () => (Random.value >= 0.5);

}