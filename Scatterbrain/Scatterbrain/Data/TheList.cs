using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scatterbrain.Data
{
    [Table(Constants.TheListTableName)]
    public class TheList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
