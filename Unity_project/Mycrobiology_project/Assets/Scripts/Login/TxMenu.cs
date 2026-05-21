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
        UserRegister.Instance.setText(this);
        UserRegister.Instance.Relogin();
        
    }
}