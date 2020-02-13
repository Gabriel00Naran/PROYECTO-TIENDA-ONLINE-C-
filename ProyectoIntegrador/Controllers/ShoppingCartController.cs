using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProyectoIntegrador.Models;
using PayPal.Api;
using System.Linq;

namespace ProyectoIntegrador.Controllers
{
    public class ShoppingCartController : Controller
    {

        private TienditaDeadpoolDBEntities2 db = new TienditaDeadpoolDBEntities2();
        private string strCard = "Cart";
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrdeneAhora(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[strCard] == null)
            {
                List<Cart> lsCarts = new List<Cart>
                {
                    new Cart(db.Producto.Find(id),1)
                };
                Session[strCard] = lsCarts;

            }
            else
            {
                // FUNCION PARA QUE NO TOME VALORES REPETIDOS 
                List<Cart> lsCarts = (List<Cart>)Session[strCard];
                int checkealo = revisarSiExiste(id);
                if (checkealo == -1)
                    lsCarts.Add(new Cart(db.Producto.Find(id), 1));

                else
                    lsCarts[checkealo].Quantity++;
                Session[strCard] = lsCarts;

            }
            return View("Index");

        }
        private int revisarSiExiste(int? id)
        {
            List<Cart> lsCarts = (List<Cart>)Session[strCard];
            for (int i = 0; i < lsCarts.Count; i++)
            {
                if (lsCarts[i].Producto.ProductoId == id) return i;
            }
            return -1;
        }
        public ActionResult EliminarPedido(int? id)
        {
            if (id == null)
            {
                Session.Remove(strCard);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int checkealo = revisarSiExiste(id);
            List<Cart> lsCarts = (List<Cart>)Session[strCard];
            lsCarts.RemoveAt(checkealo);
            return View("Index");
        }
        public ActionResult ActualizarCart(FormCollection frc)
        {
            string[] cantidades = frc.GetValues("quantity");
            List<Cart> lstCart = (List<Cart>)Session[strCard];
            for (int i = 0; i < lstCart.Count; i++)
            {
                lstCart[i].Quantity = Convert.ToInt32(cantidades[i]);
            }
            Session[strCard] = lstCart;
            return View("Index");
        }


        public ActionResult ChekearSalida()
        {
            return View("ChekearSalida");
        }

        public ActionResult ProcesarOrden(FormCollection frc)
        {
            List<Cart> lstCart = (List<Cart>)Session[strCard];
            //1. guarda la orden el la tabla de base de datos
            Orden orden = new Orden()
            {
                ClienteNombre = frc["CliNombre"],
                ClienteTelefono = frc["CliTelefono"],
                ClientEmail = frc["CliEmai"],
                ClienteDirreccion = frc["CliDireccion"],
                OrdenFecha = DateTime.Now,
                TipoPago = "Paypal",
                Status = "Procesado en PayPal"
            };
            db.Orden.Add(orden);
            db.SaveChanges();

            //2. toma los valores de la orden de detalle 

            foreach (Cart cart in lstCart)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    OrdenID = orden.OrdenID,
                    ProductoID = cart.Producto.ProductoId,
                    Cantidad = cart.Quantity, // RECUERDA QUE EL QUANTITI ESTA DECLARADA EN UNA CLASE LLAMADA CART.CS
                    precio = cart.Producto.Precio
                };

                db.OrdenDetalle.Add(ordenDetalle);
                db.SaveChanges();
            }
            //3.  eliminar la compra session UNA VEZ REALIZADA LA COMPRA
            Session.Remove(strCard);
            return View("OrdenSatisfactoria");
        }

        //--------------------------------TRABAJANDO CON PAY PAL-----------------------------///

        private Payment payment;

        // Creamos un Pago usando el ApiCOntexte que tenemos configurado en los modelos
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };
            List<Cart> listCarts = (List<Cart>)Session[strCard];
            foreach (var cart in listCarts)
            {
                listItems.items.Add(new Item()
                {

                    name = cart.Producto.ProductoName,
                    currency = "USD",
                    price = cart.Producto.Precio.ToString(),
                    quantity = cart.Quantity.ToString(),
                    sku = cart.Producto.ProductoId.ToString()

                });
            }
            var payer = new Payer() { payment_method = "paypal" };
            //
            //se hace la configuración Redireccionar el URL aqui con el objeto RedirectUrls.
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // se crean los detalles del Objeto

            var details = new Details()
            {
                
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.Quantity * x.Producto.Precio).ToString() /// aqui no le agrege el iva 
                // ya que pay pay no acepta calculos repetidos

            };

            // Creamos los cuantas en objetos.
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };

            // Creamos la Transaccion

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Gabriel Naran pruebas con Pay pal",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return payment.Create(apiContext);
        }

        // Crear el ejecutador de Payment Metodo
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);

        }
        // Creamos el metodo Payment con Pay pal 
        public ActionResult PaymentWithPaypal() 
        {
             //Contexto de gettings de las bases de paypal en clientId y clientSecret del pago
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //creando el pago 
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);


                    //se obtiene los enlaces devueltos de la respuesta de PayPal para crear la función de las llamadas
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext()) 
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);

                }
                else
                {
                    //este se ejecutará cuando hayamos recibido todos los pagos de la llamada anterior
                                        var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executePayment.state.ToLower() != "approved") 
                    {
                        return View("Fracaso");
                    }
                
                }

            }
            catch (Exception ex) 
            {
                PayPalLogger.Log("Error: " + ex.Message);
                return View("Fracaso");
            }

            return View("satisfactori");
        }
    }
}