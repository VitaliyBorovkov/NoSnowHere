# NoSnowHere — Claude Instructions

## Game
Snow collection idle/progression game on Unity (URP).
Player collects snow by hand → brings to drop-off point → gets money → buys upgrades.
Progression: hands → gloves → shovel (flat) → shovel (scoop) → snow blower.

## Critical Rules

### No runtime GetComponent
All component references must be cached in Awake via PlayerComponents hub.
Never call GetComponent in Update, FixedUpdate, LateUpdate, or any gameplay method.

### Execution Order
- -100: InputManager
- -50: PlayerComponents (hub — caches all player refs)
- 0: everything else

### Adding new player components
1. Add the component class
2. Add a property to PlayerComponents and cache it in Awake
3. Add the component to the Player prefab (Assets/Prefubs/Player.prefab)

### Prefab folder
The prefab folder is named "Prefubs" (typo) — keep it as is, don't rename.

### URP
Project uses URP. Never use Built-in render pipeline materials or shaders.
Use existing project materials or create new URP-compatible ones.

### Events for UI
PlayerInventory.OnSnowChanged(int current, int max)
PlayerWallet.OnMoneyChanged(int money)
Always use these events to update UI — never poll in Update.

## Layer Setup
- Layer 3: Player
- Layer 6: Ground
- Layer 7: Environment
- Layer 8: Snow (LayerMask m_Bits: 256)

## Key Files
- Player prefab: Assets/Prefubs/Player.prefab
- Drop-off prefab: Assets/Prefubs/DropOffPoint.prefab
- Scene: Assets/Scenes/GameScene.unity
- Full script map and session history: .claude/projects/.../memory/MEMORY.md
