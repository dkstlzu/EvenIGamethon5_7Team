using UnityEngine;
using UnityEngine.UI;

namespace EvenI7.Proto.UI
{
    public class HPBar : MonoBehaviour
    {
        private ProtoCharacter _protoCharacter;
        private Slider _hpBar;
        void Start()
        {
            _protoCharacter = GameObject.FindWithTag("Player").GetComponent<ProtoCharacter>();
            _protoCharacter.OnHit += UpdateHPBar;
            _hpBar = GetComponent<Slider>();
            
            UpdateHPBar();
        }

        void UpdateHPBar()
        {
            _hpBar.value = Mathf.Clamp01((float)_protoCharacter.CurrentHP / _protoCharacter.MaxHP);
        }
    }
}