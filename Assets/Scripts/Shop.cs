using System.Collections.Generic;
using UnityEngine;

public class Shop: MonoBehaviour
{
    [SerializeField] Transform shopParent;
    [SerializeField] ShopObjectHandler shopObjectPrefab;
    [SerializeField] List<Turret> turretsForSale = new List<Turret>();
    Turret selectedItem = null;
    public TileObject Buy(Tile tileBase)
    {
        TileObject turret = null;
        if(selectedItem)
        {
            GameManager.Instance.LoseHexes(selectedItem.turretProperties.price);
            Tile.BuyMode = false;
            Vector3 spawnPos = tileBase.transform.position;
            spawnPos.z = - 0.5f;
            turret = Instantiate(selectedItem, spawnPos, Quaternion.identity, tileBase.transform);
        }
        return turret;
    }

    public void ActivateBuyMode(int itemNo)
    {
        selectedItem = turretsForSale[itemNo];
        Tile.BuyMode = true;
    }

    void OnEnable()
    {
        ShopObjectHandler [] shopItems = shopParent.GetComponents<ShopObjectHandler>();
        foreach (var item in shopItems)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < turretsForSale.Count; i++)
        {
            Instantiate(shopObjectPrefab, shopParent).Setup(turretsForSale[i].turretProperties, i);
        }
    }
}