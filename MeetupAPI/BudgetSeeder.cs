using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupAPI.Entities;

namespace MeetupAPI
{
    public class BudgetSeeder
    {
        private readonly BudgetContext _budgetContext;

        public BudgetSeeder(BudgetContext budgetContext)
        {
            _budgetContext = budgetContext;
        }

        public void Seed()
        {
            //if (_budgetContext.Database.CanConnect())
            //{
            //    if (!_budgetContext.Budgets.Any())
            //    {
            //        InsertSampleData();
            //    }
            //}
        }

        private void InsertSampleData()
        {
            var user1 = new User { userId = "admin", email = "johnckl3d@gmail.com", firstName = "John", lastName = "Cheang", passwordHash = "AQAAAAEAACcQAAAAEBu2+IljPGmOKLRAKnEToNisJN1ZZt6dqxknhBg0fS+/MDifbDff7UkCXKSW06vcZg==", roleId = 1 };
            var roles = new List<Role> {
            new Role { RoleName = "User"},
            new Role { RoleName = "Admin"}
        };
            var costItems1 = new List<CostItem>
                    {
                          new CostItem
                        {
                              costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c4",
                           name = "electric",
                           amount = 400.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                            dateTime = new DateTime(2021, 01, 15),
                            description = "electric desc"
        },
                             new CostItem
                        {
                                   costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c3",
                           name = "water",
                           amount = 30.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                              dateTime = new DateTime(2021, 01, 19),
                            description = "water desc"
                        },
                     };

            var costItems2 = new List<CostItem>
                    {
                          new CostItem
                        {
                              costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c5",
                           name = "pizza",
                           amount = 400.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429g",
                            dateTime = new DateTime(2021, 01, 01),
                            description = "pizza desc"
        },
                             new CostItem
                        {
                                   costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c6",
                           name = "burger",
                           amount = 30.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429g",
                              dateTime = new DateTime(2021, 01, 20),
                            description = "burger desc"
                        },
                     };
            var costCategories1 = new List<CostCategory> {
                 new CostCategory
                 {
                     costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     name = "Utilities",
                     totalAmount = 431.00,
                     budgetId = "b4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     costItems = costItems1
                   }
            };

            var costCategories2 = new List<CostCategory> {
                 new CostCategory
                 {
                     costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429g",
                     name = "food",
                     totalAmount = 20.00,
                     budgetId = "b4f10e4c-70cb-44ae-985b-32f13cf9429g",
                     costItems = costItems2
                   }
            };

            var budgets = new List<Budget>
            {
                new Budget
                {
                     budgetId = "b4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     name = "household",
                     totalBudgetAmount = 53,
                     totalCostAmount = 21,
                     costCategories = costCategories1

                },
                 new Budget
                {
                     budgetId = "b4f10e4c-70cb-44ae-985b-32f13cf9429g",
                     name = "travel",
                     totalBudgetAmount = 2000,
                     totalCostAmount = 20,
                     costCategories = costCategories2

                }
            };

            _budgetContext.AddRange(roles);
            _budgetContext.AddRange(budgets);
            _budgetContext.SaveChanges();
        }
    }
}
