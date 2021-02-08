using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using ConsimpleTestTask.WebApp.DTO;
using ConsimpleTestTask.WebApp.Exceptions;
using ConsimpleTestTask.WebApp.Models;
using ConsimpleTestTask.WebApp.Utils;
using IRO.Mvc.CoolSwagger;
using IRO.Mvc.Core;
using IRO.Mvc.MvcExceptionHandler;
using IRO.Mvc.MvcExceptionHandler.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConsimpleTestTask.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings.Init(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    var contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                    opt.SerializerSettings.ContractResolver = contractResolver;
                });
            AddSwaggerGen_Local(services);
            services.AddMvcExceptionHandler();
            AddDatabase(services);
            InitAutomapper(services);
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            var mapper = app.ApplicationServices.GetService<IMapper>();
            GlobalMapper.Init(mapper);

            if (AppSettings.IS_DEBUG)
            {
                app.UseDeveloperExceptionPage();
            }
            UseExceptionBinder_Local(app, AppSettings.IS_DEBUG);
            if (AppSettings.IS_DEBUG)
            {
                app.UseMiddleware<RewindHttpStreamsMiddleware>();
                app.UseAllRequestsLogging("requestsLogs");
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            UseSwaggerUI_Local(app);
        }

        void InitAutomapper(IServiceCollection services)
        {

            services.AddAutoMapper(c =>
            {
                var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.Name.StartsWith("ConsimpleTestTask.WebApp.Models"));
                foreach (var t in types)
                {
                    c.CreateMap(t, t);
                }
            });
        }

        void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>();
        }

        void UseExceptionBinder_Local(IApplicationBuilder app, bool isDebug)
        {
            app.UseMvcExceptionHandler((s) =>
            {
                s.IsDebug = isDebug;
                s.DefaultHttpCode = 500;
                s.CanBindByHttpCode = true;
                s.Host = AppSettings.EXTERNAL_URL;
                s.JsonSerializerSettings.Formatting = isDebug ? Formatting.Indented : Formatting.None;
                s.FilterAfterDTO = async (errorContext) =>
                {
                    var errorDto = new CustomErrorDto();
                    errorDto.Error.Message = errorContext.ResponseDTO.Message;
                    errorDto.Error.ErrorKey = errorContext.ResponseDTO.ErrorKey;
                    if (isDebug)
                    {
                        errorDto.Error.DebugUrl = errorContext.ResponseDTO.DebugUrl;
                    }

                    var jsonStr = JsonConvert.SerializeObject(errorDto, s.JsonSerializerSettings);
                    var resp = errorContext.HttpContext.Response;
                    resp.ContentType = "application/json";
                    resp.StatusCode = errorContext.ErrorInfo.HttpCode ?? 500;
                    await resp.WriteAsync(jsonStr);
                    return true;
                };

                s.Mapping((builder) =>
                {
                    //Регистрируем исключение по http коду
                    builder.Register(
                        httpCode: 500,
                        errorKey: "InternalServerError"
                        );
                    builder.Register(
                        httpCode: 403,
                        errorKey: "Forbidden"
                        );
                    builder.Register<UnauthorizedException>(
                        httpCode: 401,
                        errorKey: "Unauthorized"
                        );
                    builder.Register(
                        httpCode: 400,
                        errorKey: "BadRequest"
                    );

                    builder.RegisterAllAssignable<Exception>(
                        httpCode: 500,
                        errorKeyPrefix: ""
                        );
                });
            });
        }

        void UseSwaggerUI_Local(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.EnableValidator();
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V1");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });


        }

        void AddSwaggerGen_Local(IServiceCollection services)
        {

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(
                    "v2",
                    new OpenApiInfo
                    {
                        Title = "My API",
                        Version = "v2",
                        Description = "Api description"
                    });
                opt.EnableAnnotations();
                opt.UseCoolSummaryGen();
                opt.UseDefaultIdentityAuthScheme();
                opt.AddSwaggerTagNameOperationFilter();
                opt.AddDefaultResponses(new ResponseDescription()
                {
                    StatusCode = 500,
                    Description = "Server visible error.",
                    Type = typeof(CustomErrorDto)
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
