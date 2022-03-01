// See https://aka.ms/new-console-template for more information

using System.Dynamic;
using System.Net.Sockets;
using lab_1;

public class Program
{
    static int Main()
    {
        Console.WriteLine(@"Добрый день, это лабораторная работа № 1 группы 20ВП1, реализованная бригадой в составе: Дьячкова Д.А.");
        var obj = new TelephoneStation(address:"Велосипедная, 32е", companyName:"Тестовая компания № 1", countOfUsers:8);
        Console.WriteLine(obj.ToString());
        Console.WriteLine($"Адрес компании: {obj.Address ?? "Не установлен"}");
        obj.GetCompanyName();
        Console.WriteLine(@"Число клиентов компании в шестнадцатиричной системе счисления:" + 
                          $"{ (obj.CountOfUsers is null ? "<unset>": Convert.ToString((int) obj.CountOfUsers, 8))}");
        List<string> allFields;
        Console.WriteLine(
            @"Далее можно изменять значения полей класа. Для этого введите одно из следующих полей и через пробел его новое значение");
        Console.WriteLine($"Возможные поля: {string.Join(", ", (allFields = obj.GetAllFields()))}");
        while (true)
        {
            var inputData = Console.ReadLine() ?? "";
            string[] rawData;
            if ((rawData = inputData.Split(" ")).Length > 1 && obj.SetSomeValue(rawData[0],
                    string.Join(" ", rawData.Where((x, index) => index != 0))))
            {
                Console.WriteLine(obj.ToString());
            }
            else if (rawData[0] == @"end")
            {
                break;
            }
            else
            {
                Console.WriteLine(@"Некорректное название поля или некорректное значение поля");
            }
        }
        return 0;
    }
}