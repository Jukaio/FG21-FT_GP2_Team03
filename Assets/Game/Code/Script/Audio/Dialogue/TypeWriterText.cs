using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWriterText : MonoBehaviour
{

    public float delay = 0.1f; 
    [TextArea] public string fullText;
    private string currentText = "";

    private ContinueExposition continueExposition; 
    
    void Start()
    {
        continueExposition = GameObject.FindGameObjectWithTag("ContinueExposition").GetComponent<ContinueExposition>();
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0,i);
            this.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(2);
        continueExposition.MoveToNext();
    }

    void Update()
    {
        
    }
}
