using DotSpatial.Controls;
using DotSpatial.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using DotSpatial.Symbology;
using DotSpatial.Data.Forms;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NPinyin;
using System.Text.RegularExpressions;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 填充数据库名称到 comboBox1
            ComboBox1.Items.Add("winformlink");
            ComboBox1.Items.Add("4326");
        }

        private string selectedDb;
        private string connectionString = "Host=localhost;Port=5432;Database=winformlink;Username=postgres;Password=yly23333";
        private List<string> temporaryShpFiles = new List<string>();
        public string ConvertToPinyin(string chinese)
{
    // 将中文转换为拼音
    string pinyin = Pinyin.GetPinyin(chinese);

    // 替换掉所有不是字母、数字或下划线的字符
    pinyin = Regex.Replace(pinyin, "[^a-zA-Z0-9_]", "");

    return pinyin.ToLower();
}
        private void InPut_Click(object sender, EventArgs e)
        {
            if (ComboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(ComboBox1.SelectedItem.ToString()))
            {
                MessageBox.Show("请先选择你需要导入的数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取选中的图层
            IFeatureLayer selectedLayer = map1.Layers.OfType<IFeatureLayer>().FirstOrDefault(l => l.IsSelected);

            if (selectedLayer != null)
            {
                string chineseName = selectedLayer.LegendText; // 原始中文名称
                string pinyinName = ConvertToPinyin(chineseName); // 转换为拼音

                // 临时目录用于存储转换后的 Shapefile 文件
                string tempDir = Path.Combine(Path.GetTempPath(), "DotSpatialExport");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                try
                {
                    IFeatureSet featureSet = selectedLayer.DataSet as IFeatureSet;
                    if (featureSet != null)
                    {
                        // 生成临时 Shapefile 文件路径
                        string tempShapefilePath = Path.Combine(tempDir, $"{pinyinName}.shp");

                        // 保存数据集为 Shapefile 文件
                        featureSet.SaveAs(tempShapefilePath, true);

                        // 注意：这里我们使用了拼音名称的临时文件路径
                        ImportShapefileToPostgreSQL(tempShapefilePath);

                        MessageBox.Show("导入成功完成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                finally
                {
                    // 清理临时目录
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);
                }
            }
            else
            {
                MessageBox.Show("请选择要导入的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        // 使用 shp2pgsql 工具导入 Shapefile 到 PostgreSQL 数据库
        private void ImportShapefileToPostgreSQL(string shapefilePath)
        {
            string schema = "public";
            bool dropIfExists = true;
            string table = Path.GetFileNameWithoutExtension(shapefilePath).ToLower();

            string args = string.Format("{0} {1}.{2}", shapefilePath, schema, table);
            ProcessStartInfo psi = new ProcessStartInfo("shp2pgsql.exe", args);
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            string sql = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (dropIfExists) sql = sql.Replace("CREATE TABLE", string.Format("DROP TABLE IF EXISTS \"{0}\".\"{1}\";\r\nCREATE TABLE", schema, table));
            connectionString = $"Host=localhost;Port=5432;Database={selectedDb};Username=postgres;Password=yly23333";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                new NpgsqlCommand(sql, connection).ExecuteNonQuery();
            }
        }
        //
        //
        //导出部分
        private void OutBrowse_Click(object sender, EventArgs e)
        {
            string selectedDb = ComboBox1.SelectedItem.ToString();
            string connectionString = $"Host=localhost;Port=5432;Database={selectedDb};Username=postgres;Password=yly23333";

            TableSelectionForm tableSelectionForm = new TableSelectionForm();
            tableSelectionForm.SetSelectedDatabase(selectedDb, connectionString);
            tableSelectionForm.TableSelected += TableSelectionForm_TableSelected; // 确保订阅事件
            DialogResult result = tableSelectionForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // 如果需要，这里可以执行一些操作，例如刷新界面或显示消息
            }

        }
        private DataTable LoadSchemasFromPostgreSQL()
        {
            DataTable schemaList = new DataTable();
            schemaList.Columns.Add("SCHEMA_NAME", typeof(string));

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                DataTable schemaTable = connection.GetSchema("Tables");

                var schemas = schemaTable.AsEnumerable()
                    .Select(row => row.Field<string>("TABLE_SCHEMA"))
                    .Distinct();

                foreach (var schema in schemas)
                {
                    schemaList.Rows.Add(schema);
                }
            }

            return schemaList;
        }
       
        
        private void TableSelectionForm_TableSelected(object sender, TableSelectedEventArgs e)
        {
            string selectedSchema = e.SelectedSchema;
            string[] selectedTables = e.SelectedTables;

            // 循环处理每个表
            foreach (string selectedTable in selectedTables)
            {
                // 导出并获取临时 shp 文件
                string temporaryShpFile = ExportToShapefile(selectedTable);

                if (File.Exists(temporaryShpFile))
                // 检查文件是否存在
                {
                    // 将临时 shp 文件路径加入列表
                    temporaryShpFiles.Add(temporaryShpFile);

                    // 使用 AddLayer 导入地图
                    map1.AddLayer(temporaryShpFile);
                }
                else
                {
                    MessageBox.Show($"未能找到文件：{temporaryShpFile}", "文件不存在", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 刷新地图
            map1.Refresh();
        }
        private string ExportToShapefile(string tableName)
        {
            string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            string tempShapefilePath = Path.Combine(tempDir, $"{tableName}.shp");

            // 构建 pgsql2shp 命令
            string connectionString = connectionString = $"host=localhost;port=5432;dbname={selectedDb};user=postgres;password=yly23333";
            string connectionString1 = connectionString.Replace(";", " ");
            string args = $"-f \"{tempShapefilePath}\" \"{connectionString1}\" \"SELECT * FROM \\\"{tableName}\\\"\"";
            Console.WriteLine(args);
            ProcessStartInfo psi = new ProcessStartInfo("pgsql2shp.exe", args)
            {
                RedirectStandardOutput = true, // 重定向标准输出
                RedirectStandardError = true,  // 重定向标准错误s
                UseShellExecute = false        // 不使用外壳程序执行
            };

            try
            {
                using (Process p = Process.Start(psi))
                {
                    // 读取标准输出
                    string output = p.StandardOutput.ReadToEnd();
                    // 读取错误输出
                    string errorOutput = p.StandardError.ReadToEnd();

                    p.WaitForExit();

                    // 检查导出是否成功
                    if (p.ExitCode == 0 && File.Exists(tempShapefilePath))
                    {
                        return tempShapefilePath;
                    }
                    else
                    {
                        // 如果有错误输出，可以在这里处理
                        if (!string.IsNullOrEmpty(errorOutput))
                        {
                            MessageBox.Show($"导出过程中出现错误: {errorOutput}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理异常信息
                MessageBox.Show($"导出时发生异常: {ex.Message}", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 导出失败时，返回空字符串或者执行其他错误处理
                return string.Empty;
            }
        }
        private TableSelectionForm tableSelectionForm;
        private void Test_Click(object sender, EventArgs e)
        {
            selectedDb = ComboBox1.SelectedItem.ToString();
            connectionString = $"Host=localhost;Port=5432;Database={selectedDb};Username=postgres;Password=yly23333";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show($"已经连接到数据库 {selectedDb}", "连接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 这里假设你已经有方法来获取新的 schema 列表
                    DataTable newSchemaList = GetNewSchemaList(connection);

                    // 创建 TableSelectionForm 实例
                    if (tableSelectionForm == null || tableSelectionForm.IsDisposed)
                    {
                        tableSelectionForm = new TableSelectionForm();
                    }
                    else
                    {
                        // 如果实例已经存在，只需要更新 schema 列表
                        tableSelectionForm.UpdateSchemaList(newSchemaList);
                    }

                    // 更新 TableSelectionForm 的 schema 列表
                    tableSelectionForm.UpdateSchemaList(newSchemaList);

                    // 可以选择显示或重新显示 TableSelectionForm
                    // tableSelectionForm.Show();
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("连接失败: " + ex.Message, "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //private void toolStripButton1_Click(object sender, EventArgs e)
        //{
        //    map1.AddLayer(@"E:\大创\c#Test\低速高导体埋深线.shp");
        //}

    }
}
