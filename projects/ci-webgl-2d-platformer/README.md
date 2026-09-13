# Автоматизация сборки 2D-игры (Unity CLI / CI)

Проект по дисциплине «Автоматизация разработки программного обеспечения» (АРПО), лабораторная №1:
2D-платформер на Unity + собственный пайплайн автоматической сборки WebGL из командной строки.

## Ключевые файлы

| Файл | Что это |
|---|---|
| `Assets/Editor/BuildManager.cs` | Editor-скрипт сборки: `BuildPipeline.BuildPlayer()`, проверка сцен, обработка ошибок, коды возврата |
| `Assets/Scenes/Game.unity` | Сцена 2D-платформера |
| `Lab1_Report_АРПО.docx` | Полный отчёт по лабораторной работе |
| `README.md` (внутри) | Отчёт, зафиксированный в Git |

*(Папки Library/Builds исключены из репозитория — стандартная практика для Unity.)*

## Как собрать из терминала

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.4.8f1\Editor\Unity.exe" `
  -batchmode -nographics `
  -projectPath "<путь к этой папке>" `
  -executeMethod SetupBuildSettings.SetupAndBuild `
  -quit -logFile build_webgl.log
```

Результат: папка `Builds/WebGL/` (index.html + .data/.wasm/.js). Для локального запуска —
`python -m http.server 8080` в папке билда (прямой двойной клик по index.html не работает
из-за CORS-политик браузера).

## Что освоено

- Программная сборка WebGL через `BuildPipeline.BuildPlayer()` и `BuildPlayerOptions`.
- Headless-сборка Unity в CLI: `-batchmode -nographics -executeMethod -quit -logFile`
  (успешная сборка ~11 сек, 17.2 МБ), анализ логов.
- Настройка Publishing Settings WebGL (отключение сжатия для локального хостинга).
- Git-флоу: ветки `main` / `LR1`, осмысленные коммиты, Pull Request с ревью и слиянием.
