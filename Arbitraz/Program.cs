using System;
using System.ComponentModel.Design;
using Arbitraz;

Program program = new Program();

string HelpMessage = "/stocks - Вывести список всех акций\n/profile - Просмотреть профиль\n/addstock - Создать акцию\n/buy - Купить акции";
string HelpMessageProfile2 = "/info - Ваши данные\n/mystocks - Посмотреть список ваших акций\n/exit - Выйти из профиля\n/addmoney - Добавить денег";
string HelpMessageProfile1 = "/register - Регистрация\n/login - Вход";

Console.WriteLine("Добро пожаловать на Arbitraz! Напишите /help, если вам нужна помощь");

User user = null;

while (true)
{
    string input = Console.ReadLine();
    switch (input)
    {
        case "/help":
            Console.WriteLine(HelpMessage);
            break;
        case "/addstock":
            Stock.AddStock();
            break;
        case "/stocks":
            Stock.ShowAllStocks();
            break;

        case "/auth":
            if (user != null) Console.WriteLine("Вы уже вошли в аккаунт");
            while (user == null)
            {
                Console.WriteLine("Напишите /register для регистрации или /login для входа.\n /quit - Выйти из аутентификации");
                string profileInput = Console.ReadLine();
                switch (profileInput)
                {
                    case "/register":
                        user = await User.Register();
                        break;
                    case "/login":
                        user = await User.Login();
                        break;
                    case "/quit":
                        break;
                }
                break;
            }
            break;

        case "/info":
            if (user == null)
            {
                Console.WriteLine("Войдите в аккаунт.");
                break;
            }
            user.GetData();
            break;

        case "/mystocks":
            if (user == null)
            {
                Console.WriteLine("Войдите в аккаунт.");
                break;
            }
            break;

        case "/addmoney":
            if (user == null)
            {
                Console.WriteLine("Войдите в аккаунт.");
                break;
            }
            Console.WriteLine("Введите сумму денег");
            string moneyInput = Console.ReadLine();
            int money;
            try
            {
                money = int.Parse(moneyInput);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка! Попробуйте ещё раз");
                goto case "/addmoney";
            }
            user.AddMoney(money);
            Console.WriteLine($"{money}$ зачиселно на баланс.");
            break;
        case "/logout":
            if (user == null)
            {
                Console.WriteLine("Войдите в аккаунт.");
                break;
            }
            user = null;
            break;
        case "/buy":
            if (user == null)
            {
                Console.WriteLine("Войдите в аккаунт.");
                break;
            }
            Stock.ShowBuyableStocks(user);
            break;
            
        default:
            Console.WriteLine("Неизвестная команда. Введите /help для списка команд.");
            break;
    }
}
