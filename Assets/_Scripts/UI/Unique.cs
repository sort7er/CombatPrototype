using PlayerSM;
using DG.Tweening;
using RunSettings;
using SlashABit.UI.HudElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unique : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public CanvasGroup group;
    public Player player;
    public Image icon;
    public RectTransform container;
    public Color activeColor, loadingColor;
    public GameObject filter, glyph;
    public Image fillImage;

    private bool loading;
    float timer;

    private ActiveHudHandler<float> hudHandler;

    private void Awake()
    {
        hudHandler = new ActiveHudHandler<float>(3, group);
        fillImage.fillAmount = Tools.Remap(player.uniqueCoolDown,0,player.uniqueCoolDown, 0,1);
        Active();
    }

    public void Active()
    {
        timerText.text = "";
        loading = false;
        icon.color = activeColor;
        filter.SetActive(false);
        glyph.SetActive(true);
        player.CanUseUnique();
    }

    public void Using()
    {
        fillImage.fillAmount = 0.1f;
        fillImage.fillAmount = 1f;
        container.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutExpo);
        filter.SetActive(true);
        glyph.SetActive(false);
    }

    public void Loading()
    {
        timer = player.uniqueCoolDown;
        container.DOScale(Vector3.one, 0.2f);
        loading = true;
        icon.color = loadingColor;
        filter.SetActive(false);
    }
    private void Update()
    {
        if(loading)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F0");
            fillImage.fillAmount = Tools.Remap(timer, 0, player.uniqueCoolDown, 1, 0);
            if(timer <= 0)
            {
                Active();
            }
        }

        hudHandler.Update(RunManager.activeHud, fillImage.fillAmount);
    }

}
