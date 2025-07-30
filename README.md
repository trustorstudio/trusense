# 🎯 Trusense – UI/UX Toolkit for Unity

**UI with feeling. UX with sense.**  
Developed & maintained by **Trustor Studio**

---

## 📖 Introduction

**Trusense** is a modern, modular UI/UX framework for Unity — thoughtfully crafted by **Trustor Studio** to streamline the creation of high-quality, scalable user interfaces in games and interactive experiences.

Trusense focuses on:

- Emotional connection between UI and player (UX-first mindset)
- Clean architecture (separation of logic, visuals, and data)
- Developer productivity (easy integration and customization)
- Visual elegance (themeable, animated, responsive)

> Whether you're building a mobile RPG, a console shooter, or a casual puzzle game — Trusense empowers your UI to be both **functional and delightful**.

---

## 🧩 Why Trusense?

Unity's default UI tools are powerful but often unstructured. Trusense offers:

- A clean, extensible architecture built on ScriptableObjects, events, and patterns like MVC & SOA.
- Centralized UI routing & navigation system.
- Modular components that can be composed like LEGO.
- Fully themeable interface with live theme switching.
- DOTween-based animation layer (with fallback).
- Unity Editor tooling for faster workflow.

Designed for **production-scale** projects.

---

## ✨ Feature Overview

### 🔹 Core Features

| Feature                   | Description                                                                |
| ------------------------- | -------------------------------------------------------------------------- |
| 🧱 Modular UI Elements    | Build menus, popups, and HUDs using reusable, composable elements.         |
| ⚙️ UI Router & Navigation | Stack-based routing system with back-navigation, push/pop & modal support. |
| 🎨 Dynamic Theme System   | Easily change fonts, colors, sprites globally via ScriptableObjects.       |
| 📐 Responsive Design      | Supports screen ratios, safe areas, auto-layouts for mobile & desktop.     |
| 🎞 Animated Transitions    | Built-in support for DOTween & Unity Animator. Smooth UI flows.            |
| 🔧 Editor Tooling         | Visual preview, layout helpers, and in-editor UI testing.                  |
| 📦 Samples Included       | Ready-to-use screens: main menu, settings, inventory, HUD, popups, etc.    |

---

## 🏗 Architecture

Trusense is built on a **clean, decoupled architecture** designed to scale.

### 🧠 Core Layers

```
[ UIManager ]
      |
[ UIRouter ] <-- navigation & history
      |
[ UIElements ] (Panels, Popups, Windows)
      |
[ Theme System ] -- styles, fonts, colors
      |
[ Transition Layer ] -- DOTween/Animator
```

### 🔄 UIElement Lifecycle

Each `UIElement` supports:

- `Initialize()`
- `OnShow()`
- `OnHide()`
- `PlayShowAnimation()`
- `PlayHideAnimation()`

You can override these for custom behaviors.

---

## 📂 Directory Structure

```
Trusense/
├── Core/                   # Base classes, managers, lifecycle
├── Components/             # Buttons, panels, windows, modals, HUDs
├── Theme/                  # TrusenseTheme SO, colors, fonts, UI skins
├── Animations/             # DOTween-based transitions
├── Router/                 # Navigation manager and screen stack
├── Editor/                 # Custom inspectors, layout tools
├── Samples/                # Example scenes, demo UI
└── Documentation/          # Guides and usage references
```

---

## ⚡ Getting Started

### 1. Installation

- **Manual**: Drag `Trusense/` into `Assets/`.
- **UPM (coming soon)**:
  ```json
  "com.trustor.trusense": "https://github.com/trustorstudio/trusense.git"
  ```

### 2. Initialize Theme

```csharp
UIThemeManager.Instance.SetTheme(myTrusenseTheme);
```

### 3. Show a Popup

```csharp
TrusenseUI.Show("SettingsPopup");
```

### 4. Navigate Between Screens

```csharp
UIRouter.NavigateTo("InventoryScreen");
UIRouter.Back();
```

---

## 🎞 Animation System

### 🔸 DOTween (default)

Attach `TrusenseAnimator` and define:

- Scale
- Fade
- Move transitions

### 🔸 Unity Animator

Fallback if DOTween is not available.

You can override animation behavior per element via:

```csharp
protected override void PlayShowAnimation() { ... }
```

---

## 🎨 Theme System

Trusense uses a ScriptableObject-based theme engine that supports:

- Font presets
- Color presets
- Spacing/margins
- Icon/sprite swaps
- Live switching at runtime

Example:

```csharp
myButton.SetColor(Theme.Color.Primary);
myLabel.SetFont(Theme.Font.Bold);
```

---

## 🧠 UX-Oriented Design

- Stack-based screen history for "back" logic
- Smooth transitions between contexts
- Prevents popup overlap, screen flicker
- Optional audio feedback on interaction
- Suitable for touch, controller, or mouse/keyboard

---

## 🧪 Requirements

| Tool / Library     | Version       | Required |
| ------------------ | ------------- | -------- |
| Unity              | 2021.3+ (LTS) | ✅       |
| TextMeshPro        | Built-in      | ✅       |
| DOTween (Pro/Free) | Recommended   | ⚠️       |
| Unity Input System | Optional      | ❌       |

---

## 📦 Use Cases

| Game Type       | Use Case                                  |
| --------------- | ----------------------------------------- |
| RPG             | Inventory, Quest UI, Character stats      |
| Mobile Puzzle   | Popup flow, onboarding, hints             |
| Strategy        | Mini-map overlays, build menus            |
| Action Shooter  | HUD, ammo bar, damage indicators          |
| Narrative Games | Dialogue UI, cutscene flows, choice trees |

---

## 🖼 Screenshots

_(Add visual examples of your UI components, HUDs, and transitions.)_

---

## 🔧 Extending Trusense

You can extend by:

- Creating new `UIElement` subclasses
- Creating new theme fields (e.g. spacing)
- Hooking into `UIRouter` navigation events
- Integrating audio/sound feedback per element

---

## 🛡 Best Practices

- Keep one `UIRouter` instance per UI Layer.
- Use `TrusenseTheme` for global consistency.
- Avoid hardcoding values — rely on theme + layout groups.
- Animate using DOTween for better control (vs Animator).
- Use `UIEvents` to communicate between UI and gameplay.

---

## 🔒 License

MIT License  
You are free to use, modify, and distribute in personal or commercial projects.

See [`LICENSE`](./LICENSE) for full details.

---

## 🙋 FAQ

**Q: Can I use Trusense with Unity's new UI Toolkit?**  
A: Currently no. Trusense is based on the UGUI system (`Canvas`, `RectTransform`, etc.).

**Q: Can I use custom animation engines?**  
A: Yes. You can override `PlayShowAnimation()` with any tweening logic.

**Q: Does Trusense support mobile input?**  
A: Yes. Fully compatible with touch, controller, and keyboard input.

---

## 🤝 Contributing

We welcome feedback, bug reports, and pull requests.

Coming soon:

- `CONTRIBUTING.md`
- GitHub Discussions

---

## 🌐 Links

- 🔗 [Trustor Studio Website](https://trustorstudio.com)
- 🐙 [GitHub](https://github.com/TrustorStudio/Trusense)
- 📄 Docs: Coming soon
- 🧠 Devlog: https://dev.trustorstudio.com/trusense

---

## ❤️ About Trustor Studio

**Trustor Studio** is a game technology team based in Vietnam, focusing on building reusable, production-quality tools for Unity developers. We believe great tools create great experiences — both for players and creators.

> “We design systems so players can feel, not just see.”
