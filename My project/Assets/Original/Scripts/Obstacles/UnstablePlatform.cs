using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePlatform : MonoBehaviour
{
    [SerializeField] private float triggerDelay = 1;
    [SerializeField] private float diffLayerDelay = 0.3f;
    [SerializeField] private float selfDestroyDelay = 1.2f;

    private Animator _animator;

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 12)
        {
            return;
        }

        trigger();
    }

    void trigger()
    {

        StartCoroutine(selfDestroyCoroutine());
    }

    private IEnumerator selfDestroyCoroutine()
    {
        yield return new WaitForSeconds(triggerDelay);
        _animator.SetTrigger("destroy");
        yield return new WaitForSeconds(diffLayerDelay);
        gameObject.layer = LayerMask.NameToLayer("Enemy death layer");
        yield return new WaitForSeconds(selfDestroyDelay);
        Destroy(gameObject);
    }
}
