using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vit.Extensions;

namespace App
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

            //配置跨域
            services.AddCors();


            services.AddMvc(options =>
            {
                //使用自定义异常处理器
                options.Filters.Add<Sers.Serslot.ExceptionFilter.ExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                //全局配置Json序列化处理

                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                //设置序列化原样输出
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            })
            //设置兼容版本
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

       
            services.Populate();


            //配置上传文件 大小限制
            //https://blog.csdn.net/ddsthinkyou123/article/details/98850216
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 2000_000_000;//小于2000M
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region 允许跨域            
            app.UseCors(builder => builder
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
            #endregion


            //配置静态文件
            foreach (var config in Vit.Core.Util.ConfigurationManager.ConfigurationManager.Instance.GetByPath<Vit.WebHost.StaticFilesConfig[]>("server.staticFiles"))
            {
                app.UseStaticFiles(config);
            }
         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            #region api for appVersion
            app.Map("/version", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var version= System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).FileVersion;
                    await context.Response.WriteAsync(version);
                });

            });
            #endregion



            //app.UseHttpsRedirection();
            app.UseMvc();

 

        }
    }




     
}
