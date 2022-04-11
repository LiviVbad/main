using Prism.Mvvm;

namespace AppFrameworkDemo.Shared.Models
{
    public class EntityObject : BindableBase
    {
        public int Id { get; set; }

        public EntityObject()
        { }

        public EntityObject(int id) => Id = id;
    }
}