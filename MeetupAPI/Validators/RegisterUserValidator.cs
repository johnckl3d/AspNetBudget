using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using FluentValidation;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(BudgetContext meetupContext)
        {
            RuleFor(x => x.email).NotEmpty();
            RuleFor(x => x.password).MinimumLength(6);
            RuleFor(x => x.password).Equal(x => x.confirmPassword);
            RuleFor(x => x.email).Custom((value, context) =>
            {
                var userAlreadyExists = meetupContext.Users.Any(user => user.userId == value);
                if (userAlreadyExists)
                {
                    context.AddFailure("Userid", "That user id is taken");
                }

                var emailAlreadyExists = meetupContext.Users.Any(user => user.email == value);
                if (emailAlreadyExists)
                {
                    context.AddFailure("Email", "That email address is taken");
                }
            });
        }
    }
}
