using DG.Tweening;
using RunSettings;
using SlashABit.UI.HudElements;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Color completeColor;
    public Image point;

    public RectTransform rectTransform;
    public TextMeshProUGUI objectiveText;

    private ActiveHudHandler<string> hudHandlerObjective;

    private void Awake()
    {
        hudHandlerObjective = new ActiveHudHandler<string>(3, canvasGroup);
    }
    private void Update()
    {
        hudHandlerObjective.Update(RunManager.activeHud, objectiveText.text);
    }
    public IEnumerator SetListElement(string text, float time)
    {
        yield return new WaitForSeconds(time);
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;
        SetColor(Color.white);
        gameObject.SetActive(true);
        objectiveText.text = text;
    }
    public void UpdateListElement(string text)
    {
        objectiveText.text = text;
        rectTransform.DOKill();
        rectTransform.localScale= Vector3.one;
        rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
    }
    public void Completed(string text, string newObjective)
    {
        UpdateListElement(text);
        SetColor(completeColor);

        if (newObjective == "")
        {
            Invoke(nameof(Disable), 1.5f);
        }
        else
        {
            StartCoroutine(SetListElement(newObjective, 1.5f));
        }
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color newColor)
    {
        objectiveText.color = newColor;
        point.color = newColor;
    }
}
