using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektBouzek
{
	internal class DatabaseManager
	{
		private const string characterTableName = "Character";
			string databaseName = "Database1.mdf";
		CharacterDatabaseHandler characterDatabaseHandler;
		string connectionString;

		public DatabaseManager()
		{
			string actualDir = Directory.GetCurrentDirectory();
			string databaseDir = actualDir;
			//go 3 levels up -> database directory
			for (int i = 0; i < 3; i++)
			{
				DirectoryInfo parentDirectory = Directory.GetParent(databaseDir);
				databaseDir = parentDirectory.FullName;
			}
			databaseDir += "\\" + databaseName;
			SqlConnectionStringBuilder sqlonnectionString = new SqlConnectionStringBuilder();
			sqlonnectionString.DataSource = @"(LocalDB)\MSSQLLocalDB";
			sqlonnectionString.AttachDBFilename = databaseDir;
			sqlonnectionString.IntegratedSecurity = true;

			Console.WriteLine($"complete path used for database connection: {databaseDir}");
			Console.Write("connecting to database");
			
			connectionString = sqlonnectionString.ConnectionString;
            Console.WriteLine("1st connection " + connectionString);
            characterDatabaseHandler = new(connectionString, characterTableName);
		}

		public void GetCharactersDataInfo(string loginName, string loginPassword)
		{
            object id = null;
			
			using(SqlConnection connection = new(connectionString))
			{
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.CommandText = "SELECT [Id] FROM [UsersTable] WHERE [UserName]=@loginName AND [Password]=@loginPassword";
				command.Parameters.AddWithValue("@LoginName", loginName);
				command.Parameters.AddWithValue("@LoginPassword", loginPassword);
				connection.Open();
				id = command.ExecuteScalar();
			}
			
			if(id != null)
			{
                Console.WriteLine("user id: " + Convert.ToInt32(id));
                characterDatabaseHandler.GetAllCharacters(Convert.ToInt32(id));
			}
            else
            {
                Console.WriteLine("no id found");
            }
        }
	}
}
