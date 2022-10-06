using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriteEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 20f;
    public TextMeshProUGUI textLabel;

    public void Start()
    {
        Run();
    }

    public void Update()
    {
        if(Input.anyKeyDown)
        {
            typewriterSpeed = 1000f;
        }
    }

    public void Run()
    {
        StartCoroutine(TypeText(textLabel.text, textLabel));
    }

    private IEnumerator TypeText(string textToType, TextMeshProUGUI textLabel)
    {
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            
            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
    }
}
