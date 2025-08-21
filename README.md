<div style="text-align: center;">

![logo](Documentation~/Assets/Logo.png)

Simple, fast and modular DI framework for Unity.  
Minimum code - maximum control over dependencies.

[![License: MIT](https://img.shields.io/badge/License-MIT-indigo.svg)](https://opensource.org/licenses/MIT)
[![Releases](https://img.shields.io/github/release/zerobject/laboost.svg)](https://github.com/zerobject/laboost/releases)

</div>

---

> ### 📢Important Note
> Documentation is available in multiple languages.
> #### English | [Русский](Documentation~/README_ru.md)

# 📜Table of Contents

- ### [🔎 Review](#review)
- ### [🚀 Features](#features)
- ### [📦 Installation](#installation)
- ### [🖥️ Quick Start](#quick-start)
- ### [📚 Examples](#examples)
- ### [⚖️ Comparison with other solutions](#comparison-with-other-solutions)
- ### [🪙 Support](#support)
- ### [🤝 Contribution](#contribution)

# 🔎Review

**Laboost** is a lightweight and performant Dependency Injection (DI) framework for Unity,
inspired by [Zenject](https://github.com/modesttree/Zenject)
and [Extenject](https://github.com/Mathijs-Bakker/Extenject), but with a focus on:

- a simplified API without unnecessary magic;
- high speed and low allocations;
- minimal dependence on `MonoBehaviour`;
- a modular architecture that is easy to extend.

The framework implements IoC principles and automatic dependency injection by attributes or through configuration,
removing manual binding of classes and facilitating testing.

# 🚀Features

- 📦 Easy installation via `Git URL` or `Unity Package`.
- ⚡ High performance and minimal allocations.
- 🎯 Attribute injection for fields, properties and constructors.
- 🛠️ `MonoBehaviour` and `ScriptableObject` support.
- 🔁 Lifecycles: `Singleton`, `Transient`, `Scoped`.
- 🧩 Flexible container configuration.
- 🧪 Integration with unit tests.

# 📦Installation

- ### Via `Git URL` (recommended)

1. Copy: https://github.com/zerobject/laboost.git
2. Open Unity Package Manager (`Window -> Package Manager`) `+` → Install package from Git URL...
3. Paste the link and click `Add`.

- ### Manual installation

1. Download `.unitypackage` from [releases](https://github.com/zerobject/laboost/releases).
2. In Unity: `Assets -> Import Package -> Custom Package...`
3. Select the file and import.

# 🖥️Quick Start

### 1. Define dependencies

```C#
public class PlayerService
{
    public void Attack() => Debug.Log("Игрок атакует!");
}

public class EnemyAI
{
    [Inject] 
    private PlayerService m_PlayerService;

    private void Update()
    {
        if (shouldAttack)
            m_PlayerService.Attack();
    }
}
```

### 2. Setting up the container

```C#
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerService>().AsSingle();
        Container.Bind<EnemyAI>().AsTransient();
    }
}
```

# 📚Examples

### Container Registration

```C#
Container.Bind<TestClassA>().AsSingle();
Container.Bind<TestClassB>().AsTransient();
```

### Implementation in MonoBehaviour

```C#
public class Player : MonoBehaviour
{
    [Inject]
    private IWeapon _weapon;

    private void Start()
    {
        _weapon.Fire();
    }

}
```

# ⚖️Comparison with other solutions

| Feature              | Laboost  |  Zenject  | Extenject |
|----------------------|:--------:|:---------:|:---------:|
| API simplicity       |    ✅     |    ⚠️     |    🟡     |
| Performance          |    ✅     |    🟡     |    🟡     |
| Lifecycle support    |    ✅     |     ✅     |     ✅     |
| Minimum dependencies |    ✅     |     ❌     |     ❌     |

# 🪙Support

If you found this project useful:

- Put a ⭐ on GitHub
- Report a bug in [Issues](https://github.com/zerobject/laboost/issues)
- Suggest improvements in Discussions

# 🤝Contribution

- Fork the repository
- Create a branch `feature/name`
- Make changes and add tests
- Open a `Pull Request`.