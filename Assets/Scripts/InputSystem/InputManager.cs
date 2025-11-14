using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private const string PlayerMapName = "PlayerActionMap";
    private const string UIMapName = "UIActionMap";
    private const string LOG = "InputManager";


    private PlayerInput playerInput;

    public InputAction Look => playerInput.actions["Look"];
    public InputAction Move => playerInput.actions["Move"];
    public InputAction Jump => playerInput.actions["Jump"];
    public InputAction Crouch => playerInput.actions["Crouch"];
    public InputAction Sprint => playerInput.actions["Sprint"];
    public InputAction Collect => playerInput.actions["Collect"];
    public InputAction Interact => playerInput.actions["Interact"];
    //public InputAction SwitchWeaponByScroll => playerInput.actions["SwitchWeaponByScroll"];
    //public InputAction WeaponSlot1 => playerInput.actions["WeaponSlot1"];
    //public InputAction WeaponSlot2 => playerInput.actions["WeaponSlot2"];
    //public InputAction WeaponSlot3 => playerInput.actions["WeaponSlot3"];
    public InputAction Pause => playerInput.actions["Pause"];

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void SetActionsEnabled(bool enable)
    {
        if (enable)
        {
            Move.Enable();
            Jump.Enable();
            Look.Enable();
            Sprint.Enable();
            Collect.Enable();
            Interact.Enable();
            Crouch.Enable();
            //SwitchWeaponByScroll.Enable();
            //WeaponSlot1.Enable();
            //WeaponSlot2.Enable();
            //WeaponSlot3.Enable();
        }
        else
        {
            Move.Enable();
            Jump.Enable();
            Look.Enable();
            Sprint.Enable();
            Collect.Enable();
            Interact.Enable();
            Crouch.Enable();
            //SwitchWeaponByScroll.Disable();
            //WeaponSlot1.Disable();
            //WeaponSlot2.Disable();
            //WeaponSlot3.Disable();
        }
    }

    public void SwitchToGameplayActionMap()
    {
        TrySwitchActionMap(PlayerMapName);
    }

    public void SwitchToUIActionMap()
    {
        TrySwitchActionMap(UIMapName);
    }

    private void TrySwitchActionMap(string mapName)
    {
        if (playerInput == null)
        {
            return;
        }

        if (playerInput.actions == null)
        {
            Debug.LogWarning($"{LOG}: PlayerInput.actions is null, cannot switch to '{mapName}'.");
            return;
        }

        var map = playerInput.actions.FindActionMap(mapName, throwIfNotFound: false);
        if (map == null)
        {
            Debug.LogWarning($"{LOG}: action map '{mapName}' not found in actions '{playerInput.actions.name}'. Available maps:");
            foreach (var m in playerInput.actions.actionMaps)
            {
                Debug.Log($"  - {m.name}");
            }
            return;
        }

        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log($"{LOG}: Switched to action map '{mapName}'.");
    }
}
