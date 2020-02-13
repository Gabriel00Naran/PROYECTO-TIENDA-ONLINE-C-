namespace ProyectoIntegrador.Models
{

    /// <summary>
    /// ESTE MODELO CONTROLADOR CONTROLA EL VALOR DEL PRODUCTO Y LA CANDIDAD COMO VALORES VARIANTES
 
    /// </summary>
    public class Cart
    {

        public Producto Producto { get; set; }
        public int Quantity { get; set; }
        public Cart(Producto producto, int quantity)
        {
            Producto = producto;
            Quantity = quantity;
        }
    }


}