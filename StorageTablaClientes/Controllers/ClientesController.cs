using StorageTablaClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageTablaClientes.Controllers
{
    public class ClientesController : Controller
    {
        ModeloClientes modelo;
        public ClientesController()
        {
            this.modelo = new ModeloClientes();
        }
        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InsertarCliente( )  
        {            
            return View();
        }
        [HttpPost]
        public ActionResult InsertarCliente(Cliente cliente) //bug de viual : da error en el binding si pones muchos caracteres(examen!!!)
        {
            modelo.InsertarCliente(cliente.IdCliente, cliente.Nombre, cliente.Apellidos, cliente.Empresa);
            return View();
        }
        public ActionResult ClientesEmpresa( )
        {
            
            return View();
        }        

        [HttpPost]
        public ActionResult ClientesEmpresa(string empresa)
        {
            List<Cliente> clientes = modelo.BluscarClientesEmpresa(empresa);
            return View(clientes);
        }
        public ActionResult DetallesCliente(String rowkey,String partitionkey)
        {
            int idcliente = int.Parse(rowkey);
            string empresa = partitionkey;
            Cliente cliente = modelo.BuscarCliente(idcliente, empresa);
            return View(cliente);
        }
        public ActionResult ModificarCliente(string rowkey,string partitionkey)
        {
            int idcliente = int.Parse(rowkey);
            string empresa = partitionkey;
            Cliente cliente = modelo.BuscarCliente(idcliente, empresa);
            return View(cliente);
        }
        [HttpPost]
        public ActionResult ModificarCliente(Cliente cliente)
        {
            modelo.ModificarCliente(cliente.IdCliente, cliente.Nombre, cliente.Apellidos, cliente.Empresa);
            return View(cliente);
        }

        public ActionResult EliminarCliente(string rowkey,string partitionkey)
        {
            int idcliente = int.Parse(rowkey);
            string empresa = partitionkey;
            modelo.EliminarCliente(idcliente, empresa);
            return RedirectToAction("Index");
        }
        public ActionResult ListarClientes()
        {
            List<Cliente> lista = modelo.ListarClientes();

            return View(lista);
        }
    }
}