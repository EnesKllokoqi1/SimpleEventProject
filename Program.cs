using System;

namespace ConsoleApp4{
    class Program{
        static void Main(string[] args){
            Employee employee = new Employee(2000); // Employee's salary
            employee.ExceededLimit += EmployeeAnnualSalaryTest;
            employee.Bonuses = 10; // Allowed bonus entries

            Console.WriteLine($"The employee's annual salary is: {employee.GetAnnualSalary()} euros");
            Console.WriteLine($"You can only enter bonuses {employee.Bonuses} times.");
            Console.WriteLine("Would you like to add a bonus to the employee? (yes/no)");

            string response = Console.ReadLine();
            if (response.ToUpper() == "YES"){
                Console.WriteLine("Press 'A' to add a bonus.");

                while (true){
                    char key = Console.ReadKey(true).KeyChar;
                    if (key != 'a') break; // Exit if the user presses anything other than 'A'

                    Console.Write("Enter bonus amount (or press Enter to stop): ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input)){
                        break; // Exit if the user presses Enter without entering a bonus
                    }

                    if (double.TryParse(input, out double bonusAmount)){
                        Console.WriteLine("Adding bonus...");
                        employee.AddBonus(bonusAmount);
                    } else {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }

                // Show the final salary when the user stops adding bonuses
                employee.DisplayFinalSalary();
            }
        }

        static void EmployeeAnnualSalaryTest(object sender, SalaryInformation e){
            Console.WriteLine($"{e.Message} {e.TotalBonuses}{Environment.NewLine}Final Salary : {e.FinalSalary}");
        }
    }

    class Employee{
        private double annualSalary;
        private double totalBonuses = 0;
        private int bonusCount = 0;

        public int Bonuses { get; set; }

        public Employee(double annualSalary){
            this.annualSalary = annualSalary;
        }

        public double GetAnnualSalary(){
            return annualSalary;
        }

        public void AddBonus(double amount){
            if (bonusCount >= Bonuses){
                SalaryInformation salaryInfo = new SalaryInformation{
                    Message = $" Maximum bonus limit of {Bonuses} reached. Total bonuses : ",
                    TotalBonuses = totalBonuses,
                    FinalSalary = annualSalary + totalBonuses
                };
                OnExceededLimit(salaryInfo);
                return;
            }

            totalBonuses += amount;
            bonusCount++;

            Console.WriteLine($" Bonus {bonusCount} added! Total bonuses: {totalBonuses}, ");
        }

        // Method to display the final salary if the user stops early
        public void DisplayFinalSalary(){
            Console.WriteLine($"\nFinal Salary: {annualSalary + totalBonuses}");
        }

        protected virtual void OnExceededLimit(SalaryInformation e){
            EventHandler<SalaryInformation> handler = ExceededLimit;
            handler?.Invoke(this, e);
        }

        public event EventHandler<SalaryInformation> ExceededLimit;
    }

    public class SalaryInformation : EventArgs{
        public string Message { get; set; }
        public double TotalBonuses { get; set; }
        public double FinalSalary { get; set; }
    }
}
