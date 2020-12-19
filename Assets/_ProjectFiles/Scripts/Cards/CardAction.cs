using Game.Resources;

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
    }
}