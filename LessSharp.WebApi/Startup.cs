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
            #region 选项实体配置
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
             #region 身份验证服务
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
                        //判断accessToken是否已禁用
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
            //添加自定义的Api认证及授权失败方案
            .AddScheme<AuthenticationSchemeOptions, ApiFailHandler>(nameof(ApiFailHandler), o => { });
            #endregion
            #region 授权服务
            services.AddSingleton<IAuthorizationHandler, ApiPermissionAuthorizationHandler>();
            services.AddAuthorizationCore(o =>
            {
                o.AddPolicy(AuthorizationPolicyNames.ApiPermission, b =>
                {
                    b.RequireAuthenticatedUser().AddRequirements(new ApiPermissionRequirement());
                });
                //企业微信权限
                o.AddPolicy(AuthorizationPolicyNames.QyWeixin, b =>
                {
                    b.RequireClaim("qywxid").RequireClaim("employeeid");
                });
                //后台管理员权限
                o.AddPolicy(AuthorizationPolicyNames.Admin, b =>
                {
                    b.RequireClaim("uid");
                });
                //员工权限
                o.AddPolicy(AuthorizationPolicyNames.Employee, b =>
                {
                    b.RequireClaim("employeeid");
                });
            });
            #endregion
            #region 控制器服务
            services.AddControllers(o =>
            {
                //添加全局授权过滤器
                //o.Filters.Add(new AuthorizeFilter());
                //o.Filters.Add(new AuthorizeFilter(nameof(ApiPermissionRequirement)));
                //添加约定器，对ApiConventionController的派生类添加路由前缀
                o.Conventions.Add(new AutoRouteControllerModelConvention(Configuration.GetValue<string>("RoutePrefix")));
                //o.ValueProviderFactories.Add(new JQueryQueryStringValueProviderFactory());

            }).AddControllersAsServices().AddNewtonsoftJson(o =>
            {
                //转换json的日期时间格式
                o.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).ConfigureApiBehaviorOptions(options =>
            {
                //当发生400参数错误时
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
            #region Swagger服务    
            var apiSecurityScheme = new NSwag.OpenApiSecurityScheme()
            {
                Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                Name = "Authorization",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey
            };
            services.AddOpenApiDocument(settings =>
            {
                settings.DocumentName = "全部接口";
                settings.Title = "全部接口";
                settings.Description = "系统全部接口";
                settings.AddSecurity("身份认证Token", Enumerable.Empty<string>(), apiSecurityScheme);
            });
            typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
            {
                //获取枚举值上的特性
                var info = f.GetCustomAttributes(typeof(ApiGroupInfoAttribute), false).OfType<ApiGroupInfoAttribute>().FirstOrDefault();
                services.AddOpenApiDocument(settings =>
                {
                    settings.ApiGroupNames = new string[] { f.Name };
                    settings.DocumentName = info?.Title ?? f.Name;
                    settings.Title = info?.Title ?? f.Name;
                    settings.Description = info?.Description;
                    settings.Version = info?.Version;
                    settings.AddSecurity("身份认证Token", Enumerable.Empty<string>(), apiSecurityScheme);
                });
            });
            #endregion           
            #region 缓存
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
                //添加AOP的配置
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

            #region Swagger中间件
            app.UseOpenApi().UseSwaggerUi3();
            #endregion


            #region Api异常捕捉中间件
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
