using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameplayDesechos : AbstractPuzzleGameplay
{
    [Header("Desechos disponibles")]
    [SerializeField] private List<DesechoElement> desechos = new List<DesechoElement>();

    [Header("Posiciones de spawn")]
    [SerializeField] private List<Transform> positions = new List<Transform>();

    [Header("Prefabs")]
    [SerializeField] private GameObject blankDesecho;
    [SerializeField] private GameObject FeedbackElement;
    [SerializeField] private GameObject CorrectElement;
    [SerializeField] private HipocloritoRegar hipocloritoRegar;

    private int elementsFinished = 0;
    private int totalElements = 0;

    private PuzzleEvaluation score;

    private bool IsPerfect = true;

    private readonly string feedbackIncorrectoGeneral = "Incorrecto";

    protected override void OnStartGameplay()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        score = GetComponent<PuzzleEvaluation>();

        
        _puzzleEvaluation = score;

        if (_title)
        {
            _title.text = score.puzzleID.ToString();
        }

        if (hipocloritoRegar != null)
        {
            hipocloritoRegar.CreateElement(score, this);
        }

        SelectDesechos();
    }

    private void SelectDesechos()
    {
        if (blankDesecho == null)
        {
            Debug.LogError("[DESECHOS] No se asignó el blankDesecho en el inspector.");
            return;
        }

        if (desechos.Count == 0)
        {
            Debug.LogError("[DESECHOS] La lista de DesechoElement está vacía.");
            return;
        }

        if (positions.Count == 0)
        {
            Debug.LogError("[DESECHOS] No hay posiciones de spawn asignadas.");
            return;
        }

        List<DesechoElement> selectDesechos = new List<DesechoElement>(desechos);

        int amountToSpawn = Mathf.Min(positions.Count, selectDesechos.Count);
        totalElements = amountToSpawn;

        for (int i = 0; i < amountToSpawn; i++)
        {
            int randomIndex = Random.Range(0, selectDesechos.Count);

            GameObject newDesecho = Instantiate(
                blankDesecho,
                positions[i].position,
                Quaternion.identity,
                transform
            );

            Desecho manager = newDesecho.GetComponent<Desecho>();

            if (manager == null)
            {
                Debug.LogError("[DESECHOS] El blankDesecho no tiene el script Desecho.");
                Destroy(newDesecho);
                continue;
            }

            manager.CreateElement(selectDesechos[randomIndex], score, this);

            selectDesechos.RemoveAt(randomIndex);
        }
    }

    public void NoPerfect()
    {
        IsPerfect = false;
    }

    public string GetFeedbackIncorrectoGeneral()
    {
        return feedbackIncorrectoGeneral;
    }

    public void CreateFeedback(Vector2 posSpawn, string message)
    {
        if (FeedbackElement == null)
        {
            return;
        }

        GameObject feedback = Instantiate(
            FeedbackElement,
            posSpawn,
            Quaternion.identity,
            transform
        );

        TextMeshProUGUI text = feedback.GetComponentInChildren<TextMeshProUGUI>();

        if (text != null)
        {
            text.text = message;
        }

        Destroy(feedback, 5f);
    }

    public void CorrectFeedback(Vector2 posSpawn)
    {
        if (CorrectElement == null)
        {
            Debug.LogWarning("[DESECHOS] No se asignó CorrectElement.");
            return;
        }

        GameObject feedback = Instantiate(
            CorrectElement,
            posSpawn,
            Quaternion.identity,
            transform
        );

        Destroy(feedback, 5f);
    }

    public void addFinishElement()
    {
        elementsFinished++;

        Debug.Log("[DESECHOS] cuenta: " + elementsFinished + " / " + totalElements);

        if (elementsFinished >= totalElements)
        {
            FinishPuzzle();
        }
    }

    private void FinishPuzzle()
    {
        PlayerDamageDealer damageDealer = GetComponentInParent<PlayerDamageDealer>();

        if (IsPerfect)
        {
            score.AddPoints("BonusPerfect");
        }

        if (damageDealer != null)
        {
            damageDealer.CalculateDamage(score.GetCurrentScore());
        }

        bool Finish = score.GetCurrentScore() > 60;

        score.FinishGame(Finish);

        if (Finish)
        {

            Victory();
        }
        else
        {
            Defeat();
        }
    }

    public override void Victory()
    {
        GetComponentInParent<PlayerDamageDealer>().CalculateDamage(score.GetCurrentScore());
        NotifyPuzzleVictory(true);
        Destroy(gameObject);
    }

    public override void Defeat()
    {
        GetComponentInParent<PlayerDamageDealer>().CalculateDamage(score.GetCurrentScore());
        NotifyPuzzleVictory(false);
        Destroy(gameObject);
    }
}