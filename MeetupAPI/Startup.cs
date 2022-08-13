﻿using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MeetupAPI.Authorization;
using MeetupAPI.Entities;
using MeetupAPI.Filters;
using MeetupAPI.Identity;
using MeetupAPI.Models;
using MeetupAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using MeetupAPI.Helpers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MeetupAPI
{
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptions = new JwtOptions();
            Configuration.GetSection("jwt").Bind(jwtOptions);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton(jwtOptions);

            //Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions aiOptions
            //    = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            // Disables adaptive sampling.
            //aiOptions.EnableAdaptiveSampling = false;

            // Disables QuickPulse (Live Metrics stream).
            //aiOptions.EnableQuickPulseMetricStream = false;
            //services.AddApplicationInsightsTelemetry(aiOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.JwtIssuer,
                    ValidAudience = jwtOptions.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "English"));
                options.AddPolicy("AtLeast18", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
            });
            services.AddScoped<TimeTrackFilter>();
            services.AddScoped<IAuthorizationHandler, BudgetResourceOperationHandler>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddFluentValidation().AddNewtonsoftJson(
          options => {
              options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          });
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>(); 

            services.AddDbContext<BudgetContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<BudgetSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo() {Title = "MeetupAPI", Version = "v2"});
            });

            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient",
                    builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BudgetSeeder meetupSeeder, BudgetContext context, ILogger<Startup> logger)
        {
            RunMigrations(context);
            logger.LogInformation(
                "Configuring for {Environment} environment",
                env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCaching();
            app.UseStaticFiles();
            app.UseCors("FrontEndClient");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "MeetupAPI v2");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            meetupSeeder.Seed();
        }

        private void RunMigrations(BudgetContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
