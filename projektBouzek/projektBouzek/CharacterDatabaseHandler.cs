using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace projektBouzek
{
	internal class CharacterDatabaseHandler
	{		
		private readonly string characterTableName;
		private readonly string connectionString;
		public CharacterDatabaseHandler(string connectionString, string characterTableName)
		{
			this.connectionString = connectionString;
			this.characterTableName = characterTableName;
			using (SqlConnection connection = new(connectionString))
			{
				connection.Open();
				Console.WriteLine("database OPEN");

				SqlCommand commandRecordCounts = new SqlCommand();
				commandRecordCounts.Connection = connection;
				commandRecordCounts.CommandText = $"SELECT COUNT(*) FROM [{characterTableName}]";
				var rowCount = commandRecordCounts.ExecuteScalar();
				Console.WriteLine("number of characters in database: " + (int)rowCount);
			}
		}
		public void GetAllCharacters(int id)
		{
			using (SqlConnection connection = new(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.Parameters.AddWithValue("@id", id);
				command.CommandText =	@"SELECT [Character].[Name], [Character].[Level], [ClassTable].[ClassName]
										FROM [Character]
										JOIN [ClassTable] ON [Character].[ClassId] = [ClassTable].[Id]
										WHERE [UserId]=@id";
				SqlDataReader data = command.ExecuteReader();
                while (data.Read())
                {
                    Console.WriteLine("postava jmenem " + data[0] + " level " + data[1] + " trida " + data[2]);
                }
			}
			
			

		}
	}
}
