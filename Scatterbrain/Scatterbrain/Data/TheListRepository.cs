using Newtonsoft.Json;
using Scatterbrain.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scatterbrain.Data
{
    public class TheListRepository
    {
        private static SQLiteConnection _db;

        static TheListRepository()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _db = new SQLiteConnection(Path.Combine(path, "ScatterbrainSQLite.db3"));
            var info = _db.GetTableInfo(Constants.TheListTableName);
            if (!info.Any())
            {
                _db.CreateTable<TheList>();
            }
        }

        public static Task<Models.TheList> Read()
        {
            var items = _db.Table<TheList>().ToArray();
            var item = items.FirstOrDefault();
            if (item == null)
            {
                return Task.FromResult<Models.TheList>(null);
            }
            return Task.FromResult(JsonConvert.DeserializeObject<Models.TheList>(item.Content));
        }

        public static Task Write(Models.TheList theList)
        {
            _db.Table<TheList>().Delete(l => true);
            _db.Insert(new TheList { Content = JsonConvert.SerializeObject(theList) });
            return Task.FromResult(0);
        }
    }
}
