using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem normalHitEffect;
    [SerializeField] ParticleSystem perfectlHitEffect;


    void OnEnable()
    {
        EventManager.Instance.OnHitEffect += OnHitEffect;
    }
    void OnHitEffect(Transform hitTransform, EHitTiming t)
    {

        if (t == EHitTiming.Perfect)
        {
            perfectlHitEffect.transform.position = hitTransform.position;
            perfectlHitEffect.Play();
        }
        else {
            normalHitEffect.transform.position = hitTransform.position;
            normalHitEffect.Play(); 
        }
    }
    void OnDisable()
    {
        EventManager.Instance.OnHitEffect -= OnHitEffect;

    }
}
