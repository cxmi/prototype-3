using System.Collections;
using UnityEngine;

public class DashAfterEffect : MonoBehaviour
{
    private Coroutine _coroutine;

    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Color effectColor;

    [SerializeField] private float duration;
    [SerializeField] private int quantity;
    [SerializeField] private float lifespan;
    
    public void AfterEffect()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(AfterEffectCoroutine(duration, quantity, lifespan));
    }

    private IEnumerator AfterEffectCoroutine(float d, int q, float l)
    {
        WaitForSeconds wait = new WaitForSeconds(d/q);
        for (int i = 0; i < q; i++)
        {
            GameObject effect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y, 1), transform.rotation);
            DashEffectScript effectScript = effect.GetComponent<DashEffectScript>();
            effectScript.timer = l;
            effectScript.color = effectColor;
            yield return wait;
        }
    }
}
