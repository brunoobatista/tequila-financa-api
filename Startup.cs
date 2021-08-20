using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Tequila.Core;
using Tequila.Repositories;
using Tequila.Services;

namespace Tequila
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
            services.AddCors();
            services.AddControllers()
                .AddJsonOptions(
                    o => o.JsonSerializerOptions.IgnoreNullValues = true
                );

            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(
                    Configuration.GetSection("ConnectionStrings")["TequilaDev"]
                )
            );

            services.AddScoped<CarteiraRepository>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<DespesasFixasRepository>();
            services.AddScoped<RendaAdicionalRepository>();
            services.AddScoped<DespesaRepository>();

            services.AddScoped<TokenService>();
            services.AddScoped<CarteiraService>();
            services.AddScoped<DespesasFixasService>();
            services.AddScoped<RendaAdicionalService>();
            services.AddScoped<DespesaService>();


            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false, 
                        ValidateLifetime = true
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseOptions();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}