using TMPro;
using UnityEngine;

public class TxMenu : MonoBehaviour
{
    [Header("Register Inputs")]
    public TMP_InputField  inputCodigo;
    public TMP_InputField inputNombre;
    public TMP_InputField inputPassword;
    [Header("Login Inputs")]
    public TMP_InputField  inputCodigoL;
    public TMP_InputField inputPasswordL;
    public TextMeshProUGUI StateRegister,StateLogin,StateGeneral;

    private void Start() {
        if (UserRegister.Instance == null)
        {
            Debug.LogError("[TxMenu] No se encontro UserRegister en la escena.");
            return;
        }

        UserRegister.Instance.setText(this);
        UserRegister.Instance.Relogin();
        
    }
}
