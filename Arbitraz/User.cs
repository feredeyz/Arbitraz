using System;
using static Arbitraz.Stock;
using System.Text.Json;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace Arbitraz
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Money = 0;
        public Dictionary<Stock, int> Stocks = new Dictionary<Stock, int>();

        public User(string username, string password) {
            Username = username;
            Password = password;
        }
        public async void AddMoney(int money)
        {
            Money += money;
            await RefreshUserData();
        }

        public async Task RefreshUserData()
        {
            Dictionary<string, UserData> data = await GetDataFromJSON();
            data[Username] = new UserData { Password = Password, Money = Money, Stocks = Stocks };
            using (FileStream fs = new FileStream("", FileMode.Create, FileAccess.Write))
            {
                await JsonSerializer.SerializeAsync(fs, data);
            }
        }

        public void GetData()
        {
            Console.WriteLine($"Username: {Username}\nMoney: {Money}");
        }

        public static async Task<Dictionary<string, UserData>> GetDataFromJSON()
        {
            string filePath = "";
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, UserData>>(fs) ?? new Dictionary<string, UserData>();
        }

        async public static Task<User> Register()
        {
            string filePath = "";

            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            Dictionary<string, UserData> userData = await GetDataFromJSON();
            UserData data = new UserData { Password = password, Money = 0, Stocks = { } };
            if (userData.ContainsKey(username))
            {
                Console.WriteLine("This username is already taken");
                return null;
            } else
            {
                userData[username] = data;
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await JsonSerializer.SerializeAsync(fs, userData);
                }
                return new User(username, password);
            }
        }

        async public static Task<User> Login()
        {
            string filePath = "";

            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            Dictionary<string, UserData> userData = await GetDataFromJSON();
            if (userData.ContainsKey(username))
            {
                if (userData[username].Password == password)
                {
                    Console.WriteLine("You are logged in");
                    return new User(username, password);
                }
                else
                {
                    Console.WriteLine("Wrong password");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("This username doesn't exist");
                return null;
            }
        }
    }

    public class UserData
    {
        public string Password { get; set; }
        public int Money { get; set; }
        public Dictionary<Stock, int> Stocks { get; set; }
    }
    
}
