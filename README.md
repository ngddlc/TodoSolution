# Todo.Core

Библиотека классов .NET 8 для управления списком задач (ToDo List) с поддержкой поиска и JSON-персистенции. Разработана в рамках практического занятия по курсу «Инструментальные средства разработки ПО» с настройкой CI/CD пайплайнов в GitHub Actions.

## Возможности

- Добавление и удаление задач
- Отметка выполнения (`MarkDone`, `MarkUndone`)
- Переименование задач с валидацией (`Rename`)
- Поиск задач по подстроке (`Find`)
- Сохранение и загрузка состояния в/из JSON-файла (`Save`, `Load`)
- Полное покрытие юнит-тестами (xUnit)
- Автоматическая сборка, тестирование и публикация NuGet-пакета в GitHub Packages через GitHub Actions

---

## Подключение пакета через GitHub Packages

### 1. Добавление источника пакетов NuGet

Для подключения приватного/публичного фида GitHub Packages добавьте источник пакетов:

```bash
dotnet nuget add source "https://nuget.pkg.github.com/ngddlc/index.json" \
  --name "GitHubPackages" \
  --username "<GITHUB_USERNAME>" \
  --password "<GITHUB_PAT_TOKEN>" \
  --store-password-in-clear-text
```

### 2. Установка пакета в проект

```bash
dotnet add package Todo.Core --version 0.1.0
```

Либо через файл `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Todo.Core" Version="0.1.0" />
</ItemGroup>
```

---

## Примеры использования

### Создание и управление задачами

```csharp
using Todo.Core;

// Создание списка задач
var todoList = new TodoList();

// Добавление задач
var task1 = todoList.Add("Купить продукты");
var task2 = todoList.Add("Подготовить отчет по практике");

// Отметка выполнения
task1.MarkDone();

// Поиск задач
var found = todoList.Find("отчет");
foreach (var item in found)
{
    Console.WriteLine($"{item.Title} (Выполнено: {item.IsDone})");
}

// Удаление задачи
todoList.Remove(task2.Id);
```

### Сохранение и загрузка в JSON (JSON-персистенция)

```csharp
using Todo.Core;

var list = new TodoList();
list.Add("Изучить GitHub Actions");
list.Add("Опубликовать NuGet-пакет");

// Сохранение в JSON
list.Save("tasks.json");

// Загрузка из JSON
var restoredList = new TodoList();
restoredList.Load("tasks.json");

Console.WriteLine($"Загружено задач: {restoredList.Count}");
```

---

## CI/CD пайплайн

- **CI (`ci.yml`)**: Запускается при каждом `push` и `pull_request` в ветки `master`/`main`. Выполняет восстановление зависимостей, сборку в режиме `Release`, запуск тестов xUnit и сборку пакета `.nupkg`.
- **CD (`publish-package.yml`)**: Запускается автоматически после успешного завершения `CI` и публикует NuGet-пакет в GitHub Packages (`nuget.pkg.github.com`).
