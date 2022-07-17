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
    public Overlay overlay;
    public RectTransform logo;

    private void Start()
    {
        overlay.ShowOverlay(false);
        AnimateLogo();

        SoundManager.Instance.Play(Sounds.menuMusic, true, 0.3f);
    }

    public void FightButton()
    {
        overlay.ShowOverlay(true, () =>
        {
            SceneManager.LoadScene((int)SceneType.mainBattle);
        });
    }

    private void AnimateLogo()
    {
        float orgPosY = logo.localPosition.y;

        Sequence logoAnimSeq = DOTween.Sequence();
        logoAnimSeq.Append(logo.DOAnchorPosY(orgPosY + 25f, 0.2475f))
                   .SetLoops(-1, LoopType.Yoyo);
    }
}
