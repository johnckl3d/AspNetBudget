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
            if (_budgetContext.Database.CanConnect())
            {
                if (!_budgetContext.Budgets.Any())
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {
            var costItems = new List<CostItem>
                    {
                          new CostItem
                        {
                              costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c4",
                           name = "electric",
                           amount = 400.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                            dateTime = new DateTime(2020, 11, 18),
                            description = "821123145081"
        },
                             new CostItem
                        {
                                   costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c3",
                           name = "water",
                           amount = 30.50,
                            costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                              dateTime = new DateTime(2020, 11, 19),
                            description = "821123145081"
                        },
                     };
            var costCategories1 = new List<CostCategory> {
                 new CostCategory
                 {
                     costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     name = "Utilities",
                     totalAmount = 431.00,
                     budgetId = "b4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     costItems = costItems
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

                }
            };
            _budgetContext.AddRange(budgets);
            _budgetContext.SaveChanges();
        }
    }
}
