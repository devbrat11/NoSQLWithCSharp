using NoSQLWithCSharp.Common;
using NoSQLWithCSharp.Common.Entity;
using System;
using System.Collections.Generic;

namespace NoSqlWithCSharp.UI
{
    public class MongoApp
    {
        private IDataStore<User> _userDataStore;
        private INoSQLDB _db;
        private INoSQLClient _client;
        private List<Functionality> _functionalities;

        public MongoApp()
        {
            Console.WriteLine("|| NoQLWithCSharp ||");
            _client = DataStoreFactory.GetNoSQLClient();
            _db = _client.CreateDB(Configuration.DBName);
            _userDataStore = DataStoreFactory.GetUserDataStore(_db);
            Init();
            SeedUsersData();
        }

        public void ShowAppFunctionalities()
        {
            int index = 1;
            foreach(var functionality in _functionalities)
            {
                Console.WriteLine($"{index}. {functionality.Name}");
                index++;
            }
        }

        public void SelectFunctionality()
        {
            int shouldContinue = 1;
            do
            {
                Console.Write("Select Choice : ");
                var choice = int.Parse(Console.ReadLine());
                if (choice <= _functionalities.Count)
                {
                    var functionality = _functionalities[choice-1];
                    functionality.Function.Invoke();
                }
                else
                {
                    Console.WriteLine("Functionality doesn't exist !");
                }
                Console.Write("\nTo continue press '1' or press any other key to exit.");
                shouldContinue = int.Parse(Console.ReadLine());
            }
            while (shouldContinue == 1);
            
        }

        private void Init()
        {
            _functionalities = new List<Functionality>();
            _functionalities.Add(new Functionality("Show All Databases", ShowAllDBS));
            _functionalities.Add(new Functionality("Delete Database", DeleteDB));
            _functionalities.Add(new Functionality("Add User", AddUser));
            _functionalities.Add(new Functionality("Fetch All Users", FetchAllUsers));
            _functionalities.Add(new Functionality("Get User", GetUser));
            _functionalities.Add(new Functionality("Delete User", DeleteUser));
        }

        private void SeedUsersData()
        {
            Console.WriteLine("Seeding Test Data...");
            var ageGenerator = new Random();
            var users = new List<string> { "Anil", "Aditya", "Subham", "Harsh", "Shilpi", "Gayathri", "Devbrat", "Ashish" };

            int index = 0;
            foreach (var user in users)
            {
                _userDataStore.AddItem(new User()
                {
                    Name = user,
                    Age = ageGenerator.Next(25, 40),
                    Team = ((Team)(index % 3)).ToString()
                });
                index++;
            }
            Console.WriteLine("Seeding Test Finished !");

        }

        private void ShowAllDBS()
        {
            var dbs = _client.GetAllDBs();

            foreach (var db in dbs)
            {
                Console.WriteLine(db);
            }
        }

        private void DeleteDB()
        {
            Console.Write("Enter Database Name:");
            var dbName = Console.ReadLine();
            _client.DeleteDB(dbName);
        }

        private void AddUser()
        {
            var newUser = new User();
            Console.Write("Name : ");
            newUser.Name = Console.ReadLine();
            Console.Write("Age : ");
            newUser.Age = int.Parse(Console.ReadLine());
            Console.Write("Team : ");
            newUser.Team = Console.ReadLine();

            _userDataStore.AddItem(newUser);

        }

        private void FetchAllUsers()
        {
            var users = _userDataStore.GetAllItems().Result;
            foreach(var user in users)
            {
                Console.WriteLine(user.ToString());
            }
        }

        private void GetUser()
        {
            Console.Write("Enter User ID:");
            var userID = Console.ReadLine();
            var user = _userDataStore.GetItem(userID);
            if (user != null)
            {
                Console.WriteLine(user.ToString());
            }
            else
            {
                Console.WriteLine("User not found!");
            }
           
        }

        private void DeleteUser()
        {
            Console.Write("Enter User ID:");
            var userID = Console.ReadLine();
            _userDataStore.DeleteItem(userID);
        }

    }
}
