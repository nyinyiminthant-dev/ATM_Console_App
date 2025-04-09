using System;
using System.Collections.Generic;
using System.Linq;
using ATM_Console_App.Dtos;

namespace ATM_Console_App
{
    public class ATMService
    {
        private readonly AppDbContext _db;
        private User? currentUser;
        private int round = 1;
        private List<User> users;

        public ATMService()
        {
            _db = new AppDbContext();
            users = _db.users.ToList();
        }

        public void ATMRun()
        {
            Console.WriteLine("Welcome to ATM Console Application.");
            Console.WriteLine("____________________________________\n");

            int attempts = 0;

            while (currentUser == null)
            {
                Console.Write("Enter UserName: ");
                string userName = Console.ReadLine()!;
                Console.WriteLine("__________________________");

                Console.Write("Enter Password: ");
                string password = Console.ReadLine()!;
                Console.WriteLine("__________________________");

                var user = users.FirstOrDefault(u => u.UserName == userName && u.Password == password);

                if (user != null)
                {
                    if (user.Islock == "N")
                    {
                        currentUser = user;
                        Console.WriteLine("Login Successful\n");
                        ShowMenu();
                    }
                    else
                    {
                        Console.WriteLine("Your account is locked.\n");
                        break;
                    }
                }
                else
                {
                    attempts++;
                    Console.WriteLine("Login Failed\n");

                    if (attempts >= 3)
                    {
                        Console.WriteLine("Your acoount has locked due to 3 times filed password \n");

                        var targetUser = users.FirstOrDefault(u => u.UserName == userName);
                        if (targetUser != null)
                        {
                            targetUser.Islock = "Y";
                            _db.users.Update(targetUser);
                            _db.SaveChanges();
                        }

                        break;
                    }
                }
            }
        }

        private void ShowMenu()
        {
            while (round == 1)
            {
                Console.WriteLine("ATM Menu");
                Console.WriteLine("__________________________\n");
                Console.WriteLine("1. Withdraw");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Check Balance");
                Console.WriteLine("4. Logout");
                Console.WriteLine("5. End Program");
                Console.WriteLine("6. Report");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine()!;
                Console.WriteLine("__________________________");

                switch (choice)
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
                    case "6":
                        ShowReport();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
        }

        private void Withdraw()
        {
            Console.Write("Enter amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("__________________________");
                if (amount <= 0)
                {
                    Console.WriteLine("Invalid withdraw amount.\n");
                }
                else if (amount > currentUser!.Wallet)
                {
                    Console.WriteLine("Insufficient funds.\n");
                }
                else
                {
                    currentUser.Wallet -= amount;
                    _db.users.Update(currentUser);
                    _db.SaveChanges();

                    AddTransaction("Withdraw", amount);

                    Console.WriteLine($"Withdrawal successful. New balance: {currentUser.Wallet}\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.\n");
            }
        }

        private void Deposit()
        {
            Console.Write("Enter amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount > 0)
                {
                    currentUser!.Wallet += amount;
                    _db.users.Update(currentUser);
                    _db.SaveChanges();

                    AddTransaction("Deposit", amount);

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

        private void CheckBalance()
        {
            Console.WriteLine($"Current Balance: ${currentUser!.Wallet}\n");
        }

        private void AddTransaction(string type, decimal amount)
        {
            Transaction tx = new Transaction
            {
                
                UserID = currentUser!.UserID,
                TransactionType = type,
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            _db.Transactions.Add(tx);
            _db.SaveChanges();
        }

        private void ShowReport()
        {
            Console.WriteLine($"\nTransaction Report for User: {currentUser!.UserID}");
            Console.WriteLine("------------------------------------");

            var transactions = _db.Transactions
                .Where(t => t.UserID == currentUser.UserID)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            foreach (var t in transactions)
            {
                Console.WriteLine($"{t.TransactionDate}: {t.TransactionType} - ${t.Amount}");
            }

            Console.WriteLine();
        }

        private void Logout()
        {
            Console.WriteLine("Logging out successful\n");
            round = 0;

            Console.WriteLine("Do you want to login again? (y/n)");
            string login = Console.ReadLine()!;

            if (login.ToLower() == "y")
            {
                currentUser = null;
                round = 1;
                ATMRun();
            }
            else if (login.ToLower() == "n")
            {
                EndProgram();
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                Logout();
            }
        }

        private void EndProgram()
        {
            Console.WriteLine("Exiting program...");
            round = 0;
        }
    }
}
