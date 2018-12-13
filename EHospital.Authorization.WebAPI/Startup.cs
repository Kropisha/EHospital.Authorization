using EHospital.Authorization.BusinessLogic.EmailAction;
using EHospital.Authorization.Data.Data;
using EHospital.Authorization.WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EHospital.Authorization.WebAPI
{
    public class Startup
    {
        private ILogging _log;

        private const string ConnectionStringName = "EHospitalDB";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString(ConnectionStringName);

            services.AddDbContext<UsersDataContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IDataProvider, UsersDataContext>();
            services.AddScoped(typeof(UsersDataContext));
            services.AddScoped<ILogging, CurrentLogger>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidIssuer = AuthorizationOptions.Issuer,
                           ValidateAudience = true,
                           ValidAudience = AuthorizationOptions.Audience,
                           ValidateLifetime = true,
                           IssuerSigningKey = AuthorizationOptions.GetSymmetricSecurityKey(),
                           ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "EHospital",
                    Description = "Authorization for eHospital Project",
                    TermsOfService = "Welcome everybody!",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact() { Name = "Julia Kropivnaya", Email = "kropisha@gmail.com" }
                });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogging logg)
        {
            _log = logg;
            _log.LogInfo("Using authorization service.");
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
            app.UseCors("CorsPolicy");
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EHospital");
            });
        }
    }
}
