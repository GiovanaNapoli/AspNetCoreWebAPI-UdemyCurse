using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.WebAPI.Data;

namespace SmartSchool.WebAPI
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
            services.AddDbContext<SmartContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            services.AddControllers()
                .AddNewtonsoftJson(
                    opt => opt.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            services.AddVersionedApiExplorer( opt =>{
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(opt => {
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.DefaultApiVersion = new ApiVersion(1,0);

                    opt.ReportApiVersions = true;
            });

            var apiProviderDescription = services.BuildServiceProvider()
                .GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(opt =>
            {
                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    opt.SwaggerDoc(
                        description.GroupName,
                        new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = "SmartSchool API",
                            Version = description.ApiVersion.ToString(),
                            TermsOfService = new Uri("http://meustermosdeuso.com"),
                            Description = "Está API foi criada ao decorrer do Curso: Criação de Web API com .NET Core",
                            License = new Microsoft.OpenApi.Models.OpenApiLicense{
                                Name = "SmartSchool Licence",
                                Url = new Uri("http://mit.com")
                            },
                            Contact = new Microsoft.OpenApi.Models.OpenApiContact{
                                Name = "Giovana Aparecida",
                                Email = "",
                                Url = new Uri("http://meusite.com")
                            }
                        });
                }
              

                //  var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //  var xmlCommentsFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                //  opt.IncludeXmlComments(xmlCommentsFileFullPath);
            });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors( x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI()
                .UseSwaggerUI(opt =>
                {
                    foreach (var description in apiDescriptionProvider.ApiVersionDescriptions)
                    {
                        opt.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json", 
                            description.GroupName.ToUpperInvariant());   
                    }
                    opt.RoutePrefix = "";
                });

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
