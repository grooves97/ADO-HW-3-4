using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;

            string sql = @"SELECT [name] FROM sys.tables";
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["appShop"].ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                comboBox1.Items.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            connection.Close();

            // TODO: данная строка кода позволяет загрузить данные в таблицу "databaseDataSet.Sale". При необходимости она может быть перемещена или удалена.
            this.saleTableAdapter.Fill(this.databaseDataSet.Sale);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "databaseDataSet.Customers". При необходимости она может быть перемещена или удалена.
            this.customersTableAdapter.Fill(this.databaseDataSet.Customers);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Visible = true;
            LoadTable();
        }

        private void LoadTable()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            var tableName = comboBox1.AccessibilityObject.Value;

            SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["appShop"].ConnectionString);

            myConnection.Open();

            string query = $"SELECT * FROM [{tableName}]";

            SqlCommand command = new SqlCommand(query, myConnection);

            SqlDataReader reader = command.ExecuteReader();

            for (var i = 0; i < reader.FieldCount;i++)
            {
                dataGridView1.Columns.Add(reader.GetName(i), reader.GetName(i));
            }

            while (reader.Read())
            {
                List<string> data = new List<string>();
                for(var j = 0; j < reader.FieldCount; j++)
                {
                    data.Add(reader[j].ToString());
                }
                dataGridView1.Rows.Add(data.ToArray());
            }
            reader.Close();
            myConnection.Close();
        }
    }
}
