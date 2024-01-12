using AutoMapper;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;

namespace eHandbook.modules.ManualManagement.Application.Mappings
{
    /// <summary>
    /// AutoMapper is a simple library that helps us to transform one object type into another. 
    /// It is a convention-based object-to-object mapper that requires very little configuration. 
    /// </summary>
    public class ManualProfileMap : Profile
    {
        /// <summary>
        /// A good way to organize our mapping configurations is with Profiles. 
        /// We need to create classes that inherit from Profile class and put the configuration in the constructor:
        /// </summary>
        public ManualProfileMap()
        {
            //In this case both POCO objects have same same, in case of diferents props. name, the props. need to be mapped as well.(http://tinyurl.com/yremtj9b)
            CreateMap<ManualEntity, ManualDto>().ReverseMap();
        }
    }
}
