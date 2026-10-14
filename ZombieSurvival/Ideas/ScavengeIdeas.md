==================================================
8. SCAVENGING
==================================================

Example:
POST /api/scavenge

The server determines:
1. Current city and area
2. Whether it is Day
3. Whether this area was already scavenged today (if yes -> reject; try again after dawn)
4. Whether the player has enough stamina for this area's ScavengeStaminaCost
5. What loot/event occurs (weighted by AreaType)
6. What resources are consumed (stamina; later hunger/thirst)
7. Mark area as scavenged for this Day
8. What gets persisted
9. Whether night should begin (stamina exhausted)

Same-area rule:
- Cannot scavenge the same area twice in one Day
- After dawn, that area is available again (reloot across different nights/days is allowed)

Loot focus by area type (v1):
- GroceryStore -> food / water
- GunStore -> weapons / ammo
- GasStation -> fuel (refill empty tank) AND common car parts (repair wear and tear)
- Other areas may rarely drop parts; gas stations are the reliable source
- Church -> small food / lower danger / light supplies

Possible events:
- Nothing useful
- Food / water
- Weapon / ammo
- Car part / fuel
- Small medical find
- Ambush (optional later)

The game logic belongs primarily in services/domain logic, not controllers.

- Ideas to branch off of from above. 
- 