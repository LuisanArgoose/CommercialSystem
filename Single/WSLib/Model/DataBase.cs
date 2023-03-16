using WSLib.Tables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSLib.Core;
using System.Data;

namespace WSLib.Model
{
    /// <summary>
    /// Класс базы данных
    /// </summary>
    public class DataBase 
    {
        private readonly SqlConnection _con;
        private SqlDataReader _reader;
        private User _actualUser;
        private List<MyException> _exceptions = new List<MyException>();
        private MyException _exception 
        { 
            set { _exceptions.Add(value);  } 
        }
        private SqlDataAdapter _adapter = new SqlDataAdapter();
        private SqlCommandBuilder _builder = new SqlCommandBuilder();
        public MyException GetException() { return _exceptions.Last(); }
        public User GetActualUser() { return _actualUser; }
        public DataBase()
        {
            _con = new SqlConnection("Server = .\\SQLEXPRESS; Database = SingleDB;Trusted_Connection=True;");
        }
        private bool ExecuteCom(string com, Action action)
        {
            bool result;
            SqlCommand cmd = new SqlCommand(com,_con);
            try
            {
                _con.Open();
            }
            catch (Exception e)
            {
                _exception = new MyException("Нет подключения к БД", 103);
            }
            try
            {
                _reader = cmd.ExecuteReader();
                int count = 0;
                while (_reader.Read())
                {
                    count++;
                    action();
                }
                if (count == 0)
                {
                    _exception = new MyException("Ни одной строки не найдено", 201);
                    result = false;

                }
                else
                {
                    result = true;
                }
                _reader.Close();
               
            }
            catch (Exception ex)
            {
                _exception= new MyException(ex.Message,101);
                result = false;
            }
            _con.Close();
            return result;
        }
        private bool ExecuteCom(string com)
        {
            SqlCommand cmd = new SqlCommand(com, _con);
            bool a;
            _con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                a = true;
            }
            catch (Exception ex)
            {
                _exception = new MyException(ex.Message, 102);
                a = false;
            }
            _con.Close();
            return a;
        }
        public void SingIn(string login, string password)
        {
            string command = string.Format("Select Users.Id, Users.Login, Users.FullName, Roles.Name as RoleName from Users " +
                             "join Roles on Users.RoleId = Roles.Id " +
                             "Where Login = '{0}' and Password = '{1}'",login,password);

            bool res = ExecuteCom(command, () => {
                _actualUser = new User(
                    int.Parse(_reader["Id"].ToString()),
                    _reader["Login"].ToString(),
                    _reader["RoleName"].ToString()
                    )
                { 
                    FullName = _reader["FullName"].ToString() 
                };
            });
            if (!res)
            {
                
                _exception = new MyException("Ошибка авторизации", 202);
                return;              
            }
            _exception = new MyException("Успешная авторизация", 301);

        }
        
