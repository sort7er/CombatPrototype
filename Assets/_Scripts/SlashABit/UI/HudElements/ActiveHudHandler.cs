using System.Collections.Generic;
using UnityEngine;

namespace SlashABit.UI.HudElements {

public class ActiveHudHandler<T> {

    private readonly float _activeHudDelay;
    private readonly CanvasGroup _canvasGroup;
    
    private T _previousValue;
    private float _activeHudTimer;

    public ActiveHudHandler(float activeHudDelay, CanvasGroup canvasGroup) {
        _activeHudDelay = activeHudDelay;
        _canvasGroup = canvasGroup;
    }

    public void Update(bool useActiveHud, T currentValue) {
        if (useActiveHud) {
            if (!EqualityComparer<T>.Default.Equals(_previousValue, currentValue)) {
                _previousValue = currentValue;
                _activeHudTimer = _activeHudDelay;
            }
        } else {
            _canvasGroup.alpha = 1;
            _activeHudTimer = _activeHudDelay;
        }

        //Full opacity for first 2/3 of delay, then fade out
        if (useActiveHud && _activeHudTimer > 0) {
            _activeHudTimer -= Time.deltaTime;
            _canvasGroup.alpha = _activeHudTimer > _activeHudDelay * .33f 
                ? 1 
                : _activeHudTimer / (_activeHudDelay * .33f);
        }
    }
}

}