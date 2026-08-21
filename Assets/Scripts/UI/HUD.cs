using TMPro;

using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI snowText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private PlayerInventory inventory;
    private PlayerWallet wallet;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");

        inventory = player.GetComponent<PlayerInventory>();
        wallet = player.GetComponent<PlayerWallet>();

        inventory.OnSnowChanged += UpdateSnow;
        wallet.OnMoneyChanged += UpdateMoney;

        UpdateSnow(inventory.SnowAmount, inventory.MaxSnowAmount);
        UpdateMoney(wallet.Money);
    }

    private void OnDestroy()
    {
        if (inventory != null) inventory.OnSnowChanged -= UpdateSnow;
        if (wallet != null) wallet.OnMoneyChanged -= UpdateMoney;
    }

    private void UpdateSnow(int current, int max)
    {
        if (snowText != null)
            snowText.text = $"Snow: {current}/{max}";
    }

    private void UpdateMoney(int money)
    {
        if (moneyText != null)
            moneyText.text = $"${money}";
    }
}
