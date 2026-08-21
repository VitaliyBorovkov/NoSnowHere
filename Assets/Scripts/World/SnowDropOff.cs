using UnityEngine;

public class SnowDropOff : MonoBehaviour
{
    private const string LOG = "SnowDropOff";

    [SerializeField] private int moneyPerSnow = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var inventory = other.GetComponent<PlayerInventory>();
        var wallet = other.GetComponent<PlayerWallet>();

        if (inventory == null || wallet == null)
        {
            return;
        }

        if (inventory.SnowAmount <= 0)
        {
            Debug.Log($"{LOG}: Player has no snow to drop off.");
            return;
        }

        int earned = inventory.SnowAmount * moneyPerSnow;
        inventory.Clear();
        wallet.AddMoney(earned);

        Debug.Log($"{LOG}: Dropped off snow. Earned ${earned}. Total: ${wallet.Money}");
    }
}
