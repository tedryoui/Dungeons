namespace Assets.Scripts.Entities.Interactables
{
    public interface ICraftable : IInteractable
    {
        public Recipe GetRecipe { get; }
        public bool CanCraft(IItemContainer itemContainer);
        public void Craft(IItemContainer itemContainer);
    }
}