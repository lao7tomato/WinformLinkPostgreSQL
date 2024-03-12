using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace WindowsFormsApp2
{
    public partial class TableSelectionForm : Form
    {
        //public string SelectedTable { get; private set; }
        public string SelectedDatabase { get; set; }
        public DataTable tableList { get; set; }

        public event EventHandler<TableSelectedEventArgs> TableSelected;
        
        public TableSelectionForm()
        {
            InitializeComponent();
           
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            string selectedSchema = comboBox1.Text;
            string[] selectedTables = checkedListBox1.CheckedItems.Cast<string>().ToArray();

            if (selectedTables.Length == 0)
            {
                MessageBox.Show("请选择一个表。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 触发 TableSelected 事件
                TableSelected?.Invoke(this, new TableSelectedEventArgs(selectedSchema, selectedTables));

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSchema = comboBox1.SelectedValue?.ToString();

            if (!string.IsNullOrEmpty(selectedSchema))
            {
                checkedListBox1.Items.Clear();
                string connectionString = $"Host=localhost;Port=5432;Database={SelectedDatabase};Username=postgres;Password=yly23333";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        DataTable tables = connection.GetSchema("Tables");

                        var tablesInSchema = tables.AsEnumerable()
                            .Where(row => row.Field<string>("TABLE_SCHEMA").Equals(selectedSchema, StringComparison.InvariantCultureIgnoreCase) &&
                                          !row.Field<string>("TABLE_NAME").Equals("spatial_ref_sys", StringComparison.InvariantCultureIgnoreCase))
                            .Select(row => row.Field<string>("TABLE_NAME"))
                            .ToArray();

                        checkedListBox1.Items.AddRange(tablesInSchema);
                    }
                    catch (NpgsqlException ex)
                    {
                        MessageBox.Show("错误获取表信息: " + ex.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public void UpdateSchemaList(DataTable newSchemaList)
        {
            // 更新 comboBox 数据源
            comboBox1.DataSource = newSchemaList;
            // ... 其他必要的更新
        }
        public void SetSelectedDatabase(string database, string connectionString)
        {
            SelectedDatabase = database;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable newSchemaList = GetNewSchemaList(connection);
                    if (newSchemaList != null && newSchemaList.Rows.Count > 0)
                    {
                        comboBox1.DataSource = newSchemaList;
                        comboBox1.DisplayMember = "SCHEMA_NAME";
                        comboBox1.ValueMember = "SCHEMA_NAME";
                    }
                    else
                    {
                        MessageBox.Show("未找到任何 schema。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("无法连接到数据库: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public DataTable GetNewSchemaList(NpgsqlConnection connection)
        {
            DataTable schemaList = new DataTable();
            schemaList.Columns.Add("SCHEMA_NAME", typeof(string));

            string query = "SELECT schema_name FROM information_schema.schemata WHERE schema_name NOT IN ('pg_catalog', 'information_schema');";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string schemaName = reader.GetString(0);
                        schemaList.Rows.Add(schemaName);
                    }
                }
            }

            return schemaList;
        }
    }
    public class TableSelectedEventArgs : EventArgs
    {
        public string SelectedSchema { get; }
        public string[] SelectedTables { get; }

        public TableSelectedEventArgs(string selectedSchema, string[] selectedTables)
        {
            SelectedSchema = selectedSchema;
            SelectedTables = selectedTables;
        }
    }

}
