using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App;

public class ATMService
{
    private List<User> users = new List<User>();
    private User currentUser = null;

    int round = 1;
    bool log = true;

    public ATMService()
    {

        InitializeUsers();
    }

    private void InitializeUsers()
    {
        User user1 = new User("nyinyi", "nnn", 1000);
        User user2 = new User("user2", "password2", 2000);
        User user3 = new User("user3", "password3", 3000);
        User user4 = new User("user4", "password4", 4000);
        User user5 = new User("user5", "password5", 5000);
        users.Add(user1);
        users.Add(user2);
        users.Add(user3);
        users.Add(user4);
        users.Add(user5);
    }

    public void ATMRun()
    {
        Console.WriteLine("Welcome to ATM Console Application.");
        Console.WriteLine("____________________________________");
        Console.WriteLine();

        int attempts = 0;

        while (currentUser == null )
        {
            Console.Write("Enter User ID: ");
            string userId = Console.ReadLine()!;
            Console.WriteLine("__________________________");
            Console.Write("Enter Password: ");

            string password = Console.ReadLine()!;
            Console.WriteLine("__________________________");


            currentUser = users.Find(u => u.UserID == userId && u.Password == password )!;

            if (currentUser is not null)
            {
                Console.WriteLine("Login Successful");
                Console.WriteLine();
                
                ShowMenu();
            }
            else
            {
                Console.WriteLine("Login Failed");
                attempts++;
                if (attempts >= 3)
                {

                   
                        Console.WriteLine("Account locked due to 3 failed login attempts. Restart the program to try again.");
                         Console.WriteLine();

                    EndProgram();
                    break;
                    
                   
                }
            }
        }




    }

    private void ShowMenu()
    {

        while (round == 1)
        {
            Console.WriteLine("ATM-Menu");
            Console.WriteLine("__________________________");
            Console.WriteLine();
            Console.WriteLine("1. Withdraw");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Check Balance");
            Console.WriteLine("4. Logout");
            Console.WriteLine("5. End Program");
            Console.Write("Enter your choice: ");


            string chose = Console.ReadLine()!;

            Console.WriteLine("__________________________");

            switch (chose)
            {

                case "1":
                    Withdraw();
                    break;
                case "2":
                    Deposit();
                    break;
                case "3":
                    CheckBalance();
                    break;
                case "4":
                    Logout();
                    break;
                case "5":
                    EndProgram();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }


        }

    }



    private void Withdraw()
    {
        Console.Write("Enter amount to withdraw: ");
        decimal amount = decimal.Parse(Console.ReadLine()!);
        Console.WriteLine("__________________________");
        if (amount > currentUser.Wallet)
        {
            Console.WriteLine("Insufficient funds.");
        }
        else
        {
            currentUser.Wallet -= amount;
            Console.WriteLine($"Withdrawal successful. New balance: {currentUser.Wallet}");
        }
    }

    private void Deposit()
    {
        Console.Write("Enter amount to deposit: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            if (amount > 0)
            {
                currentUser.Wallet += amount;
                Console.WriteLine($"Deposited: ${amount}. New Balance: ${currentUser.Wallet}\n");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount.\n");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.\n");
        }
    }

    private void EndProgram()
    {
        Console.WriteLine("Exiting program...");
        round = 0;
        
    }

    private void Logout()
    {
        Console.WriteLine("Logging out successful");
        
        round = 0;

        Console.WriteLine("__________________________");

        Console.WriteLine();
        Console.WriteLine("____________________________________");
        Console.WriteLine();
        Console.WriteLine("Do you want to login (y/n)?");

        string  login = Console.ReadLine()!;

        if (login == "y")
        {

            Console.WriteLine();
           
            currentUser = null;
            round = 1;
            ATMRun();
        }
        else if(login == "n") {
        
            round = 0;

            EndProgram();
            
           
        }

        else
        {
            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            Console.WriteLine("__________________________");
            Logout();
        }
    }

    private void CheckBalance()
    {
        Console.WriteLine($"Current Balance: ${currentUser.Wallet}\n");
    }
}
