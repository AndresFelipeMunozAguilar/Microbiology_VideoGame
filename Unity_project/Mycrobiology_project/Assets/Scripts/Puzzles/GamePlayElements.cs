using System.Collections.Generic;
using UnityEngine;

public class GamePlayElements : MonoBehaviour
{
    [SerializeField] List<Element> elements= new List<Element>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [SerializeField] GameObject blankElement;

    private void Start() {
        SelectElements();
    }
    void SelectElements()
    {
        List<Element> SelectElements = elements;
        for(int i = 0; i < positions.Count; i++) {
            int pos = Random.Range(0,SelectElements.Count);
            GameObject newElement = Instantiate(blankElement,positions[i]);
            newElement.GetComponent<ElementManager>().CreateElement(SelectElements[pos]);
            SelectElements.RemoveAt(pos);
        }
    }


}
