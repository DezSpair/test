using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlWebService.Contracts.Models;

namespace XmlWebService.Contracts
{
    public interface IDataRepository
    {
        bool CheckOrCreateDb(string dbName);
        void CheckTable(IEnumerable<TableModel> tableModels, string dbName);
        bool CreateTable(string dbName, string tableName, Dictionary<string, string> fieldsAndType);

        List<string> GetAllTableName(string dbName);
        void AddRecord(string dbName, ref List<TableModel> tableModels);
    }
}
