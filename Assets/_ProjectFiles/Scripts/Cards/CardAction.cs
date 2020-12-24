using System;
using Game.Resources;
using UnityEngine;

namespace Game.Cards
{
    [System.Serializable]
    public class CardAction
    {
        /// <summary>
        /// Ресурс с которым данная операция работает.
        /// </summary>
        public ResourceInfo resourceInfo;
        
        /// <summary>
        /// Операция совершаемая этим действием.
        /// </summary>
        public IResourceOperation Operation;

        [TextArea]
        public string description;

        public string GetDecription()
        {
            return description + Operation?.GetDescriptionSymbols();
        }

        public void Invoke(IResourcesStorage storage)
        {
            var sourceResource = storage.GetResourceByInfo(resourceInfo);
            if(sourceResource == null)
                throw new NullReferenceException();
            
            Operation.Invoke(sourceResource);
        }
    }
}