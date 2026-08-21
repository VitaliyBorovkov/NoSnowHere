using System;

using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxSnowAmount = 5;

    public int SnowAmount { get; private set; }
    public int MaxSnowAmount => maxSnowAmount;
    public bool IsFull => SnowAmount >= maxSnowAmount;

    public event Action<int, int> OnSnowChanged;

    public bool TryAddSnow(int amount = 1)
    {
        if (IsFull)
        {
            Debug.Log("Inventory: Hands are full.");
            return false;
        }

        SnowAmount = Mathf.Min(SnowAmount + amount, maxSnowAmount);
        OnSnowChanged?.Invoke(SnowAmount, maxSnowAmount);
        Debug.Log($"Inventory: Snow {SnowAmount}/{maxSnowAmount}");
        return true;
    }

    public bool TryRemoveSnow(int amount = 1)
    {
        if (SnowAmount <= 0)
        {
            return false;
        }

        SnowAmount = Mathf.Max(SnowAmount - amount, 0);
        OnSnowChanged?.Invoke(SnowAmount, maxSnowAmount);
        return true;
    }

    public void Clear()
    {
        SnowAmount = 0;
        OnSnowChanged?.Invoke(SnowAmount, maxSnowAmount);
    }
}
