# quantum-kcc-movement
Deterministic character movement for Photon Quantum using Kinematic Character Controller (KCC)

A lightweight, deterministic character movement system built for Photon Quantum 3.0+. Perfect for 2D/3D games requiring precise collision response and multiplayer-ready physics.

[![Quantum 3.0+](https://img.shields.io/badge/Photon_Quantum-3.0%2B-blue)](https://www.photonengine.com/quantum) 
[![KCC 3.0+](https://img.shields.io/badge/KCC-3.0%2B-blue)](https://www.photonengine.com/quantum) 
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Unity 2021.3+](https://img.shields.io/badge/Unity-2021.3%2B-black)](https://unity.com)

---

## Key Features 🔑  
✅ **Deterministic Movement** - Frame-perfect sync in multiplayer  
✅ **Collision Handling** - Penetration correction & surface sliding  
✅ **Configurable Parameters** - Tweak via ScriptableObjects  
✅ **Rotation Support** - Optional directional facing  
✅ **Debug Visualizations** - See hitboxes and movement vectors  

---

## Installation ⚙️  
### Requires:
2. **Requires:** Photon Quantum 3.0+ and KCC add-on 3.0+

### Manual Setup:
1. Copy these folders to your project:  
   - Download and import Package  'LuisPagolaMovementKCC.unitypackage'

---

## Basic Usage ▶️  
1. Create a `KCCMovementSettings` asset:  
   *Right-click > Create > Quantum > KCCMovement*
   *Or use the imported one*

2. Add to your entity prototype:  

3. Use it:  
    var movement = f.Unsafe.GetPointer<KCCMovement>(filter.Entity); //Creo que puedo ponerlo en el OnPLayerADD
    var settings = f.FindAsset<KCCMovementSettings>(movement->Settings.Id);
    KCCMovementSystem.Move(f, filter.Entity, direction);


## About the Author

- **LinkedIn**: [Luis Pagola](https://www.linkedin.com/in/luis-adri%C3%A1n-pagola-guill%C3%A9n/)
- **Twitter**: [@LuigiPagola](https://x.com/EternalLoopSt)
- **Website**: [www.luispagola.dev](https://luispagola.dev/)

Feel free to reach out or connect if you're interested in my work!