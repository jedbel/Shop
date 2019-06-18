
namespace Shop.Web.Data.Entities
{
    /*Con esta clase obligamos a que las que Hereden de ella (ej Product.cs), deban tener obligatoriamente la Prop "Id"*/
    public interface IEntity
    {
        int Id { get; set; }
    }
}
