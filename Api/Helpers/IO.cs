using Core.Entities;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace Api.Helpers;
public class IO
{


    public Boolean ValidarCabecera(DataColumnCollection dt)
    {

        List<string> headerProduct = new List<string> { "Name", "Price", "Amount" };
        var col = string.Empty;
        if(dt.Count!=3) throw new Exception(string.Format("ERROR en la longitud de la cabecera"));
        
      
        for (int i = 0; i < dt.Count; i++)
        {

            col = dt[i].ColumnName;
            if (headerProduct.Contains(col))
                continue;
            else
            {
                //numError += 1;
                //errorString += "," + col;
                throw new Exception(string.Format("ERROR, La columna: {0}, no esta registrada", col));
            }
        }
        return true;
    }

    public DataTable convertDataTable(IFormFile fileProduct)
    {
        DataTable dataTable = new DataTable();
        using (var streamReader = new StreamReader(fileProduct.OpenReadStream()))
        using (var textFieldParser = new TextFieldParser(streamReader))
        {
            var isFirst = true;
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(";");

            while (!textFieldParser.EndOfData)
            {

                //string[] fields = textFieldParser.ReadFields();
                if (isFirst == true)
                {
                    foreach (var _row in textFieldParser.ReadFields())
                    {
                        dataTable.Columns.Add(_row);
                    }
                    isFirst = false;
                    continue;
                }
                DataColumnCollection dataColumn = dataTable.Columns;
               
                var validHeader = ValidarCabecera(dataColumn);

                string[] fields = textFieldParser.ReadFields();
                DataRow row = dataTable.NewRow();
                row["Name"] = fields[0];
                row["Price"] = fields[1];
                row["Amount"] = fields[2];
                dataTable.Rows.Add(row);
            }
            
        }
        return dataTable;
    }

}

