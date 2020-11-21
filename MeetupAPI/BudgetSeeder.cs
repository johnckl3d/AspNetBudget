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
                if (!_budgetContext.CostCategories.Any())
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {
            var meetups = new List<Meetup>
            {
                new Meetup
                {
                    Name = "Web summit",
                    Date = DateTime.Now.AddDays(7),
                    IsPrivate = false,
                    Organizer = "Microsoft",
                    Location = new Location
                    {
                        City = "Krakow",
                        Street = "Szeroka 33/5",
                        PostCode = "31-337"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "Bob Clark",
                            Topic = "Modern browsers",
                            Description = "Deep dive into V8"
                        }
                    }
                },
                new Meetup
                {
                    Name = "4Devs",
                    Date = DateTime.Now.AddDays(7),
                    IsPrivate = false,
                    Organizer = "KGD",
                    Location = new Location
                    {
                        City = "Warszawa",
                        Street = "Chmielna 33/5",
                        PostCode = "00-007"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "Will Smith",
                            Topic = "React.js",
                            Description = "Redux introduction"
                        },
                        new Lecture
                        {
                            Author = "John Cena",
                            Topic = "Angular store",
                            Description = "Ngxs in practice"
                        }
                    },

                },
            };

            var budgets = new List<CostCategory> {
                 new CostCategory
                 {
                     costCategoryId = "a4f10e4c-70cb-44ae-985b-32f13cf9429f",
                     name = "Utilities",
                     totalAmount = 431.00,
                     costItems = new List<CostItem>
                    {
                          new CostItem
                        {
                              costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c4",
                           name = "electric",
                           amount = 400.50,
                        },
                             new CostItem
                        {
                                   costItemId = "cb4bf545-176b-4453-94c5-45262a9fd8c3",
                           name = "water",
                           amount = 30.50,
                        },
                     }
                   },
                   new CostCategory
                 {
                        costCategoryId = "ce9103a4-e331-45ea-9bac-54a693de3bfb",
                     name = "Food",
                       totalAmount = 10.50
                   }
        };
            _budgetContext.AddRange(budgets);
            _budgetContext.AddRange(meetups);
            _budgetContext.SaveChanges();
        }
    }
}
