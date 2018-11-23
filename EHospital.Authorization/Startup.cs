namespace EHospital.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using EHospital.Authorization.BusinessLogic;
    using EHospital.Authorization.Data;
    using EHospital.Authorization.WebAPI;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        private static readonly log4net.ILog log = log4net.LogManager
                                                          .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = "Data Source=JULIKROP;Initial Catalog=Schema;Integrated Security=True"; //Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UsersDataContext>(options =>
                options.UseSqlServer(connection));

            //services.AddIdentity<UsersData,Roles>()
            //.AddUserStore<UsersDataContext>();
           // services.AddAutoMapper();

            services.AddScoped<IDataProvider, UsersDataContext>();
            services.AddScoped(typeof(UsersDataContext));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // укзывает, будет ли валидироваться издатель при валидации токена
                           ValidateIssuer = true,
                           // строка, представляющая издателя
                           ValidIssuer = AuthorizationOptions.ISSUER,

                           // будет ли валидироваться потребитель токена
                           ValidateAudience = true,
                           // установка потребителя токена
                           ValidAudience = AuthorizationOptions.AUDIENCE,
                           // будет ли валидироваться время существования
                           ValidateLifetime = true,

                           // установка ключа безопасности
                           IssuerSigningKey = AuthorizationOptions.GetSymmetricSecurityKey(),
                           // валидация ключа безопасности
                           ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddSingleton<IEmailSender, EmailSender>();

            log.Info("Using authorization service.");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "EHospital",
                    Description = "Authorization for eHostpital Project",
                    TermsOfService = "Welcome everybody!",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact() { Name = "Julia Kropivnaya", Email = "kropisha@gmail.com" }
                });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EHospital");
            });
        }
    }
}
