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
        //private static SQLiteAsyncConnection _db;

        private static string _path;

        static TheListRepository()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _path = Path.Combine(path, "Scatterbrain.dat");
            //_db = new SQLiteAsyncConnection(Path.Combine(path, "ScatterbrainSQLite.db3"));
            //var info = _db.GetTableInfoAsync(Constants.TheListTableName).GetAwaiter().GetResult();
            //if (!info.Any())
            //{
            //    _db.CreateTableAsync<TheList>().GetAwaiter().GetResult();
            //}
        }

        public static Task<Models.TheList> Read()
        {
            //var items = await _db.Table<TheList>().ToArrayAsync();
            //var item = items.FirstOrDefault();
            //if (item == null)
            //{
            //    return null;
            //}
            //return JsonConvert.DeserializeObject<Models.TheList>(item.Content);
            if (!File.Exists(_path))
            {
                return Task.FromResult<Models.TheList>(null);
            }
            return Task.FromResult(JsonConvert.DeserializeObject<Models.TheList>(File.ReadAllText(_path)));
        }

        public static Task Write(Models.TheList theList)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(theList));
            return Task.FromResult(0);
            //await _db.Table<TheList>().DeleteAsync();
            //await _db.InsertAsync(new TheList { Content = JsonConvert.SerializeObject(theList) });
        }
    }
}
