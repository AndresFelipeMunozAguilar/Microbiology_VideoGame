using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
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
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                firebaseReady = true;
                tx.StateGeneral.text="Inicia Sesion para jugar"; 
            }
            else
            {
                tx.StateGeneral.text="Error de conexion:" + task.Result;
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
        if (!firebaseReady)
        {
            tx.StateRegister.text="Server No listo";
            return;
        }

        string codigo = tx.inputCodigo.text.Trim();
        string nombre = tx.inputNombre.text.Trim();
        string password = tx.inputPassword.text.Trim();

        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(password))
        {
            tx.StateRegister.text="Todos los campos son obligatorios.";
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
                    tx.StateRegister.text="Usuario ya existente";
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
                        tx.StateRegister.text="Usuario registrado correctamente.";
                        tx.StateGeneral.text= "Registrado Correctamente";
                        tx.inputCodigo.text="";
                        tx.inputNombre.text="";
                        tx.inputPassword.text="";
                        FindAnyObjectByType<Menu>().RegisterWindowState(false);
                        Loguear(codigo,password);
                    }
                    else
                    {
                        tx.StateRegister.text="Error al registrar: " + t.Exception;
                    }
                });
            }
            else
            {
                tx.StateRegister.text="Error consultando usuario: " + task.Exception;
            }
        });
    }
    
    public void Login()
    {
        tx.StateLogin.text = "Cargando...";
        if (!firebaseReady)
        {
            tx.StateLogin.text = "Server no listo";
            return;
        }

        string codigo = tx.inputCodigoL.text.Trim();
        string password =  tx.inputPasswordL.text.Trim();
        Loguear(codigo,password);
    }

    public void Loguear(string codigo, string password)
    {
        isLogin=false;
        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(password))
        {
            tx.StateLogin.text = "Ingrese código y contraseña";
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
                    tx.StateLogin.text = "Usuario no encontrado";
                    return;
                }

                string storedPassword = snapshot.GetValue<string>("password");
                string inputPasswordMD5 = GetMD5(password);

                if (storedPassword == inputPasswordMD5)
                {
                    currentUser=codigo;
                    currentPassword=password;
                    isLogin=true;
                    tx.StateLogin.text = "Login exitoso";
                    tx.StateGeneral.text= "Bienvenido " +snapshot.GetValue<string>("nombre");
                    PlayerPrefs.SetString("playerID", codigo);
                    PlayerPrefs.Save();
                    FirebaseResultsUploader.Instance.setPlayerId(codigo);
                    if (DataManager.Instance != null)
                    {
                        DataManager.Instance.SetPlayerID(codigo);
                        LoadBestScoresFromUserSnapshot(codigo, snapshot);
                    }
                    FindAnyObjectByType<Menu>().activePlay();
                    tx.inputCodigoL.text="";
                    tx.inputPasswordL.text="";
                    FindAnyObjectByType<Menu>().LoginWindowState(false);
                }
                else
                {
                    tx.StateLogin.text = "Contraseña incorrecta";
                }
            }
            else
            {
                tx.StateLogin.text = "Error en login: " + task.Exception;
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

    public void UploadEvaluation(EvaluationData data,TextMeshProUGUI tx)
    {
        if (!firebaseReady)
        {
            Debug.LogWarning("[Firebase] Todavía no está listo.");
            tx.text= "Firebase no listo " +data.playerID+" : " +PlayerPrefs.GetString("playerID", "");
        }

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
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("[Firebase] Resultado subido correctamente.");
                tx.text= "Resultados enviados al profesor";
                SetBestScoresForUser(data.playerID, data.puzzles);
            }
            else
            {
                Debug.LogError("[Firebase] Error subiendo resultado: " + task.Exception);
                tx.text=  "Error subiendo resultados: " +task.Exception;
            }
        });
        
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

    public void UploadEvaluationFromFile2(TextMeshProUGUI tx)
    {
        string path = GetPlayerEvaluationPath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Firebase] No existe evaluation.json en: " + path);
            tx.text= "No se encontraron resultados";
            return;
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        UploadEvaluation(data,tx);
    }

    public void ListarUsuarios()
    {
        if (!firebaseReady)
        {
            tx.StateGeneral.text = "Firebase no listo";
            return;
        }

        db.Collection("usuarios").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;

                if (snapshot.Count == 0)
                {
                    tx.StateGeneral.text = "No hay usuarios";
                    return;
                }

                string resultado = "Usuarios:\n";
                string codigo = tx.inputCodigoL.text.Trim();

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

                tx.StateGeneral.text = resultado;
            }
            else
            {
                tx.StateGeneral.text = "Error listando usuarios: " + task.Exception;
            }
        });
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