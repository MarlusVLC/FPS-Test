using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float delay = 5f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explsionForce = 700f;
    [SerializeField] private GameObject explosionEffect;
    
    float _countdown;
    private bool _hasExploded;

    // Start is called before the first frame update
    void Start()
    {
        _countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        _countdown -= Time.deltaTime;
        if (_countdown <= 0f && !_hasExploded)
        {
            Explode();
            _hasExploded = true;

        }
    }


    private void Explode()
    {
        // Mostrar efeito
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Pegar objetos próximos
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Adicionar forca
                rb.AddExplosionForce(explsionForce, transform.position, explosionRadius);
            }

            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Die();
            }
        }
        
        
        
        
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Adicionar forca
                rb.AddExplosionForce(explsionForce, transform.position, explosionRadius);
            }
        }
        
        
        // Dano


        //Remover granada
        Destroy(gameObject);
    }
}
