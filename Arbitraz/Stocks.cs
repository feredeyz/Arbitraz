using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Arbitraz
{
    public class Stock
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public double Growth { get; set; }

        public Stock(string name, int price, double growth)
        {
            Name = name;
            Price = price;
            Growth = growth;
        }

        public void ChangePrice()
        {
            Price *= 2;
        }

        public static async Task<Dictionary<string, StockData>> GetDataFromJSON()
        {
            string filePath = "";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File doesn't exist");
                return new Dictionary<string, StockData>();
            }

            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, StockData>>(fs) ?? new Dictionary<string, StockData>();
        }


        public async Task CreateStock()
        {
            string filePath = "";

            Dictionary<string, StockData> stockData = await GetDataFromJSON();
            StockData a = new StockData { Price = Price, Growth = Growth };
            stockData[Name] = a;

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await JsonSerializer.SerializeAsync(fs, stockData);
            }
        }

        public bool CheckData()
        {
            return Name.Length <= 5;
        }

        public static async Task AddStock()
        {
            Console.WriteLine("Напишите название вашей акции");
            string name = Console.ReadLine();
            Stock newStock = new Stock(name, 1, 1.0);

            if (!newStock.CheckData())
            {
                Console.WriteLine("Неправильно введённые данные. Попробуйте ещё раз");
            }
            else
            {
                await newStock.CreateStock();
                Console.WriteLine("Акция успешно создана!");
            }
        }
        
        public static async Task ShowAllStocks()
        {
            Dictionary<string, StockData> stocks = await GetDataFromJSON();
            if (stocks.Count > 0)
            {
                string result = "";
                foreach (var stock in stocks)
                    {
                        result += $"{stock.Key} - {stock.Value.Price}$ ({stock.Value.Growth})%\n";
                    }
                Console.Write(result);
            } else
            {
                Console.WriteLine("Нету акций.");
            }
        }

        public async static Task<List<Stock>> GetBuyableStocks(User user)
        {
            Dictionary<string, StockData> stocks = await GetDataFromJSON();
            List<Stock> BuyableStocks = new List<Stock> { };
            foreach (var stock in stocks)
            {
                if (stock.Value.Price <= user.Money)
                {
                    Stock newStock = new Stock(stock.Key, stock.Value.Price, stock.Value.Growth);
                    BuyableStocks.Add(newStock);
                }
            }
            return BuyableStocks;
        }

        public async static void ShowBuyableStocks(User user)
        {
            List<Stock> buyableStocks = await GetBuyableStocks(user);
            string result = "Вам доступны к покупке следующие акции:\n";
            foreach (var stock in buyableStocks)
            {
                result += $"{stock.Name} - ${stock.Price} ({stock.Growth}\n";
            }
            Console.Write(result);
        }

        public class StockData
        {
            public int Price { get; set; }
            public double Growth { get; set; }
        }
    }
}
