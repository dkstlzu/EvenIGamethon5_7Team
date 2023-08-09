using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MoonBunny.UIs
{
    public class ConfirmUI : UI
    {
        public TextMeshProUGUI Description;
        
        [SerializeField] private UnityEvent _onConfirm;
        [SerializeField] private UnityEvent _onCancel;
        
        public void OnConfirmButtonClicked()
        {
            _onConfirm?.Invoke();
            
            _onConfirm.RemoveAllListeners();
            _onCancel.RemoveAllListeners();
            
            OnExitButtonClicked();
        }

        public void OnCancelButtonClicked()
        {
            _onCancel?.Invoke();
            
            _onConfirm.RemoveAllListeners();
            _onCancel.RemoveAllListeners();
            
            OnExitButtonClicked();
        }

        public void AddConfirmListener(UnityAction action)
        {
            _onConfirm.RemoveListener(action);
            _onConfirm.AddListener(action);
        }

        public void AddCancelListener(UnityAction action)
        {
            _onCancel.RemoveListener(action);
            _onCancel.AddListener(action);
        }
    }
}