using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MoonBunny.UIs
{
    public class ConfirmUI : UI
    {
        public TextMeshProUGUI Description;
        
        public UnityEvent OnConfirm;
        public UnityEvent OnCancel;
        
        public void OnConfirmButtonClicked()
        {
            OnConfirm?.Invoke();
            OnExitButtonClicked();
        }

        public void OnCancelButtonClicked()
        {
            OnCancel?.Invoke();
            
            OnConfirm.RemoveAllListeners();
            OnCancel.RemoveAllListeners();
            
            OnExitButtonClicked();
        }
    }
}