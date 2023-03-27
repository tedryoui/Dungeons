using System;
using UnityEngine;

namespace GUI
{
    [Serializable]
    public class ScrollScreenViewModel
    {
        [SerializeField] private ScrollScreenView _view;

        public void Show(Sprite sign, string msg, string date)
        {
            _view.gameObject.SetActive(true);
            _view.signature.sprite = sign;
            _view.date.text = date;
            _view.message.text = msg;
        }

        public void Hide()
        {
            _view.gameObject.SetActive(false);
        }
    }
}