using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace my_agenda
{
    class agenda
    {
        private SQLiteConnection cn = null; //Para la conexion.
        private SQLiteCommand cmd = null; //Para ejecutar comandos SQLite.
        private SQLiteDataReader reader = null; //Para almacenar los datos.
        private DataTable table = null; //Para organizar la informacion recibida.

        //Metodo para darle nombres a las columnas.
        private void nombresColumnas()
        {
            table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Nombre");
            table.Columns.Add("Telefono");
        }

        //Metodos para insertar la base de datos.
        public bool insertar(string nombre, string telefono)
        {
            try
            {
                string query = "INSERT INTO directorio ( nombre , telefono ) VALUES ('" + nombre + "','" + telefono + "')";
                cn = conexion.conectar();
                cn.Open();
                cmd = new SQLiteCommand(query, cn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return false;
        }

        //Metodo para consultar.
        public DataTable consultar()
        {
            try
            {
                nombresColumnas();
                string query = "SELECT * FROM directorio";
                cn = conexion.conectar();
                cn.Open();
                cmd = new SQLiteCommand(query, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    table.Rows.Add(new object[] { reader["id"], reader["nombre"], reader["telefono"] });
                }
                reader.Close();
                cn.Close();
                return table;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return table;
        }

        //Metodo para Eliminar.
        public bool eliminar(int id)
        {
            try
            {
                string query = "DELETE FROM directorio WHERE id='" + id + "'";
                cn = conexion.conectar();
                cn.Open();
                cmd = new SQLiteCommand(query, cn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    cn.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "ocurrio un ERROR en el proceso");
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return false;
        }

        //Metodo para Filtrar.
        public DataTable filtrar (string filtro)
        {
            string query = $"SELECT * FROM directorio WHERE nombre LIKE '%{filtro} %' OR Telefono LIKE '%{filtro} %'";
            return ExecuteReader(query);
        }

        private DataTable ExecuteReader(string query)
        {
            try
            {
                nombresColumnas();
                cn = conexion.conectar();
                cmd = new SQLiteCommand(query, cn);

                reader = cmd.ExecuteReader();

                int c = 0;
                while (reader.Read())
                {
                    c++;
                    object[] fila = new object[] { reader["id"], c, reader["nombre"], reader["telefono"] };
                    table.Rows.Add(fila);
                }
                reader.Close();
                return table;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + " " + e.StackTrace);
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return table;
        }

        //Metodo para actualizar.
        public bool actualizar(int id, string nombre, string telefono)
        {
            try
            {
                string query = "UPDATE directorio SET nombre ='" + nombre + "', telefono ='" + telefono + "' WHERE id =" + id;
                System.Windows.Forms.MessageBox.Show(query);
                cn = conexion.conectar();
                cn.Open();
                cmd = new SQLiteCommand(query, cn);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    cn.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Ocurrio un Errror en el proceso");
            }
            finally
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return false;
        }
    }

}
