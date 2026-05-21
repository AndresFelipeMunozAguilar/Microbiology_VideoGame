using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserRegister : MonoBehaviour
{
    [Header("Register Inputs")]
    public TMP_InputField  inputCodigo;
    public TMP_InputField inputNombre;
    public TMP_InputField inputPassword;
    [Header("Login Inputs")]
    public TMP_InputField  inputCodigoL;
    public TMP_InputField inputPasswordL;
    public TextMeshProUGUI StateRegister,StateLogin,StateGeneral,Name;
    private FirebaseFirestore db;
    private bool firebaseReady = false;
    public static UserRegister Instance;

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

    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                firebaseReady = true;
                StateGeneral.text="Inicia Sesion para jugar";
            }
            else
            {
                StateGeneral.text="Error de conexion:" + task.Result;
            }
        });

    }


    public void Registrar()
    {
        if (!firebaseReady)
        {
            StateRegister.text="Server No listo";
            return;
        }

        string codigo = inputCodigo.text.Trim();
        string nombre = inputNombre.text.Trim();
        string password = inputPassword.text.Trim();

        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(password))
        {
            StateRegister.text="Todos los campos son obligatorios.";
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
                    StateRegister.text="Usuario ya existente";
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
                        StateRegister.text="Usuario registrado correctamente.";
                        StateGeneral.text= "Registrado Correctamente";
                        inputCodigo.text="";
                        inputNombre.text="";
                        inputPassword.text="";
                        FindAnyObjectByType<Menu>().RegisterWindowState(false);
                        Loguear(codigo,password);
                    }
                    else
                    {
                        StateRegister.text="Error al registrar: " + t.Exception;
                    }
                });
            }
            else
            {
                 StateRegister.text="Error consultando usuario: " + task.Exception;
            }
        });
    }
    
    public void Login()
    {
        StateLogin.text = "Cargando...";
        if (!firebaseReady)
        {
            StateLogin.text = "Server no listo";
            return;
        }

        string codigo = inputCodigoL.text.Trim();
        string password =  inputPasswordL.text.Trim();
        Loguear(codigo,password);
    }

    public void Loguear(string codigo, string password)
    {
        if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(password))
        {
            StateLogin.text = "Ingrese código y contraseña";
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
                    StateLogin.text = "Usuario no encontrado";
                    return;
                }

                string storedPassword = snapshot.GetValue<string>("password");
                string inputPasswordMD5 = GetMD5(password);

                if (storedPassword == inputPasswordMD5)
                {
                    StateLogin.text = "Login exitoso";
                    Name.text= "Bienvenido " +snapshot.GetValue<string>("nombre");
                    FindAnyObjectByType<Menu>().activePlay();
                    inputCodigoL.text="";
                    inputPasswordL.text="";
                    FindAnyObjectByType<Menu>().LoginWindowState(false);
                    FirebaseResultsUploader.Instance.setPlayerId(codigo);
                    PlayerPrefs.SetString("playerID", codigo);
                    PlayerPrefs.Save();

                }
                else
                {
                    StateLogin.text = "Contraseña incorrecta";
                }
            }
            else
            {
                StateLogin.text = "Error en login: " + task.Exception;
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
            }
            else
            {
                Debug.LogError("[Firebase] Error subiendo resultado: " + task.Exception);
                tx.text=  "Error subiendo resultados: " +task.Exception;
            }
        });
        
    }

    public void UploadEvaluationFromFile2(TextMeshProUGUI tx)
    {
        string path = Application.persistentDataPath + "/evaluation.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("[Firebase] No existe evaluation.json en: " + path);
            tx.text= "No se encontraron resultados";
        }

        string json = File.ReadAllText(path);
        EvaluationData data = JsonUtility.FromJson<EvaluationData>(json);

        UploadEvaluation(data,tx);
    }

    public void ListarUsuarios()
    {
        if (!firebaseReady)
        {
            StateGeneral.text = "Firebase no listo";
            return;
        }

        db.Collection("usuarios").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;

                if (snapshot.Count == 0)
                {
                    StateGeneral.text = "No hay usuarios";
                    return;
                }

                string resultado = "Usuarios:\n";
                string codigo = inputCodigoL.text.Trim();

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

                StateGeneral.text = resultado;
            }
            else
            {
                StateGeneral.text = "Error listando usuarios: " + task.Exception;
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