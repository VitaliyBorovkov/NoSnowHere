using System;

using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;

    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
        Debug.Log($"Wallet: ${Money}");
    }

    public bool TrySpend(int amount)
    {
        if (Money < amount)
        {
            return false;
        }

        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }
}
