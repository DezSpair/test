using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlWebService.Contracts;
using XmlWebService.Contracts.Enums;
using XmlWebService.Contracts.Models;

namespace XmlWebService.Data
{
    public class DataService : IDataRepository
    {
        public bool CheckOrCreateDb(string dbName)
        {
            if (File.Exists(dbName))
                return true;
            bool result = false;
            try
            {
                SQLiteConnection.CreateFile(dbName);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
            }
            return result;
        }

        public void CheckTable(IEnumerable<TableModel> tableModels, string dbName)
        {
            var currentTable = GetAllTableName(dbName).ToList();
            foreach (var nTable in tableModels)
            {
                var isTable = false;
                foreach (var cTable in currentTable)
                {
                    if (String.Equals(nTable.TableName, cTable))
                        isTable = true;
                    break;
                }

                if (isTable) continue;
                var rows = nTable.Records.Select(p => p.RowModels).ToList();
                var fields = rows.SelectMany(r => r).Select(r => new { r.FieldName, r.FieldType }).Distinct().ToDictionary(row => row.FieldName, row => row.FieldType);

                CreateTable(dbName, nTable.TableName, fields.Distinct().ToDictionary(k => k.Key, v => v.Value));
            }
        }

        public bool CreateTable(string dbName, string tableName, Dictionary<string, string> fieldsAndType)
        {
            bool result = false;
            string stringFields = string.Empty;
            stringFields = fieldsAndType.Aggregate(stringFields, (current, p) => current + (string.IsNullOrEmpty(current) ? string.Format("[{0}] {1}", p.Key, GetFieldType(p.Value)) : string.Format(" ,[{0}] {1}", p.Key, GetFieldType(p.Value))));


            string createTable = string.Format(@"CREATE TABLE {0} ({1})", tableName, stringFields);
            var connection = new SQLiteConnection(string.Format("Data Source={0};", dbName));
            var command = new SQLiteCommand(createTable, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public List<string> GetAllTableName(string dbName)
        {
            var result = new List<string>();
            var connection =
                new SQLiteConnection(string.Format("Data Source={0};", dbName));
            var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;",
                connection);
            try
            {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                result.AddRange(from DbDataRecord record in reader select record["name"].ToString());
            }
            catch (Exception)
            {
                result = new List<string>();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public void AddRecord(string dbName, ref List<TableModel> tableModels)
        {
            using (var connection = new SQLiteConnection(string.Format("Data Source={0};", dbName)))
            {
                connection.Open();
                foreach (var t in tableModels)
                {

                    foreach (var r in t.Records)
                    {
                        var insertComand = string.Format("INSERT INTO {0}", t.TableName);
                        var fields = string.Empty;
                        var values = string.Empty;
                        foreach (var dic in r.RowModels.ToDictionary(k => k.FieldName, v => v.FieldValue))
                        {
                            fields += string.IsNullOrEmpty(fields) ? dic.Key : " ," + dic.Key;
                            values += string.IsNullOrEmpty(values)
                                ? string.Format("'{0}'", dic.Value)
                                : string.Format(" , '{0}'", dic.Value);
                        }
                        insertComand = string.Format("{0} ({1}) values ({2});", insertComand, fields, values);
                        try
                        {
                            var command = new SQLiteCommand(insertComand, connection);
                            command.ExecuteNonQuery();
                            r.RowStatus = RowStatus.Inserted;
                        }
                        catch (Exception ex)
                        {

                            r.RowStatus = RowStatus.Error;
                            r.ErrorMsg = ex.Message;
                        }
                    }
                    
                }
                
            }
        }

        private string GetFieldType(string tempType)
        {
            string type;
            switch (tempType.ToLower())
            {
                case "text":
                    type = "TEXT";
                    break;
                case "integer":
                    type = "INTEGER";
                    break;
                case "real":
                    type = "REAL";
                    break;
                case "blob":
                    type = "BLOB";
                    break;
                case "datetime":
                    type = "DATETIME";
                    break;
                default:
                    type = "TEXT";
                    break;
            }
            return type;
        }
    }
}
