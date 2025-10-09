using System.Collections.Generic;
using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerDamage : MonoBehaviour
    { 
        public int damage = 0;
        public int maxDamage = 3;

        [Header("Sprite")]
        [SerializeField] private Sprite damagedSprite;
        private Sprite _originalSprite;

        [Header("Colors")]
        [SerializeField] private Color damagedColor;
        private Color _originalColor;

        [Header("Duration")]
        public float damageTime = 0.1f;
    
        [Header("Camera Shake")]
        [SerializeField] private CameraShake cameraShake;

        [Header("Audio")] 
        [SerializeField] private List<AudioClip> hurtSounds;
        [SerializeField] private float volume;
    
        //privates
        private float _damagedTimer;
        private bool _damaged;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _damagedTimer -= Time.deltaTime;
            if (_damagedTimer <= 0) _damagedTimer = 0;

            if (_damagedTimer == 0 && _damaged)
            {
                _spriteRenderer.sprite = _originalSprite;
                _spriteRenderer.color = _originalColor;
            }
        }

        public void Damage()
        {
            //damage player
            damage++;
        
            //set to damaged state and start timer
            _damaged = true;
            _damagedTimer = damageTime;
        
            //do camera shake
            cameraShake.Shake();

            //if dead
            if (damage > maxDamage)
            {
                //make sure to not exceed max damage
                damage = maxDamage;
            }
        
            //preserve original sprite and color
            _originalSprite = _spriteRenderer.sprite;
            _originalColor = _spriteRenderer.color;
        
            //change sprite and color
            _spriteRenderer.sprite = damagedSprite;
            _spriteRenderer.color = damagedColor;
        
            //SFX_INSERT_HERE
            //HURT SOUND
        }
    }
}
