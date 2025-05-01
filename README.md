# TaskRunnerSharp

[![NuGet](https://img.shields.io/nuget/v/TaskRunnerSharp.svg)](https://www.nuget.org/packages/TaskRunnerSharp/)  
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)  

## ✨ Overview

**TaskRunnerSharp** is a minimalistic library that makes it easy to manage, run, and monitor multiple asynchronous or synchronous tasks, with built-in logging and result aggregation.

Whether you're handling fire-and-forget jobs or collecting results from parallel computations, TaskRunnerSharp provides a clean, reliable API.

## 📦 Installation

Install via NuGet Package Manager:

```bash
dotnet add package TaskRunnerSharp
```

Or via Visual Studio ➔ Manage NuGet Packages ➔ Search for `TaskRunnerSharp`.

## 🚀 Features

- **Manage and orchestrate multiple tasks** (`Task` and `Task<T>`).
- **Collect results** from successfully completed typed tasks.
- **Execute tasks synchronously or asynchronously** (`Run` / `RunAsync`).
- **Thread-safe internal operations**.
- **Built-in execution logging** (tasks added, started, completed, or failed).
- **Easy to reset with `Clear()`**.

## ⚡ Quick Start

```csharp
using TaskRunnerSharp;

var runner = new TaskRunner();

// Add tasks
runner.Add(() => Console.WriteLine("Running Action 1"));
runner.Add(() => 123);
runner.Add(() => "Hello TaskRunnerSharp!");

// Run all tasks
runner.Run(); // or await runner.RunAsync();

// Access typed task results
foreach (var result in runner.Results)
{
    Console.WriteLine($"Result: {result}");
}

// View logs
foreach (var log in runner.Logs)
{
    Console.WriteLine(log);
}

// Clear all tasks
runner.Clear();
```

## 📄 Overview

| Method / Property | Description |
|:------------------|:------------|
| `Add(Action)` | Adds an action. |
| `Add<T>(Func<T>)` | Adds a func (returns a value). |
| `Add(Task)` | Adds a non-returning task. |
| `Add<T>(Task<T>)` | Adds a typed task that returns a value. |
| `Run()` | Runs all tasks synchronously. |
| `RunAsync()` | Runs all tasks asynchronously. |
| `Results` | Retrieves a list of results from successfully completed typed tasks. |
| `Logs` | Retrieves execution logs. |
| `Clear()` | Removes all stored tasks and logs the operation. |


## 📝 License

This project is licensed under the [MIT License](LICENSE).

> **TaskRunnerSharp** — keeping your parallelism simple and reliable.