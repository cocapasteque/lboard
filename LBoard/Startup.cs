using System.Text;
using LBoard.Context;
using LBoard.Models.Config;
using LBoard.Services;
using LBoard.Services.Interfaces;
using LBoard.Services.Security.Jwt;
using LBoard.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LBoard
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(ILoggerFactory logFactory)
        {
            ApplicationLogging.LoggerFactory = logFactory;
            _logger = logFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddDbContext<LboardDbContext>(options =>
            {
                options.UseMySql(
                    $"Server=db;Database={DbConfig.MySqlDatabase};Uid={DbConfig.MySqlUser};Pwd={DbConfig.MySqlPassword}",
                    x => x.MigrationsAssembly("LBoard"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<LboardDbContext>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiConfig.JwtSecretKey)),
                        ValidateIssuer = true,
                        ValidIssuer = ApiConfig.JwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = ApiConfig.JwtAudience
                    };
                });


            services.AddAuthorization();

            services.AddHttpContextAccessor();
            
            services.AddSingleton<IEntriesService, RedisService>();
            services.AddSingleton<ILeaderboardsService, LeaderboardsService>();
            
            services.AddSingleton<IJwtTokenProvider<IdentityUser>, JwtTokenProvider>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(e => { e.MapControllers(); });

            _logger.LogInformation("Starting database migration...");
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<LboardDbContext>().Database.Migrate();
            _logger.LogInformation("Migration done.");
        }
    }
}