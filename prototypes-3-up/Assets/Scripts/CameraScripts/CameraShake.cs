using System.Collections;
using UnityEngine;

namespace CameraScripts
{
    public class CameraShake : MonoBehaviour
    {
        public float intensity = 1f;
        public float duration;
        [SerializeField] private AnimationCurve curve;

        private Vector3 _initialPosition;

        private Coroutine _coroutine;

        private void Awake()
        {
            _initialPosition = transform.localPosition;
        }

        public void Shake()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ShakeCoroutine(duration, intensity));
        }

        public void Shake(float d, float i)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(ShakeCoroutine(d, i));
        }

        private IEnumerator ShakeCoroutine(float d, float i)
        {
            //set elapsed time
            float elapsedTime = 0;

            //shake random inside unit sphere
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
            
                float curveAdjustment = curve.Evaluate(elapsedTime / d);
            
                transform.localPosition = _initialPosition + Random.insideUnitSphere * curveAdjustment * i;

                yield return null;
            }
        
            //reset initial position
            transform.localPosition = _initialPosition;
        }
    }
}
