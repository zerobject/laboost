<div style="text-align: center;">

![Logo](Logo.png)

Простой, быстрый и мощный DI-фреймворк для Unity.

[![License: MIT](https://img.shields.io/badge/License-MIT-indigo.svg)](https://opensource.org/licenses/MIT)
[![Releases](https://img.shields.io/github/release/zerobject/laboost.svg)](https://github.com/zerobject/laboost/releases)

</div>

---

[English](README_en.md) | Русский

# 📜Содержание

- [Обзор](#обзор)
- [Установка](#установка)
- [Начало работы](#начало-работы)
- [Поддержка](#поддержка)

# 🔍Обзор

# 📦Установка

- ### Через GitHub URL (рекомендуется):

    - Скопируйте `https://github.com/zerobject/laboost.git`.
    - Откройте **Package Manager** (`Window / Package Manager`).
    - Нажмите на `+` в верхнем левом углу менеджера, затем выберите `Install a package from Git URL...`.
    - Вставьте скопированный адрес и нажмите `Add`.
    - Готово!

- ### Ручная установка:

    - Загрузите последнюю версию пакета (файлы формата `.unitypackage` вы можете найти
      в [списке релизов](https://github.com/zerobject/laboost/releases)).
    - В редакторе Unity щелкните ПКМ и выберите `Import Custom Package...`.
    - Выберите скачанный пакет. Далее вам предложат выбрать файлы пакета для установки.
      Выберите необходимое и завершите процесс установки.
    - Готово!

# 🖥️Начало работы

## Базовое использование

1. Создайте класс с зависимостью:
```c#
public class PlayerService
{
public void Attack() => Debug.Log("Игрок атакует!");
}

public class EnemyAI
{
[Inject] // <- важно!
private PlayerService m_PlayerService;

   private void Update()
   {
       if (shouldAttack)
           m_PlayerService.Attack();
   }
}
```

2. Настройте контейнер зависимостей:  

```c#
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerService>().AsSingle();
        Container.Bind<EnemyAI>().AsTransient();
    }
}
```

# 🪙Поддержка


