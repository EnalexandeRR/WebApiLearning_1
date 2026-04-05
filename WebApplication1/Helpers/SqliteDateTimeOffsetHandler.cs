using System.Data;
using Dapper;

namespace MyWebApp;

public class SqliteDateTimeOffsetHandler: SqlMapper.TypeHandler<DateTimeOffset>
{
    // Как сохранять в базу (DateTimeOffset -> String)
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
        // Превращаем дату в идеальную ISO 8601 строку (буква "O")
        parameter.Value = value.ToUniversalTime().ToString("O"); 
    }

    // Как читать из базы (String -> DateTimeOffset)
    public override DateTimeOffset Parse(object value)
    {
        // Dapper дает нам объект, мы точно знаем, что в SQLite это строка
        return DateTimeOffset.Parse((string)value);
    }
}