using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] Color normalTileColor;
    [SerializeField] Color buyModeColor;
    protected static List<Tile> AllTiles = new List<Tile>();
    protected static bool buyMode = false;
    public static bool BuyMode
    {
        get{return buyMode;}
        set{buyMode = value; SwitchAllColors();}
    }
    public bool isPath = false;
    TileObject objectUsed;
    MeshRenderer renderer;
    Material matCopy;
    void OnEnable() => AllTiles.Add(this);
    void OnDestroy() => AllTiles.Remove(this);
    void Awake() 
    {
        if(isPath)
            return;
        renderer = GetComponent<MeshRenderer>();
        matCopy = new Material(renderer.material);
        renderer.material = matCopy;
    }
    
    void OnMouseDown()
    {
        if(isPath)
            return;
        if(buyMode && !objectUsed)
        {
            objectUsed = GameManager.Instance.Buy(this);
            buyMode = false;
            matCopy.color = buyModeColor;
        }
    }

    protected static void SwitchAllColors()
    {
        foreach (var tile in AllTiles)
            tile.SwitchTileColor();
    }

    public void SwitchTileColor()
    {
        if(objectUsed || isPath)
            return;
        
        //Switch here.
        matCopy.color = BuyMode? buyModeColor : normalTileColor;
    }
}