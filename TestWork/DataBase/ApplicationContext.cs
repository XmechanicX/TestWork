using System;
using System.Data.SQLite;

namespace TestWork.DataBase
{
    public class ApplicationContext
    {
        public const string ConnectionString = "Data Source=DataBaseForATM.db;";
        public ApplicationContext()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            connection.Close();

            CreateUsersTable();
            CreateAccountsTable();
        }

        public bool CreateAccountsTable()
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText =
                            "CREATE TABLE Accounts(" +
                            "Id    INTEGER NOT NULL UNIQUE," +
                            "CardNumber    INTEGER NOT NULL UNIQUE," +
                            "PinCode   INTEGER NOT NULL," +
                            "CVV   INTEGER NOT NULL," +
                            "Year  NUMERIC NOT NULL," +
                            "User  INTEGER NOT NULL," +
                            "Balance TEXT NOT NULL," +
                            "Status TEXT NOT NULL," +
                            "FOREIGN KEY(\"User\") REFERENCES \"Users\"(\"Id\") ON DELETE SET NULL," +
                            "PRIMARY KEY(\"Id\" AUTOINCREMENT)); " +
                            "INSERT INTO Accounts VALUES " +
                            "(1, 4129866698255835, 9999, 900, 2026,\t1, 600.5, 'Active')," +
                            "(2, 4009038346203527, 3333, 203, 2024,\t2, 2000.3, 'Blocked'),"+
                            "(3, 4384784366210323, 2222, 201, 2025,\t2, 5000.3, 'Active'),"+
                            "(4, 4713701458568352, 1111, 200, 2024,\t2, 7000.3, 'Active');";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }
            finally { @return = true; }

            return @return;
        }

        public bool CreateUsersTable()
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText =
                            "CREATE TABLE Users(" +
                                "Id    INTEGER NOT NULL UNIQUE," +
                                "FirstName TEXT NOT NULL," +
                                "LastName  TEXT NOT NULL," +
                                "SurName   TEXT NOT NULL," +
                                "PRIMARY KEY(\"Id\" AUTOINCREMENT));" +
                                "INSERT INTO Users VALUES " +
                                "(1,'Peter', 'Petrov', 'Petrovich')," +
                                "(2,'Ivan', 'Ivanov', 'Ivanovich');";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }
            finally { @return = true; }

            return @return;
        }

        public Account GetCardNumber(string cardNumber)
        {
            Account @return = null;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"SELECT * from Accounts WHERE CardNumber = '{cardNumber}'"; ;

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            @return = new Account();
                            @return.Id = int.Parse(reader["Id"].ToString());
                            @return.CardNumber = reader["CardNumber"].ToString();

                            object StatucAccounts = reader["Status"].ToString();
                            StatusAccount statusAccount;
                            Enum.TryParse<StatusAccount>(StatucAccounts.ToString(), out statusAccount);
                            @return.StatusAccounts = statusAccount;
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex) { }
            finally { }

            return @return;
        }

        public bool GetAccountInfo(Account account)
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"SELECT * from Accounts WHERE CardNumber = '{account.CardNumber}'"; ;

                        var reader = command.ExecuteReader();
                        reader.Read();

                        account.Id = int.Parse(reader["Id"].ToString());
                        account.CardNumber = reader["CardNumber"].ToString();

                        string balance = reader["Balance"].ToString().Replace('.', ',');
                        account.Balance = double.Parse(balance);

                        object StatucAccounts = reader["Status"].ToString();
                        StatusAccount statusAccount;
                        Enum.TryParse<StatusAccount>(StatucAccounts.ToString(), out statusAccount);

                        account.StatusAccounts = statusAccount; 
                        reader.Close();
                    }
                }
            }
            catch (Exception ex) { }
            finally { @return = true; }

            return @return;
        }

        public bool ComparePinCodes(string cardNumber, string pinCode)
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"SELECT case WHEN EXISTS " +
                                        $"(SELECT * FROM [Accounts]" +
                                        $"WHERE (PinCode = '{pinCode}' and CardNumber = '{cardNumber}'))" +
                                        $"THEN cast(1 as bit)" +
                                        $"else cast(0 as bit) end";

                        var reader = command.ExecuteReader();

                        reader.Read();

                        var result = int.Parse(reader.GetValue(0).ToString());
                        @return = Convert.ToBoolean(result);

                        reader.Close();
                    }
                }
            }
            catch (Exception ex) { }
            finally { }

            return @return;
        }

        public bool BlockingCard(string cardNumber)
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"UPDATE Accounts SET Status = '{StatusAccount.Blocked}' WHERE CardNumber = '{cardNumber}'";
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex) { }
            finally { @return = true; }

            return @return;
        }

        public bool SetNewBalance(double newBalance, string cardNumber)
        {
            bool @return = false;
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    SQLiteCommand command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = $"UPDATE Accounts SET Balance = '{newBalance}' WHERE CardNumber = '{cardNumber}'";

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { }
            finally { @return = true; }

            return @return;
        }
    }
}
