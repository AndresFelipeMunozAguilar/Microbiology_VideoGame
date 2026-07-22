using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using TMPro;
using UnityEngine;

public class UserRegister : MonoBehaviour
{

    private FirebaseFirestore db;
    private bool firebaseReady = false;
    public static UserRegister Instance;
    TxMenu tx;
    private string currentUser,currentPassword;
    bool isLogin;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setText(TxMenu newTx)
    {
        tx=newTx;
        if (firebaseReady)
        {
            SetGeneralStatus("Inicia Sesion para jugar");
        }
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                firebaseReady = false;
                isLogin=false;
                Debug.LogError("[Firebase] Error revisando dependencias: " + task.Exception);
                SetGeneralStatus("Error de conexion con Firebase");
                return;
            }

            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                firebaseReady = true;
                SetGeneralStatus("Inicia Sesion para jugar"); 
            }
            else
            {
                SetGeneralStatus("Error de conexion:" + task.Result);
                isLogin=false;
            }
        });

    }
    
    public void Relogin()
    {
        if(!isLogin)return;
        Loguear(currentUser,currentPassword);
    }

    public void Registrar()
    {
        isLogin=false;
        if (!EnsureTextReferences())
            return;

        if (!firebaseReady)
        {
            SetRegisterStatus("Server No listo");
            return;
        }

        string codigo = ReadInput(tx.inputCodigo);
        string nombre = ReadInput(tx.inputNombre);
        string password = ReadInput(tx.inputPassword);

        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(password))
        {
            SetRegisterStatus("Todos los campos son obligatorios.");
            return;
        }

        DocumentReference userRef = db.Collection("usuarios").Document(codigo);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    SetRegisterStatus("Usuario ya existente");
                    return;
                }

                string passwordMD5 = GetMD5(password);

                Dictionary<string, object> userData = new Dictionary<string, object>
                {
                    { "codigo", codigo },
                    { "nombre", nombre },
                    { "password", passwordMD5 },
                    { "createdAt", Timestamp.GetCurrentTimestamp() }
                };

                userRef.SetAsync(userData).ContinueWithOnMainThread(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        SetRegisterStatus("Usuario registrado correctamente.");
                        SetGeneralStatus("Registrado Correctamente");
                        ClearInput(tx.inputCodigo);
                        ClearInput(tx.inputNombre);
                        ClearInput(tx.inputPassword);
                        Menu menu = FindAnyObjectByType<Menu>();
                        if (menu != null)
                        {
                            menu.RegisterWindowState(false);
                        }
                        Loguear(codigo,password);
                    }
                    else
                    {
                        SetRegisterStatus("Error al registrar: " + t.Exception);
                    }
                });
            }
            else
            {
                SetRegisterStatus("Error consultando usuario: " + task.Exception);
            }
        });
    }
    
    public void Login()
    {
        if (!EnsureTextReferences())
            return;

        SetLoginStatus("Cargando...");
        if (!firebaseReady)
        {
            SetLoginStatus("Server no listo");
            return;
        }

        string codigo = ReadInput(tx.inputCodigoL);
        string password =  ReadInput(tx.inputPasswordL);
        Loguear(codigo,password);
    }

    public void Loguear(string codigo, string password)
    {
        isLogin=false;
        if (!EnsureTextReferences())
            return;

        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(password))
        {
            SetLoginStatus("Ingrese codigo y contrasena");
            return;
        }

        if (db == null)
        {
            SetLoginStatus("Server no listo");
            return;
        }

        DocumentReference userRef = db.Collection("usuarios").Document(codigo);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                DocumentSnapshot snapshot = task.Result;

                if (!snapshot.Exists)
                {
                    SetLoginStatus("Usuario no encontrado");
                    return;
                }

                if (!snapshot.ContainsField("password"))
                {
                    SetLoginStatus("Usuario sin contrasena registrada");
                    return;
                }

                string storedPassword = snapshot.GetValue<string>("password");
                string inputPasswordMD5 = GetMD5(password);

                if (storedPassword == inputPasswordMD5)
                {
                    currentUser=codigo;
                    currentPassword=password;
                    isLogin=true;
                    string userName = snapshot.ContainsField("nombre") ? snapshot.GetValue<string>("nombre") : codigo;
                    SetLoginStatus("Login exitoso");
                    SetGeneralStatus("Bienvenido " + userName);
                    PlayerPrefs.SetString("playerID", codigo);
                    PlayerPrefs.Save();
                    if (FirebaseResultsUploader.Instance != null)
                    {
                        FirebaseResultsUploader.Instance.setPlayerId(codigo);
                    }
                    else
                    {
                        Debug.LogWarning("[Firebase] FirebaseResultsUploader no esta en la escena. Se continua usando PlayerPrefs.");
                    }

                    if (DataManager.Instance != null)
                    {
                        DataManager.Instance.SetPlayerID(codigo);
                        LoadBestScoresFromUserSnapshot(codigo, snapshot);
                    }
                    Menu menu = FindAnyObjectByType<Menu>();
                    if (menu != null)
                    {
                        menu.activePlay();
                        menu.LoginWindowState(false);
                    }

                    ClearInput(tx.inputCodigoL);
                    ClearInput(tx.inputPasswordL);
                }
                else
                {
                    SetLoginStatus("Contrasena incorrecta");
                }
            }
            else
            {
                SetLoginStatus("Error en login: " + task.Exception);
            }
        });
    }
    string GetMD5(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }

        return sb.ToString();
    }

    private void LoadBestScoresFromUserSnapshot(string playerID, DocumentSnapshot snapshot)
    {
        if (DataManager.Instance == null)
            return;

        if (!snapshot.ContainsField("bestScores"))
            return;

        Dictionary<string, object> bestScores = snapshot.GetValue<Dictionary<string, object>>("bestScores");
        if (bestScores == null || bestScores.Count == 0)
            return;

        EvaluationData data = new EvaluationData
        {
            playerID = playerID,
            totalScore = 0,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            puzzles = new System.Collections.Generic.List<PuzzleResultData>()
        };

        foreach (var kvp in bestScores)
        {
            int bestValue = 0;
            if (kvp.Value is int i)
                bestValue = i;
            else if (kvp.Value is long l)
                bestValue = (int)l;

            data.puzzles.Add(new PuzzleResultData
            {
                puzzleID = kvp.Key,
                score = 0,
                bestScore = bestValue,
                tutorialFlag = false,
                performance = ""
            });
        }

        DataManager.Instance.SaveEvaluation(data, playerID);
    }

    private string GetPlayerEvaluationPath()
    {
        string playerID = PlayerPrefs.GetString("playerID", "");
        if (string.IsNullOrEmpty(playerID))
            return Application.persistentDataPath + "/evaluation.json";

        return Application.persistentDataPath + $"/evaluation_{playerID}.json";
    }

    public void UploadEvaluation(EvaluationData data,TextMeshProUGUI tx, GameObject btButton = null)
    {
        if (!firebaseReady)
        {
            Debug.LogWarning("[Firebase] Todavía no está listo.");
            tx.text = "Firebase no listo. Pulsa 'Reintentar' para volver a intentar.";
            if (btButton != null) btButton.SetActive(true);
            return;
        }

        tx.text = "Enviando...";
        if (btButton != null) btButton.SetActive(false);
        StartCoroutine(HandleUploadTimeout(tx, btButton, data));

        string rawJson = JsonUtility.ToJson(data, true);

        List<Dictionary<string, object>> puzzleList = new List<Dictionary<string, object>>();

        foreach (PuzzleResultData puzzle in data.puzzles)
        {
            Dictionary<string, object> puzzleData = new Dictionary<string, object>
            {
                { "puzzleID", puzzle.puzzleID },
                { "score", puzzle.score },
                { "bestScore", puzzle.bestScore },
                { "performance", puzzle.performance }
            };

            puzzleList.Add(puzzleData);
        }
        data.playerID = PlayerPrefs.GetString("playerID", "");
        Dictionary<string, object> result = new Dictionary<string, object>
        {
            { "playerID", data.playerID },
            { "totalScore", data.totalScore },
            { "date", data.date },
            { "puzzles", puzzleList },
            { "rawJson", rawJson },
            { "createdAt", Timestamp.GetCurrentTimestamp() }
        };

        db.Collection("game_results").AddAsync(result).ContinueWithOnMainThread(task =>
        {
            StopCoroutine(HandleUploadTimeout(tx, btButton, data));
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("[Firebase] Resultado subido correctamente.");
                tx.text = "Resultados enviados al profesor";
                if (btButton != null) btButton.SetActive(false);
                SetBestScoresForUser(data.playerID, data.puzzles);
            }
            else
            {
                Debug.LogError("[Firebase] Error subiendo resultado: " + task.Exception);
                tx.text = "Error subiendo resultados: " + task.Exception;
                if (btButton != null) btButton.SetActive(true);
            }
        });
        
    }

    private IEnumerator HandleUploadTimeout(TextMeshProUGUI tx, GameObject btButton, EvaluationData data)
    {
        yield return new WaitForSeconds(8f);

        tx.text = "Error subiendo resultados: tiempo de espera agotado.";
        if (btButton != null) btButton.SetActive(true);
    }

    private void SetBestScoresForUser(string playerID, System.Collections.Generic.List<PuzzleResultData> puzzles)
    {
        if (string.IsNullOrEmpty(playerID) || puzzles == null)
            return;

        Dictionary<string, object> bestScores = new Dictionary<string, object>();
        foreach (PuzzleResultData puzzle in puzzles)
        {
            bestScores[puzzle.puzzleID] = puzzle.bestScore;
        }

        DocumentReference userRef = db.Collection("usuarios").Document(playerID);
        userRef.SetAsync(new Dictionary<string, object> { { "bestScores", bestScores } }, SetOptions.MergeAll)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("[Firebase] bestScores actualizados en usuario.");
                }
                else
                {
                    Debug.LogError("[Firebase] Error actualizando bestScores: " + task.Exception);
                }
            });
    }

    public void UploadEvaluationFromFile2(TextMeshProUGUI tx, GameObject btButton = null)
    {
        string path = GetPlayerEvaluationPath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Firebase] No existe evaluation.json en: " + path);
            tx.text= "No se encontraron resultados";
            if (btButton != null) btButton.SetActive(true);
            return;
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        UploadEvaluation(data, tx, btButton);
    }

    public void ListarUsuarios()
    {
        if (!EnsureTextReferences())
            return;

        if (!firebaseReady)
        {
            SetGeneralStatus("Firebase no listo");
            return;
        }

        db.Collection("usuarios").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;

                if (snapshot.Count == 0)
                {
                    SetGeneralStatus("No hay usuarios");
                    return;
                }

                string resultado = "Usuarios:\n";
                string codigo = ReadInput(tx.inputCodigoL);

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    string id = doc.Id;
                    string nombre = doc.ContainsField("nombre") ? doc.GetValue<string>("nombre") : "Sin nombre";

                    resultado += $"ID: [{id}] Nombre: {nombre}\n";
                    if (codigo == id)
                    {
                        Debug.Log("Coincidencia "+codigo + " : "+id);
                    }
                    Debug.Log(id + " : " + id.Length);
                }

                SetGeneralStatus(resultado);
            }
            else
            {
                SetGeneralStatus("Error listando usuarios: " + task.Exception);
            }
        });
    }

    private bool EnsureTextReferences()
    {
        if (tx != null)
            return true;

        tx = FindAnyObjectByType<TxMenu>();
        if (tx != null)
            return true;

        Debug.LogError("[UserRegister] No se encontro TxMenu en la escena.");
        return false;
    }

    private void SetRegisterStatus(string message)
    {
        if (EnsureTextReferences() && tx.StateRegister != null)
            tx.StateRegister.text = message;
        else
            Debug.LogWarning("[UserRegister] " + message);
    }

    private void SetLoginStatus(string message)
    {
        if (EnsureTextReferences() && tx.StateLogin != null)
            tx.StateLogin.text = message;
        else
            Debug.LogWarning("[UserRegister] " + message);
    }

    private void SetGeneralStatus(string message)
    {
        if (EnsureTextReferences() && tx.StateGeneral != null)
            tx.StateGeneral.text = message;
        else
            Debug.LogWarning("[UserRegister] " + message);
    }

    private static string ReadInput(TMP_InputField input)
    {
        return input != null ? input.text.Trim() : string.Empty;
    }

    private static void ClearInput(TMP_InputField input)
    {
        if (input != null)
        {
            input.text = string.Empty;
        }
    }

    [Serializable]
    public class SerializableUser
    {
        public string codigo;
        public string nombre;
        public string password;

        public SerializableUser(string c, string n, string p)
        {
            codigo = c;
            nombre = n;
            password = p;
        }
    }

}
