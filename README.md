## project-two-dot-like

## Overview
- `TileController` – Base class for all tiles; handles color, position, trigger flow, and destruction lifecycle.
- `TileVisual` – Manages visual appearance and destruction animations (uses DOTween).
- `GridController` – Creates and manages the grid, tile positions, collapse logic, and tile generation.
- `ConnectorController` – Handles user drag input, path validation, and match resolution.
- `BombTileController` – Special tile triggered on click to clear a 3x3 area.
- `RainbowTileController` – Special tile triggered via drag chain to clear all matching-colored tiles.
- `TileEffectExecutor` – Executes logic for area and color clears, abstracted from tile logic.
- `TileColorCycler` – Debug script to right-click cycle a tile’s color.
- `ColorId` – Enum defining color types and identifiers for game logic.

---

## Design Choices & Assumptions
- While working on this, I want to make sure that I make it as clean coded and as modular as possible under the time limitation. I want to try to make sure I can deliver it as much as according to the SOLID Principle, while not overthinking and overengineering it.
- The Tile system is mostly event base, with clear separation between visual and logic layers. I'm also considering to avoid circular depedency. That's why I don't use Singleton (which tend to eventually make circular depedency), while also use a couple of interface to inject depedency
- I'm using BombTileController and RainbowTileController that extend from the base TileController. To execute their effect itself, I use the TileEffectExecutor. This class for now use for containing all the special tile logic, but I think in the future when the tile mechanic became more complicated, I can easily replace it with some kind of interface or subclasses.
- When trying to generatae the special tile, especially the bomb tile, overlapping very often happened initially. A reservation system is used in `GridController` to prevent collapse/refill from overwriting newly spawned special tiles.
- I believe that the current system is built to be easily extendable with new tile types or gameplay modifiers. There are definitely a lot of room to improve, but I think it's enough for me for now under the time constraint (and to make sure that im not overthinking and overengineer it).

---

## Completed Features

- [x] 7x7 grid of dots
- [x] 5 different colored dots
- [x] Drag-to-connect mechanic (horizontal/vertical only)
- [x] Prevents repeated tile selection in a path
- [x] Match 3+ to clear tiles
- [x] Destruction animation
- [x] Collapse and refill system with top-down tile shifting
- [x] Right-click color cycling (for debugging)
- [x] Bomb tile (6+ match) that clears 3x3 on click

---

## Bonus Items Implemented

- **Bomb tile (6+)** – Spawns at the end of a chain and clears a 3x3 area on click.
- **Rainbow tile (9+)** – Spawns at the end of a chain and clears all same-colored tiles.
