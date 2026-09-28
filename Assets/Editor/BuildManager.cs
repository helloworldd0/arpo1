using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildManager
{
    private static readonly string WebGLBuildPath = "Builds/WebGL";
    
    public static void BuildWebGL()

    {
        Debug.Log("[CI/CD] Запущен автоматический процесссборки WebGL...");
        string[] levels = GetScenes();
        if (levels.Length == 0)
        {
            Debug.LogError("[CI/CD] Ошибка: В Настройках Сборки(Build Settings) не найдено ни одной активной сцены!");
            ExitWithCode(1);
            return;
        }
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = levels,
            locationPathName = WebGLBuildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None 
        };
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"[CI/CD] УСПЕХ! WebGL билд успешно создан.");
            Debug.Log($"[CI/CD] Время сборки: { summary.totalTime.TotalSeconds:F2} сек.Размер: { summary.totalSize} байт.");
        ExitWithCode(0);
        }
        else
        {

            Debug.LogError($"[CI/CD] ОШИБКА СБОРКИ! Количество ошибок: { summary.totalErrors}");
        ExitWithCode(1);
        }
    }
   
private static string[] GetScenes()
    {
        var editorScenes = EditorBuildSettings.scenes;
        
        int activeCount = 0;
        foreach (var scene in editorScenes)
        {
            if (scene.enabled) activeCount++;
        }
        string[] scenePaths = new string[activeCount];
        int index = 0;
        foreach (var scene in editorScenes)
        {
            if (scene.enabled)
            {
                scenePaths[index] = scene.path;
                index++;
            }
        }
        return scenePaths;
    }
    
private static void ExitWithCode(int code) {
if (Environment.CommandLine.Contains("-batchmode"))
        {
            EditorApplication.Exit(code);
        }
    }
}