using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SaveEvaluation(EvaluationData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/evaluation.json";

        File.WriteAllText(path, json);

        Debug.Log("Datos guardados en: " + path);
    }

    public static EvaluationData LoadEvaluation()
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<EvaluationData>(json);
    }
}