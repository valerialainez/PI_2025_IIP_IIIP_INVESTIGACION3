using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI_2025_II_IIIP_INVESTIGACION3
{
    public partial class frmCliente : Form
    {
        private Socket clienteSocket;

        public frmCliente()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = txtLog.Text;
                byte[] data = Encoding.UTF8.GetBytes(mensaje);
                clienteSocket.Send(data);
                txtLog.AppendText("Yo: " + mensaje + "\r\n");
                txtLog.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                clienteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clienteSocket.Connect("127.0.0.1", 5000);
                txtLog.AppendText("Conectado al servidor.\r\n");

                byte[] buffer = new byte[1024];
                clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecibirDatos), buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void RecibirDatos(IAsyncResult ar)
        {
            try
            {
                int recibido = clienteSocket.EndReceive(ar);
                byte[] buffer = (byte[])ar.AsyncState;
                string mensaje = Encoding.UTF8.GetString(buffer, 0, recibido);

                Invoke(new Action(() => txtLog.AppendText("Servidor: " + mensaje + "\r\n")));

                buffer = new byte[1024];
                clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecibirDatos), buffer);
            }
            catch { }
        }


    }
}
