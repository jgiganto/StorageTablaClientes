using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageTablaClientes.Models
{
    public class ModeloClientes
    {
        CloudStorageAccount cuenta;
        public ModeloClientes()
        {
            String claves =
                CloudConfigurationManager.GetSetting("cuentastorage");
            cuenta = CloudStorageAccount.Parse(claves);
        }
        public CloudTable GetTablaCliente()
        {
            CloudTableClient cliente = cuenta.CreateCloudTableClient(); //1 creo cliente
            CloudTable tabla = cliente.GetTableReference("clientes"); //2 accedo a la tabla usando el cliente
            tabla.CreateIfNotExists(); //3creo si no existe
            return tabla; //devuelvo
        }

        public void InsertarCliente(int idcliente,String nombre, String apellidos, String empresa)
        {
            CloudTable tabla = this.GetTablaCliente();
            Cliente cli = new Cliente(idcliente, empresa);
            //Cliente cli = new Cliente();
            cli.Nombre = nombre;
            cli.Apellidos = apellidos;
            //cli.Empresa = empresa;
            //cli.IdCliente = idcliente;
            //creamos una operacion en la tlabla
            TableOperation insert = TableOperation.Insert(cli);
            //almacenamos la accion en la tabla
            tabla.Execute(insert);
        }

        public Cliente BuscarCliente(int idcliente,String empresa)
        {
            string rowkey = idcliente.ToString();
            string partitionkey = empresa;
            CloudTable tabla = this.GetTablaCliente();
            //si deseamos buscar por un determinado objeto dentro de CloudTable(pk)
            //debemos hacerlo por sus dos Keys
            TableOperation select =
                TableOperation.Retrieve<Cliente>(partitionkey,rowkey);
            TableResult resultado = tabla.Execute(select);
            if (resultado.Result == null)
            {
                return null;
            }
            else
            {
                Cliente cliente = resultado.Result as Cliente;
                return cliente;
            }           

        }
        //metodo para buscar por empresas(partitionKey)
        public List<Cliente> BluscarClientesEmpresa(String empresa)
        {
            CloudTable tabla = this.GetTablaCliente();
            //para buscar por un determinado campo
            //se realiza mendiante tablequery
            TableQuery<Cliente> consulta = new TableQuery<Cliente>();
            //consulta = (TableQuery<Cliente>)consulta.Where(z => z.Empresa == empresa);
            //debemos indicar que recupere los datos de la tabla
            //en realidad la consulta es un filtro
            List<Cliente> clientes =
                tabla.ExecuteQuery(consulta).Where(z => z.Empresa == empresa).ToList();

            if (clientes==null)
            {
                return null;
            }
            else
            {
                return clientes;
            }

        }
        public void ModificarCliente(int idcliente,String nombre,String apellidos,String empresa)
        {
            Cliente cliente = this.BuscarCliente(idcliente, empresa);
            cliente.Nombre = nombre;
            cliente.Apellidos = apellidos;
            cliente.Empresa = empresa;
            TableOperation update =
                TableOperation.Replace(cliente);
            CloudTable tabla = this.GetTablaCliente();
            tabla.Execute(update);
        }

        public void EliminarCliente(int idcliente,String empresa)
        {
            CloudTable tabla = this.GetTablaCliente();
            Cliente cliente = this.BuscarCliente(idcliente, empresa);
            TableOperation delete = TableOperation.Delete(cliente);
            tabla.Execute(delete);
        }

        public List<Cliente> ListarClientes()
        {
            CloudTable tabla = this.GetTablaCliente();
            TableQuery<Cliente> consulta = new TableQuery<Cliente>();
            List<Cliente> clientes =
                tabla.ExecuteQuery(consulta).ToList();

            if (clientes == null)
            {
                return null;
            }
            else
            {
                return clientes;
            }

        }
    }
}