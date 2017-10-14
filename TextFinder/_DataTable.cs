using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TextFinder {
    class _DataTable: DataTable {
        DataColumn dc;
        public _DataTable() {
            dc = new DataColumn();
            dc.ColumnName = "sentence";
            dc.DataType = typeof(string);
            this.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "value";
            dc.DataType = typeof(double);
            this.Columns.Add(dc);
        }

        public DataTable sortByColumn(string column) {
            if (this.Rows.Count > 0)
                return this.AsEnumerable()
                  .OrderBy(row => row.Field<object>(column))
                  .CopyToDataTable();
            else 
                return this;
        }
    }
}
