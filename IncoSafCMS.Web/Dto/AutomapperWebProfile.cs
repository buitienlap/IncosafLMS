using AutoMapper;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Web.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web
{
     //code cũ trước 17.10.2025
    public class AutoMapperConfig
    {
        //public static void config()
        //{
        //    Mapper.Initialize( cfg => cfg.CreateMap<Author,AuthorDTO>());
        //}

        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //way one 
                cfg.CreateMap<Accreditation, AccreditationDto>();
                //way two
                //cfg.AddProfile<AuthorMappingProfile>();
            }
           );

            return config;
        }
    }

    //way two 
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Accreditation, AccreditationDto>().ReverseMap();
        }
    }
    
    /*
    //17.10.2025 Lớp cấu hình AutoMapper dùng chung toàn hệ thống
    public static class AutoMapperConfig
    {
        // Biến Mapper dùng chung (thread-safe)
        public static IMapper Mapper { get; private set; }

        // Hàm cấu hình AutoMapper – gọi một lần khi khởi động ứng dụng
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Khai báo ánh xạ (mapping) giữa các lớp
                cfg.CreateMap<Accreditation, AccreditationDto>().ReverseMap();

                // Có thể thêm các ánh xạ khác ở đây
                // cfg.CreateMap<Equipment, EquipmentDto>();
            });

            // Kiểm tra cấu hình có hợp lệ không (không bắt buộc, nhưng nên có)
            config.AssertConfigurationIsValid();

            // Tạo mapper để dùng toàn cục
            Mapper = config.CreateMapper();
        }
    }
    */
}