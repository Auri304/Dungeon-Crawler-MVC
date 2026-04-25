# Dungeon Crawler MVC 🗡️🏰

A text-based (or tile-based) dungeon crawler built in **C#** using the **Model-View-Controller (MVC)** architectural pattern. This project was created as a structured exercise in separating game logic, rendering, and input handling into clean, maintainable layers.

---

## 🎮 About

Navigate through procedurally structured dungeon rooms, battle enemies, and survive as long as possible. The MVC architecture keeps the game's data model independent of how it's displayed — making it easy to extend, test, or swap out the view layer entirely.

---

## 🏗️ Architecture

This project strictly follows the **MVC** pattern:

| Layer | Responsibility |
|-------|---------------|
| **Model** | Game state — player stats, dungeon map, enemy data, inventory |
| **View** | Rendering — console output / UI representation of the current state |
| **Controller** | Input & logic — processes player input and updates the model accordingly |

This separation ensures that changes to the display don't break game logic, and vice versa.

---

## ⚔️ Features

- Dungeon room traversal and exploration
- Player character with stats (health, attack, etc.)
- Enemy encounters and combat resolution
- Clean MVC separation for maintainability and extensibility
- Built in C# with object-oriented design principles

---

## 🛠️ Built With

- **Language:** C#
- **Pattern:** Model-View-Controller (MVC)
- **Platform:** .NET / Windows

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Auri304/Dungeon-Crawler-MVC.git
   ```
2. Open the solution in **Visual Studio** (`.sln` file inside `DungeonCrawlerV9/`).
3. Build and run the project.

---

## 📁 Project Structure

```
Dungeon-Crawler-MVC/
└── DungeonCrawlerV9/       # Main project folder
    ├── Models/             # Game data and state
    ├── Views/              # Output / rendering logic
    ├── Controllers/        # Input handling and game loop
    └── ...
```

---

## 👩💻 Contributors

- [@Auri304](https://github.com/Auri304)

---

## 📄 License

This project is for educational and portfolio purposes.
