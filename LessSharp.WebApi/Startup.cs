using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using LessSharp.Common;
using LessSharp.Common.CacheHelper;
using LessSharp.Dto;
using LessSharp.Option;
using LessSharp.Service.Sys;
using LessSharp.WebApi.ApiGroup;
using LessSharp.WebApi.AuthenticationSchemes.ApiFail;
using LessSharp.WebApi.AuthorizationPolicies;
using LessSharp.WebApi.AuthorizationPolicies.ApiPermission;
using LessSharp.WebApi.Controllers;
using LessSharp.WebApi.Conventions;
using LessSharp.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

[assembly: DependOn(typeof(LessSharp.Service.Startup))]
namespace LessSharp.WebApi
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
            #region ѡ��ʵ������
            var JwtConfiguration = Configuration.GetSection("Jwt");
            services.Configure<JwtOption>(JwtConfiguration);
            JwtOption jwtOption = JwtConfiguration.Get<JwtOption>();
            var RedisConfiguration = Configuration.GetSection("Redis");
            services.Configure<RedisOption>(RedisConfiguration);
            RedisOption redisOption = RedisConfiguration.Get<RedisOption>();
            services.Configure<UploadOption>(Configuration.GetSection("Upload"));
            services.Configure<QyWeixinOption>(Configuration.GetSection("QyWeixin"));
            services.Configure<KeyOption>("QQMap", Configuration.GetSection("QQMap"));
            #endregion
             #region �����֤����
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiFailHandler);
                o.DefaultForbidScheme = nameof(ApiFailHandler);
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.HttpContext.Items[ApiFailHandler.FailCode] = ApiFailCode.TOKEN_EXPIRE;
                        }
                        else if (context.Exception is SecurityTokenValidationException)
                        {
                            context.HttpContext.Items[ApiFailHandler.FailCode] = ApiFailCode.TOKEN_INVALID;
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //�ж�accessToken�Ƿ��ѽ���
                        var token = context.SecurityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                        var cacheHelper = context.HttpContext.RequestServices.GetService<ICacheHelper>();
                        if (cacheHelper.SetContains(TokenService.DisabledTokenCacheKey, token.RawData))
                        {
                            context.HttpContext.Items[ApiFailHandler.FailCode] = ApiFailCode.TOKEN_INVALID;
                            context.Fail("TOKEN_INVALID");
                        }
                        return Task.CompletedTask;
                    }
                };
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.AccessSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            //����Զ����Api��֤����Ȩʧ�ܷ���
            .AddScheme<AuthenticationSchemeOptions, ApiFailHandler>(nameof(ApiFailHandler), o => { });
            #endregion
            #region ��Ȩ����
            services.AddSingleton<IAuthorizationHandler, ApiPermissionAuthorizationHandler>();
            services.AddAuthorizationCore(o =>
            {
                o.AddPolicy(AuthorizationPolicyNames.ApiPermission, b =>
                {
                    b.RequireAuthenticatedUser().AddRequirements(new ApiPermissionRequirement());
                });
                //��ҵ΢��Ȩ��
                o.AddPolicy(AuthorizationPolicyNames.QyWeixin, b =>
                {
                    b.RequireClaim("qywxid").RequireClaim("employeeid");
                });
                //��̨����ԱȨ��
                o.AddPolicy(AuthorizationPolicyNames.Admin, b =>
                {
                    b.RequireClaim("uid");
                });
                //Ա��Ȩ��
                o.AddPolicy(AuthorizationPolicyNames.Employee, b =>
                {
                    b.RequireClaim("employeeid");
                });
            });
            #endregion
            #region ����������
            services.AddControllers(o =>
            {
                //���ȫ����Ȩ������
                //o.Filters.Add(new AuthorizeFilter());
                //o.Filters.Add(new AuthorizeFilter(nameof(ApiPermissionRequirement)));
                //���Լ��������ApiConventionController�����������·��ǰ׺
                o.Conventions.Add(new AutoRouteControllerModelConvention(Configuration.GetValue<string>("RoutePrefix")));
                //o.ValueProviderFactories.Add(new JQueryQueryStringValueProviderFactory());

            }).AddControllersAsServices().AddNewtonsoftJson(o =>
            {
                //ת��json������ʱ���ʽ
                o.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).ConfigureApiBehaviorOptions(options =>
            {
                //������400��������ʱ
                options.InvalidModelStateResponseFactory = context =>
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    foreach (var state in context.ModelState)
                    {
                        dict.Add(state.Key, string.Join(" | ", state.Value.Errors.Select(e => e.ErrorMessage)));
                    }
                    var result = ApiResultDto<Dictionary<string, string>>.Fail(ApiFailCode.PARAMETER_ERROR, dict);
                    return new JsonResult(result);
                };
            });
            #endregion
            #region Swagger����    
            var apiSecurityScheme = new NSwag.OpenApiSecurityScheme()
            {
                Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
                Name = "Authorization",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey
            };
            services.AddOpenApiDocument(settings =>
            {
                settings.DocumentName = "ȫ���ӿ�";
                settings.Title = "ȫ���ӿ�";
                settings.Description = "ϵͳȫ���ӿ�";
                settings.AddSecurity("�����֤Token", Enumerable.Empty<string>(), apiSecurityScheme);
            });
            typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
            {
                //��ȡö��ֵ�ϵ�����
                var info = f.GetCustomAttributes(typeof(ApiGroupInfoAttribute), false).OfType<ApiGroupInfoAttribute>().FirstOrDefault();
                services.AddOpenApiDocument(settings =>
                {
                    settings.ApiGroupNames = new string[] { f.Name };
                    settings.DocumentName = info?.Title ?? f.Name;
                    settings.Title = info?.Title ?? f.Name;
                    settings.Description = info?.Description;
                    settings.Version = info?.Version;
                    settings.AddSecurity("�����֤Token", Enumerable.Empty<string>(), apiSecurityScheme);
                });
            });
            #endregion           
            #region ����
            if (redisOption != null && redisOption.Enable)
            {
                var options = new RedisCacheOptions
                {
                    InstanceName = redisOption.InstanceName,
                    Configuration = redisOption.Connection
                };
                var redis = new RedisCacheHelper(options, redisOption.Database);
                services.AddSingleton(redis);
                services.AddSingleton<ICacheHelper>(redis);
            }
            else
            {
                services.AddMemoryCache();
                services.AddScoped<ICacheHelper, MemoryCacheHelper>();
            }

            #endregion
            #region AOP
            services.ConfigureDynamicProxy(o =>
            {
                //���AOP������
            });
            #endregion
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            //app.UseHttpsRedirection();

            #region Swagger�м��
            app.UseOpenApi().UseSwaggerUi3();
            #endregion


            #region Api�쳣��׽�м��
            app.UseMiddleware<ApiExceptionMiddleware>();
            #endregion

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
