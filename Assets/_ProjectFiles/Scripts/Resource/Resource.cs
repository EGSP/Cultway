namespace Game.Resources
{
    public class Resource
    {
        public readonly ResourceInfo Info;
        
        /// <summary>
        /// Количество данного ресурса.
        /// </summary>
        public int Count { get; set; }

        public Resource(ResourceInfo info)
        {
            Info = info;
            Count = 0;
        }
    }
}