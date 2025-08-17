using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI_2025_II_IIIP_INVESTIGACION3
{
    public partial class frmServidor : Form
    {
        private Socket servidorSocket;
            private Socket clienteSocket;
        public frmServidor()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                servidorSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                servidorSocket.Bind(new IPEndPoint(IPAddress.Any, 5000));
                servidorSocket.Listen(1);
                servidorSocket.BeginAccept(new AsyncCallback(AceptarConexion), null);
                txtLog.AppendText("Servidor iniciado en el puerto 5000...\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

            private void AceptarConexion(IAsyncResult ar)
            {
               clienteSocket = servidorSocket.EndAccept(ar);
               byte[] buffer = new byte[1024];
               clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecibirDatos), buffer);
               Invoke(new Action(() => txtLog.AppendText("Cliente conectado.\r\n")));
            }
        private void RecibirDatos(IAsyncResult ar)
        {
            try
            {
                int recibido = clienteSocket.EndReceive(ar);
                byte[] buffer = (byte[])ar.AsyncState;
                string mensaje = Encoding.UTF8.GetString(buffer, 0, recibido);

                Invoke(new Action(() => txtLog.AppendText("Cliente: " + mensaje + "\r\n")));

                byte[] data = Encoding.UTF8.GetBytes("Servidor recibió: " + mensaje);
                clienteSocket.Send(data);

                buffer = new byte[1024];
                clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecibirDatos), buffer);
            }
            catch { }
        }


    }
            
    
        
}
