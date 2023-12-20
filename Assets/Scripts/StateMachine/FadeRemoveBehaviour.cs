using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    public float fadeDelay = 0.0f;
    private float _timeElapsed = 0f;
    SpriteRenderer _spriteRenderer;
    private float _fadeDelayElapsed;
    Color _startColor;

    private GameObject _objToRemove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timeElapsed = 0f;
        _spriteRenderer = animator.GetComponent<SpriteRenderer>();
        _startColor = _spriteRenderer.color;
        _objToRemove = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (fadeDelay > _fadeDelayElapsed)
        {
            _fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            float newAlpha = _startColor.a * (1 - (_timeElapsed / fadeTime));
            _timeElapsed += Time.deltaTime;
            _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
            if (_timeElapsed > fadeTime)
            {
                Destroy(_objToRemove);
            }
        }
        
    }

   
}
