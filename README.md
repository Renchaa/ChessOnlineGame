# Unity Chess online game using Photon

This is a multiplayer and single-player chess game built in Unity using **Photon PUN 2** for networking. The project features full local and online match support, animated piece movement, and a modular architecture for future expansion.

---

## 🔧 Tech Stack

- **Unity** (2022+)
- **Photon PUN 2** (Realtime multiplayer networking)
- **C# Job System** (for board logic optimizations)
- **Scriptable Input System** (customized handlers)
- **Modular Tweeners** (for smooth movement animations)

---

## ✨ Features

-  Real-time multiplayer via Photon
-  Board interaction system with hover, select, and move events
-  Full set of chess pieces with move logic: King, Queen, Rook, Bishop, Knight, Pawn
-  Input system handles UI clicks, 3D collider hits, and audio input events
-  Tween-based movement (linear, arc, instant)
-  Team color and piece type enums for clean board logic
-  Distinct scripts for single-player and multiplayer board behaviors
-  Chess UI Manager and GameInitializer for session control

---
Scripts/
- Contains all core game logic scripts.
- Subfolders:
-  Enums/ – Stores enums like PieceType and TeamColor used for game rules and visuals.
-  InputSystem/ – Modular handlers for input sources: UI clicks, collider raycasts, and audio input.
-  Pieces/ – Each chess piece (Pawn, Rook, King, etc.) has its own script defining movement logic.

- Main logic scripts in root:
  • Board.cs – Maintains board state and data.
  • ChessGameController.cs – Manages game flow.
  • MultiplayerBoard.cs / SingleplayerBoard.cs – Separate handling for online and offline play.
  • ChessPlayer.cs – Stores player-side data.
  • NetworkManager.cs – Manages Photon connection and matchmaking.
  • PieceCreator.cs – Handles piece spawning and setup.

Photon/
- Holds networking setup for Photon PUN 2.
- Prefabs/ – Likely contains network-ready prefabs.
- Resources/ – Includes PhotonServerSettings.asset for configuration.
