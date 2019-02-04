using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ContractAPI.Models;
using ContractAPI.Models.Response;

namespace ContractAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Contract, ResponseContract>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.PlayerId))
                    .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId))
                    .ForMember(dest => dest.YearlySalary, opt => opt.MapFrom(src => src.YearlySalary))
                    .ForMember(dest => dest.TransferFee, opt => opt.MapFrom(src => src.TransferFee))
                    .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.StartAt))
                    .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.EndAt))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                    .ForAllOtherMembers(o => o.Ignore());
            });

        }
    }
}
