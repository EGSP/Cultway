using Egsp.Core.Ui;
using Game.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public interface IResourceOperationEffectVisual : IVisual<IResourceOperationEffectVisual>
    {
        void Accept(ResourceInfo resourceInfo, IResourceOperation resourceOperation);
    }
    
    public class ResourceOperationEffectVisual : SerializedVisual<IResourceOperationEffectVisual>,
        IResourceOperationEffectVisual
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text operationEffectText;

        public void Accept(ResourceInfo resourceInfo, IResourceOperation resourceOperation)
        {
            icon.sprite = resourceInfo.Sprite;
            operationEffectText.text = resourceOperation.GetDescriptionSymbols();
            operationEffectText.color = resourceOperation.GetColor();
        }
    }
}