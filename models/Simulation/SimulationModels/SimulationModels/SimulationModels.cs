using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace brt.Models.Simulation
{
    public class ModelBase
    {
        public ModelBase()
        {
            id = Guid.NewGuid().ToString();
            cachettl = 1;
        }

        public ModelBase(int _cachettl)
        {
            id = Guid.NewGuid().ToString();
            cachettl = _cachettl;
        }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public int cachettl { get; set; }
    }

    public class DataColumn
    {
        public DataColumn()
        {
            columnName = string.Empty;
            dataValue = 0.0d;
        }

        public string columnName { get; set; }
        public double dataValue { get; set; }
    }

    public class DataRow : ModelBase
    {
        public DataRow()
        {
            datasetId = string.Empty;
            rowNumber = 0;
            columns = new List<DataColumn>();
        }

        public string datasetId { get; set; }
        public int rowNumber { get; set; }
        public List<DataColumn> columns { get; set; }
    }

    public class DataSet : ModelBase
    {
        public DataSet()
        {
            name = string.Empty;
            rows = new List<string>();
        }

        public string name { get; set; }
        public List<string> rows { get; set; }
    }
}