        public List<string> GetTablesName()
        {
            List<string> tables = new List<string>();
            string command = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES");
            bool res = ExecuteCom(command, () => {

                tables.Add(_reader["TABLE_NAME"].ToString());

            });
            return tables;
        }
        public DataView SelectTable(string tableName, string filterName)
        {
            DataTable dt = new DataTable();
            if (filterName == "")
            {
                _adapter.SelectCommand = new SqlCommand($"Select * from {tableName}", _con);
            }
            else
            {
                List<string> columns = SelectColumnNames(tableName);
                string selCommand = "Select * from " + tableName + " where ";
                selCommand = LikeCommand(columns, filterName, tableName);
                _adapter.SelectCommand = new SqlCommand(selCommand, _con);
            }

            _adapter.Fill(dt);
            _builder = new SqlCommandBuilder(_adapter);
            if(dt.Rows.Count == 0) {
                _exception = new MyException("Записи не найдены", 204);
            }
            else
            {
                _exception = new MyException("Записи найдены", 303);
            }
            return dt.DefaultView;
        }
        private List<string> SelectColumnNames(string tableName)
        {
            List<string> columns = new List<string>();
            string command = string.Format($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME = '" + tableName + "'");
            bool res = ExecuteCom(command, () => {

                columns.Add(_reader["COLUMN_NAME"].ToString());

            });
            return columns;
        }
        private string LikeCommand(List<string> columns, string filterName, string tableName)
        {
            string command = "";
            for (int i = 0; i < columns.Count; i++)
            {
                command += tableName +"." + columns[i] + " like N'%" + filterName + "%'";
                if (!(i == columns.Count - 1))
                {
                    command += " or ";
                }

            }
            return command;
        }
        public void SaveChanges(DataTable table)
        {

            _adapter.InsertCommand = _builder.GetInsertCommand();
            _adapter.UpdateCommand = _builder.GetUpdateCommand();
            _adapter.DeleteCommand = _builder.GetDeleteCommand();
            try
            {
                int res = _adapter.Update(table);
                _exception = new MyException("Извменения внесены", 304);
                if (res == 0)
                    _exception = new MyException("Изменения не внесены", 205);
                
            }
            catch(Exception ex)
            {
                _exception = new MyException(ex.Message, 104);
            }
        }
        public DataView SelectTreaties(string search = "", string filter = "")
        {
            DataTable dt = new DataTable();
            string command = "Select Treaties.Id, Number,BuyDate, " +
                "Clients.FullName as ClientFullName, " +
                "Models.Name as ModelName, Users.FullName as UserFullName " +
                "from Treaties " +
                "join Clients on Clients.Id = ClientId " +
                "join Models on Models.Id = ModelId " +
                "join Users on Users.Id = UserId ";
            if(filter != "")
            {
                command += "Where Models.Name = N'" + filter + "' ";
                if(search != "")
                {
                    command += " and ";
                }
            }
            if(search != "")
            {
                if(filter == "")
                {
                    command += "Where ";
                }
                command += "(Treaties.Id like N'%" + search + "%' or Treaties.Number like N'%" + 
                    search + "%' or Treaties.BuyDate like N'%" + search + "%' or Clients.FullName like N'%" + 
                    search + "%' or Models.Name like N'%" + search + "%' or Users.FullName like N'%" + search + "%')";
            }
            


            _adapter.SelectCommand = new SqlCommand(command, _con);          
            _adapter.Fill(dt);
            _builder = new SqlCommandBuilder(_adapter);
            if (dt.Rows.Count == 0)
            {
                _exception = new MyException("Записи не найдены", 204);
            }
            else
            {
                _exception = new MyException("Записи найдены", 303);
            }
            return dt.DefaultView;
        }
        public List<string> GetModels()
        {
            List<string> models = new List<string>();
            string command = string.Format("Select Models.Name ModelsName from Models " +
                "Group by Models.Name");
            bool res = ExecuteCom(command, () => {

                models.Add(_reader["ModelsName"].ToString());

            });
            return models;
        }
        public List<string> GetClientsFullNames()
        {
            List<string> clientsFullNames = new List<string>();
            string command = string.Format("Select Clients.FullName as ClientsFullName from Clients ");
            bool res = ExecuteCom(command, () => {

                clientsFullNames.Add(_reader["ClientsFullName"].ToString());

            });
            return clientsFullNames;
        }
        public void InsertOrUpdateTreatie(Treaties t)
        {

            string command = string.Format("Select Clients.Id as ClientId from Clients where Clients.FullName = N'{0}'",t.ClientFullName);
            int ClientId = -1;
            bool res = ExecuteCom(command, () => {

                ClientId = int.Parse(_reader["ClientId"].ToString());

            });
            if (!res) { return; }
            command = string.Format("Select Models.Id as ModelId from Models where Models.Name = N'{0}'", t.ModelName);
            int ModelId = -1;
            res = ExecuteCom(command, () => {

                ModelId = int.Parse(_reader["ModelId"].ToString());

            });
            if (!res) { return; }
            if (t.Id == -1)
            {

                command = string.Format("Insert into Treaties values ({0},'{1}',{2},{3},{4})", t.Number,t.BuyDate.ToString("dd/MM/yyyy"),ClientId,ModelId,_actualUser.Id);
                res = ExecuteCom(command);
                if (!res) { return; }
            }
            else
            {
                command = string.Format("Update Treaties set Number = {0}, BuyDate = '{1}', " +
                    " ClientId = {2}, ModelId = {3}, UserId = {4} where Id = {5}", t.Number, t.BuyDate.ToString("dd/MM/yyyy"), ClientId, ModelId, _actualUser.Id,t.Id);
                res = ExecuteCom(command);
                if (!res) { return; }
            }
            _exception = new MyException("Успешное добавление", 305);
        }
        public void DeleteTreatie(int Id)
        {
            string command = string.Format("Delete from Treaties where Id = {0}", Id);
            bool res = ExecuteCom(command);
            if (!res)
            {
                return;
            }
            _exception = new MyException("Успешное удаление", 305);
        }
        public bool AddUser(string name)
        {
            if (name == "Admin")
                return false;
            else
                return true;
        }
        public bool DeleteUser(string name)
        {
            if (name == "Admin")
                return false;
            else
                return true;
        }
        public bool AddRole(string name)
        {
            if (name == "Admin")
                return false;
            else
                return true;
        }
        public bool DeleteRole(string name)
        {
            if (name == "Admin")
                return false;
            else
                return true;
        }
        public bool AddClient(string name)
        {
            if (name == "Лузан Роман Денисович")
                return false;
            else
                return true;
        }

    }
}
