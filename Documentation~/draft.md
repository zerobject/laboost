<div style="text-align: center;">

![Logo](Assets/Logo.png)

Простой, быстрый и мощный DI-фреймворк для Unity.

[![License: MIT](https://img.shields.io/badge/License-MIT-indigo.svg)](https://opensource.org/licenses/MIT)
[![Releases](https://img.shields.io/github/release/zerobject/laboost.svg)](https://github.com/zerobject/laboost/releases)

</div>

---

[Switch to English](README_en.md)

# 📜 Содержание
- [Обзор](#обзор)
- [Возможности](#возможности)
- [Установка](#установка)
- [Начало работы](#-быстрый-старт)
- [Примеры](#примеры)
- [Сравнение с другими решениями](#сравнение-с-другими-решениями)
- [Поддержка](#поддержка)
- [Вклад](#вклад)

---

## 🔍 Обзор

**Laboost** — это лёгкий и производительный Dependency Injection (DI) фреймворк для Unity, созданный для минимизации рутины и повышения гибкости кода.

Цели:
- Избавить от жёстких связей между классами.
- Сократить количество ручного связывания объектов.
- Обеспечить простую интеграцию в существующие проекты.

Фреймворк основан на принципах **IoC** и поддерживает **автоматическое внедрение зависимостей** по атрибутам или конфигурации.

---

## 🚀 Возможности

- 📦 **Простая установка** через Git URL или Unity Package.
- ⚡ **Высокая производительность** — минимум лишних аллокаций.
- 🎯 **Инъекция через атрибуты** (`[Inject]`).
- 🛠️ **Поддержка MonoBehaviour и ScriptableObject**.
- 🔁 **Жизненные циклы** (Singleton, Transient и т.д.).
- 🧩 **Гибкая настройка контейнера**.
- 🧪 Простая интеграция с тестами.

---

## 📦Установка

### Через GitHub URL (рекомендуется):

1. Скопируйте https://github.com/zerobject/laboost.git
2. Откройте **Package Manager** (`Window / Package Manager`).
3. Нажмите `+` → `Install package from Git URL...`.
4. Вставьте адрес и нажмите **Add**.

### Ручная установка:

1. Загрузите `.unitypackage` из [релизов](https://github.com/zerobject/laboost/releases).
2. В Unity: `Assets` → `Import Package` → `Custom Package...`.
3. Выберите скачанный файл и импортируйте.

---

## 🖥️Быстрый старт

### Базовое использование

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

📚 Примеры
Регистрация в контейнере
Container.Bind<TestClassA>().AsSingle();
Container.Bind<TestClassB>().AsTransient();

Внедрение в MonoBehaviour
public class Player : MonoBehaviour
{
    [Inject]
    private IWeapon _weapon;

    private void Start()
    {
        _weapon.Fire();
    }
}

⚖ Сравнение с другими решениями
Функция	Laboost	Zenject / Extenject
Простота API	✅	⚠ иногда сложно
Скорость	✅	⚠ медленнее
Поддержка жизненных циклов	✅	✅
Минимум зависимостей	✅	❌
🪙 Поддержка

Если у вас есть вопросы или предложения — создавайте Issues или пишите в Discussions.

🤝 Вклад

Буду рад вашим пулл-реквестам!
Перед отправкой:

Форкните репозиторий.

Создайте ветку: feature/имя.

Добавьте код и тесты.

Откройте Pull Request.