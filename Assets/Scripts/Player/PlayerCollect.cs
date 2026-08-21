using System.Collections;

using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private const string LOG = "PlayerCollect";

    [SerializeField] private float collectRange = 2f;
    [SerializeField] private float collectTime = 1.5f;
    [SerializeField] private LayerMask collectableLayer;
    [SerializeField] private LayerMask obstacleMask = ~0;
    [SerializeField] private float checkInterval = 0.05f;
    [SerializeField] private string pickupTag = "Snow";

    private PlayerInventory inventory;

    private Coroutine collectCoroutine;
    private GameObject currentTarget;

    public bool IsCollecting { get; private set; }

    private void Awake()
    {
        var components = GetComponent<PlayerComponents>();
        inventory = components.Inventory;
    }

    public void StartCollect()
    {
        if (IsCollecting)
        {
            return;
        }

        if (inventory != null && inventory.IsFull)
        {
            Debug.Log($"{LOG}: Hands are full, go drop off the snow.");
            return;
        }

        var target = FindClosestCollectable();

        if (target == null)
        {
            Debug.Log($"{LOG}: No collectable object in range.");
            return;
        }

        currentTarget = target;
        collectCoroutine = StartCoroutine(CollectRoutine());
    }

    public void StopCollect()
    {
        if (!IsCollecting && collectCoroutine == null)
        {
            return;
        }

        if (collectCoroutine != null)
        {
            StopCoroutine(collectCoroutine);
            collectCoroutine = null;
        }

        IsCollecting = false;
        currentTarget = null;
        Debug.Log($"{LOG}: Collection stopped.");
    }

    private GameObject FindClosestCollectable()
    {
        var colliders = Physics.OverlapSphere(transform.position, collectRange, collectableLayer);

        GameObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (!string.IsNullOrEmpty(pickupTag) && !col.CompareTag(pickupTag))
            {
                continue;
            }

            if (!HasLineOfSight(col.gameObject))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = col.gameObject;
            }
        }

        return closest;
    }

    private bool HasLineOfSight(GameObject target)
    {
        int playerLayerMask = ~(1 << gameObject.layer);
        int mask = obstacleMask & playerLayerMask;

        Vector3 origin = transform.position;
        Vector3 direction = target.transform.position - origin;
        float distance = direction.magnitude;

        if (Physics.Raycast(origin, direction.normalized, out var hit, distance, mask, QueryTriggerInteraction.Ignore))
        {
            return hit.transform.gameObject == target || hit.transform.IsChildOf(target.transform);
        }

        return true;
    }

    private IEnumerator CollectRoutine()
    {
        IsCollecting = true;
        Debug.Log($"{LOG}: Collect started. Target = {currentTarget.name}");

        float timer = 0f;

        while (timer < collectTime)
        {
            if (currentTarget == null)
            {
                Debug.LogWarning($"{LOG}: Target was destroyed during collection.");
                break;
            }

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance > collectRange)
            {
                Debug.Log($"{LOG}: Player moved out of range.");
                break;
            }

            timer += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }

        if (currentTarget != null && timer >= collectTime)
        {
            Debug.Log($"{LOG}: Collected {currentTarget.name}");
            currentTarget.SetActive(false);
            inventory?.TryAddSnow(1);
        }

        IsCollecting = false;
        collectCoroutine = null;
        currentTarget = null;
    }
}
