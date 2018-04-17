using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table; //using

namespace StorageTablaClientes.Models
{
    public class Cliente : TableEntity //hereda de TableEntity
    {
        public int IdCliente { get; set; }
        public String _Empresa { get; set; }
        public String Empresa //esta es la forma correcta de inicializarlo
        {
            get {
                return this._Empresa;
            }
            set
            {
                this.PartitionKey = value;
                this._Empresa = value;
            }
        }


        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        //constructor vacio , hay que ponerlo obligatoriamente
        public Cliente() { } //esto es solo la explicacion .. 
        //inicializamos para indicar el rowkey (IdCliente) y partition KEY (Empresa)
        public Cliente(int idcliente,String empresa) //en realidad asi esta mal . la forma correcta arriba.. esto es solo a modo explicativo
        {
            this.RowKey = idcliente.ToString();
            this.PartitionKey = empresa;
            this.IdCliente = idcliente;
            this.Empresa = empresa;
        }

    }
}