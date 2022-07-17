using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public enum SceneType
{ 
    mainMenu = 0,
    mainBattle = 1
}

public class MainMenuManager : MonoBehaviour
{
    public CanvasGroup overlay;
    public RectTransform logo;

    private void Start()
    {
        ShowOverlay(false);
        AnimateLogo();

        SoundManager.Instance.Play(Sounds.menuMusic, true);
    }

    private IEnumerator LoadBattleScene()
    {
        ShowOverlay(true);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene((int)SceneType.mainBattle);
    }

    public void FightButton()
    {
        StartCoroutine(LoadBattleScene());
    }

    public void ShowOverlay(bool show)
    {
        if (show)
            overlay.DOFade(1.0f, 0.5f);
        else
            overlay.DOFade(0.0f, 0.5f);
    }

    private void AnimateLogo()
    {
        float orgPosY = logo.localPosition.y;

        Sequence logoAnimSeq = DOTween.Sequence();
        logoAnimSeq.Append(logo.DOAnchorPosY(orgPosY + 25f, 0.2475f))
                   .SetLoops(-1, LoopType.Yoyo);
    }
}
