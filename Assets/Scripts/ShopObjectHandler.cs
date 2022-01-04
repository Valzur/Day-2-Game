using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopObjectHandler : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text fireRateText;
    [SerializeField] TMP_Text bulletVelocityText;
    [SerializeField] Button buyButton;
    int itemNo;
    public void Setup(TurretProperties turretProperties, int itemNo)
    {
        nameText.text = turretProperties.name;
        priceText.text = "Price: " + turretProperties.price;
        damageText.text = "Damage: " + turretProperties.damage;
        fireRateText.text = "Fire Rate: " + turretProperties.fireRate;
        bulletVelocityText.text = "Bullet Velocity: " + turretProperties.bulletVelocity;
        this.itemNo = itemNo;
        buyButton.onClick.AddListener(BuyItemCallBack);
    }

    public void BuyItemCallBack()
    {
        GameManager.Instance.ActivateBuyMode(itemNo);
    }

}