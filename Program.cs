using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SP_EF_OTIS.DB;
using Microsoft.EntityFrameworkCore;
using SP_EF_OTIS.Entities;
using System.Numerics;
using SP_EF_OTIS.Repository;
using System.Runtime.ConstrainedExecution;
using System.Drawing;

namespace SP_EF_OTIS
{
    class Program
    {
        static void Main(string[] args)
        {

            SbOtisContext dbContext = new SbOtisContext();
            var currencyTable = new BaseRepository<Currency>(dbContext);
            var сlientTable = new BaseRepository<Client>(dbContext);
            var bankAccountTable = new BaseRepository<Bankaccount>(dbContext);
            Console.Clear();
            PrintAllTable(currencyTable, сlientTable, bankAccountTable);
            bool endApp = false;
            do
            {
                Console.WriteLine("{Хотите добавить данные в таблицу (y/n)?\n");
                string answer = Console.ReadLine().Trim().Substring(0, 1).ToLower();
                if (answer == "y")
                {
                    Console.WriteLine("Выберите таблицу из списка:");
                    Console.WriteLine("\t1 - Валюты");
                    Console.WriteLine("\t2 - Клиенты");
                    Console.WriteLine("\t3 - Счета клиентов");
                    Console.Write("Ваш выбор? ");
                    string num = Console.ReadLine().Substring(0, 1);
                    string nums = "123";
                    while (nums.IndexOf(num) == -1)
                    {
                        Console.Write("Это неверный ввод. Пожалуйста, выберите таблицу по номеру: ");
                        num = Console.ReadLine();
                        if (num.Length > 1) num = num.Substring(0, 1);
                    }
                    switch (num)
                    {
                        case "1":
                            AddTableCurrency(currencyTable);
                            break;
                        case "2":
                            AddTableClient(сlientTable);
                            break;
                        case "3":
                            AddTableBankaccount(bankAccountTable, сlientTable, currencyTable);
                            break;
                        default: break;
                    }
                    
                }

            
                else
                {
                    endApp = true;
                }
            } while (!endApp);
            Console.WriteLine("//********");
            return;

        }

        static void PrintAllTable(BaseRepository<Currency> currencyTable,
            BaseRepository<Client> сlientTable,
            BaseRepository<Bankaccount> bankAccountTable)
        {
            Console.WriteLine("Все таблицы.\n");
            Console.WriteLine("Валюта:\n");
            var currencyList = currencyTable.Get();
            foreach (var currency in currencyList)
            {
                Console.WriteLine($"{currency.Name} ({currency.Shortname}) - {currency.Code}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("Клиенты:\n");
            var сlientList = сlientTable.Get();
            foreach (var сlient in сlientList)
            {
                Console.WriteLine($"{сlient.Firstname} {сlient.Lastname} ИНН {сlient.Inn} Возраст {сlient.Age} ({сlient.Address})");
            }
            Console.WriteLine("\n");
            Console.WriteLine("Счета клиентов:\n");
            var bankAccountTableList = bankAccountTable.GetWithInclude(b => b.Client, a => a.Currency);
            foreach (var bankAccount in bankAccountTableList)
            {
                Console.WriteLine($"Счет {bankAccount.Numberaccount} ({bankAccount.Client?.Lastname}) - {bankAccount.Bankaccountturnover} {bankAccount.Currency?.Shortname}");
            }
        }

        static void AddTableCurrency(BaseRepository<Currency> table)
        {
            Console.Clear();
            var cur = new Currency();
            Console.WriteLine("Добавить валюту:\n");

            Console.Write("Введите код: ");
            cur.Code = Console.ReadLine();
            Console.Write("Введите короткое название: ");
            cur.Shortname = Console.ReadLine();
            Console.Write("Введите полное название валюты: ");
            cur.Name = Console.ReadLine();

            table.Create(cur);
            
        }

        static void AddTableClient(BaseRepository<Client> table)
        {
            Console.Clear();
            var cl = new Client();
            Console.WriteLine("Добавить клиента:\n");

            Console.Write("Введите Фамилию: ");
            cl.Lastname = Console.ReadLine();
            Console.Write("Введите Имя: ");
            cl.Firstname = Console.ReadLine();
            Console.Write("Введите ИНН: ");
            cl.Inn = Console.ReadLine();
            Console.Write("Введите возраст: ");
            var numInput = Console.ReadLine();
            Int32 cleanNum = 0;
            while (!Int32.TryParse(numInput, out cleanNum))
            {
                Console.Write("Это неверный ввод. Пожалуйста, введите допустимое число: ");
                numInput = Console.ReadLine();
            }
            cl.Age = cleanNum;
            Console.Write("Введите адрес: ");
            cl.Address = Console.ReadLine();

            table.Create(cl);

        }

        static void AddTableBankaccount(BaseRepository<Bankaccount> table, BaseRepository<Client> tableClient, BaseRepository<Currency> tableCurrency)
        {
            Console.Clear();
            var bankAcc = new Bankaccount();
            Console.WriteLine("Добавить валюту:\n");

            Console.Write("Введите фамилию клиента: ");
            var fName = Console.ReadLine().Trim();
            Client? client = tableClient.GetWithInclude(x => x.Lastname.StartsWith(fName)).FirstOrDefault();
            while (client == null)
            {
                Console.Write("Это неверный ввод. Пожалуйста, введите фамилию правильно: ");
                fName = Console.ReadLine();
                client = tableClient.GetWithInclude(x => x.Lastname.StartsWith(fName)).FirstOrDefault();
            }
            bankAcc.Clientid = client.Id;
            Console.Write("Укажите валюту счета в виде кода: ");
            var curCode = Console.ReadLine().Trim();
            Currency? cur = tableCurrency.GetWithInclude(x => x.Code.StartsWith(curCode)).FirstOrDefault();
            while (cur == null)
            {
                Console.Write("Это неверный ввод. Пожалуйста, введите код валюты правильно: ");
                curCode = Console.ReadLine();
                cur = tableCurrency.GetWithInclude(x => x.Code.StartsWith(curCode)).FirstOrDefault();
            }
            bankAcc.Currencyid = cur.Id;
            Console.Write("Введите номер счета: ");
            bankAcc.Numberaccount = Console.ReadLine();
            Console.Write("Введите сумму денежных средств на счете: ");
            var sum = Console.ReadLine();
            decimal cleansum = 0;
            while (!decimal.TryParse(sum, out cleansum))
            {
                Console.Write("Это неверный ввод. Пожалуйста, введите допустимое число: ");
                sum = Console.ReadLine();
            }
            bankAcc.Bankaccountturnover = cleansum;

            table.Create(bankAcc);

        }

    } 
    
}



