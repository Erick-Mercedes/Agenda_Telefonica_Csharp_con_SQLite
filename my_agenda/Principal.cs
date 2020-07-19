using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace my_agenda
{
    public partial class Principal : Form
    {
        private int id;
        agenda age = new agenda();
        DataTable dt;

        private void consultar()
        {
            dgvAgenda.DataSource = dt = age.consultar();
        }

        private void obtenerId()
        {
            id = Convert.ToInt32(dgvAgenda.CurrentRow.Cells["id"].Value);
        }

        private void obtenerDatos()
        {
            obtenerId();
            txtNombre.Text = dgvAgenda.CurrentRow.Cells["nombre"].Value.ToString();
            txtTelefono.Text = dgvAgenda.CurrentRow.Cells["Telefono"].Value.ToString();
        }

        private void restablecerControles()
        {
            this.txtNombre.Clear();
            this.txtTelefono.Clear();
            this.txtFiltral.Clear();
            this.btnEliminar.Enabled = false;
            this.btnModificar.Enabled = false;
        }

        public Principal()
        {
            InitializeComponent();
            restablecerControles();
            consultar();
            dgvAgenda.Columns["id"].Visible = false;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            bool rs = age.insertar(txtNombre.Text, txtTelefono.Text);

            if (rs)
            {
                MessageBox.Show("Su registro ha sido insertado correctamente");
            }
            restablecerControles();
            consultar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            bool rs = age.actualizar(id, txtNombre.Text, txtTelefono.Text);

            if (rs)
            {
                MessageBox.Show("Su registro ha sido actualizado correctamente");
                consultar();
            }
            restablecerControles();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult r =
                MessageBox.Show("Eliminar",
                "Estas seguro que deseas Eliminar este registro ?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (r == DialogResult.OK)
            {
                bool rs = age.eliminar(id);

                if (rs)
                {
                    MessageBox.Show("Su registro ha sido eliminado correctamentea");
                }
                restablecerControles();
            }
        }

        private void txtFiltral_TextChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = $"Nombre LIKE'%{txtFiltral.Text}%'";
        }

        private void dgvAgenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvAgenda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            restablecerControles();
            obtenerId();
            this.btnEliminar.Enabled = true;
        }

        private void dgvAgenda_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            obtenerDatos();
            this.btnEliminar.Enabled = false;
            this.btnModificar.Enabled = true;
        }
    }
}
