using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomerunZone : MonoBehaviour
{
    [SerializeField] ParticleSystem pSystem;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            pSystem.Play();
        }
    }
}
