using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnterpriseArchitectureBuilder
{
    public class Business
    {
        private Business()
        {

        }

        public static void writeAbstract(string path, string modelName)
        {
            string code = "namespace Business.Services.Abstract\n" +
                    "{\n\n" +
                    "using Core.Utilities.Results;\n" +
                    "using Entities.Dtos;\n" +
                    "using System;\n" +
                    "using System.Collections.Generic;\n" +
                    "using System.Linq.Expressions;\n\n" +
                    "public interface I" + modelName + "Service\n" +
                    "{\n" +
                    "IDataResult<List<" + modelName + "Dto>> GetList(Expression<Func<" + modelName + "Dto, bool>> filter = null);\n" +
                    "IDataResult<" + modelName + "Dto> Get(Expression<Func<" + modelName + "Dto, bool>> filter = null);\n" +
                    "IResult Add(" + modelName + "Dto model);\n" +
                    "IResult Delete(" + modelName + "Dto model);\n" +
                    "IResult Update(" + modelName + "Dto model);\n" +
                    "}\n}";

            File.writeFile(code, path, "I" + modelName + "Service.cs");
        }

        public static void writeConcrete(string path, string modelName)
        {
            string code = "namespace Business.Services.Concrete\n{\n" +
                    "using AutoMapper;\n" +
                    "using Business.Constants;\n" +
                    "using Business.Services.Abstract;\n" +
                    "using Business.Statics;\n" +
                    "using Core.Aspects.Autofac.Caching;\n" +
                    "using Core.Utilities.Results;\n" +
                    "using DataAccess.Abstract;\n" +
                    "using Entities.Concrete.EntityFramework;\n" +
                    "using Entities.Dtos;\n" +
                    "using System;\n" +
                    "using System.Collections.Generic;\n" +
                    "using System.Linq;\n" +
                    "using System.Linq.Expressions;\n\n" +
                    "public class " + modelName + "Manager : I" + modelName + "Service\n" +
                    "{\n\n" +
                    "private I" + modelName + "Dal _" + modelName + "Dal;\n" +
                    "private IMapper _Mapper;\n\n" +
                    "public " + modelName + "Manager(I" + modelName + "Dal dal, IMapper Mapper)\n" +
                    "{\n" +
                    "_" + modelName + "Dal = dal;\n" +
                    "_Mapper = Mapper;\n" +
                    "}\n\n" +
                    "//[CacheAspect(duration: 120)]\n" +
                    "public IDataResult<List<" + modelName + "Dto>> GetList(Expression<Func<" + modelName + "Dto, bool>> filter = null)\n" +
                    "{\n" +
                    "var entityFilter = _Mapper.Map<Expression<Func<" + modelName + ", bool>>>(filter);\n" +
                    "var dto = _Mapper.Map<List<" + modelName + "Dto>>(_" + modelName + "Dal.GetList(entityFilter, IncludeStatic.Include" + modelName + ").ToList());\n" +
                    "return new SuccessDataResult<List<" + modelName + "Dto>>(dto);\n" +
                    "}\n\n" +
                    "public IDataResult<" + modelName + "Dto> Get(Expression<Func<" + modelName + "Dto, bool>> filter = null)\n" +
                    "{\n" +
                    "var entityFilter = _Mapper.Map<Expression<Func<" + modelName + ", bool>>>(filter);\n" +
                    "var dto = _Mapper.Map<" + modelName + "Dto>(_" + modelName + "Dal.Get(entityFilter, IncludeStatic.Include" + modelName + "));\n" +
                    "return new SuccessDataResult<" + modelName + "Dto>(dto);\n" +
                    "}\n\n" +
                    "[CacheRemoveAspect(\"I" + modelName + "Service.GetList\")]\n" +
                    "public IResult Add(" + modelName + "Dto model)\n" +
                    "{\n" +
                    "var entity = _Mapper.Map<" + modelName + ">(model);\n" +
                    "_" + modelName + "Dal.Add(entity);\n" +
                    "return new SuccessResult(Messages." + modelName + "Added);\n" +
                    "}\n\n" +
                    "[CacheRemoveAspect(\"I" + modelName + "Service.GetList\")]\n" +
                    "public IResult Update(" + modelName + "Dto model)\n" +
                    "{\n" +
                    "var entity = _Mapper.Map<" + modelName + ">(model);\n" +
                    "_" + modelName + "Dal.Update(entity);\n" +
                    "return new SuccessResult(Messages." + modelName + "Updated);\n" +
                    "}\n\n" +
                    "[CacheRemoveAspect(\"I" + modelName + "Service.GetList\")]\n" +
                    "public IResult Delete(" + modelName + "Dto model)\n" +
                    "{\n" +
                    "var entity = _Mapper.Map<" + modelName + ">(model);\n" +
                    " _" + modelName + "Dal.Delete(entity);\n" +
                    " return new SuccessResult(Messages." + modelName + "Deleted);\n" +
                    "}\n}\n}";
            
            File.writeFile(code, path, modelName + "Manager.cs");
        }

        public static void writeValidator(string path, string modelName)
        {
            string code = "using Entities.Concrete;\n" +
                    "using FluentValidation;\n\n" +
                    "namespace Business.ValidationRules.FluentValidation\n" +
                    "{\n\n" +
                    "public class " + modelName + "Validator : AbstractValidator<" + modelName + ">\n" +
                    "{\n\n" +
                    "public " + modelName + "Validator()\n" +
                    "{\n" +
                    "//RuleFor(p => p.Name).NotEmpty();\n" +
                    "}\n" +
                    "}\n}";

            File.writeFile(code, path, modelName + "Validator.cs");
        }

        public static void writeBusinessProfile(string path, List<string> models)
        {
            string code = "using AutoMapper;\n" +
                "using Core.Entities.Concrete;\n" +
                "using Entities.Concrete;\n" +
                "using Entities.Dtos;\n" +
                "using Entities.ViewModel;\n" +
                "namespace Business.AutoMapperProfile\n" +
                "{\n\n" +
                "public class BusinessProfile : Profile\n" +
                "{\n\n" +
                "public BusinessProfile()\n" +
                "{\n\n";

            foreach (var model in models)
            {
                code += "CreateMap<" + model + ", " + model + "Dto>();\n" +
                "CreateMap<" + model + "Dto, " + model + ">();\n\n";
            }

            code += "}\n}\n}";

            File.writeFile(code, path, "BusinessProfile.cs");
        }

        public static void writeMessages(string path, List<string> models)
        {

            string code = "namespace Business.Constants\n" +
                "{\n\n" +
                "public static class Messages\n" +
                "{\n\n" +
                "public static string UserNotFound = \"Lütfen bilgilerinizi kontrol ediniz.\";\n" +
                "public static string PasswordError = \"Lütfen bilgilerinizi kontrol ediniz.\";\n" +
                "public static string SuccessfulLogin = \"Sisteme giriş başarılı\";\n" +
                "public static string UserAlreadyExists = \"Bu kullanıcı zaten mevcut\";\n" +
                "public static string UserRegistered = \"Kullanıcı başarıyla kaydedildi\";\n" +
                "public static string AccessTokenCreated = \"Giriş başarılı\";\n\n\n" +
                "public static string AuthorizationDenied = \"Yetkiniz yok\";\n\n" +
                "#region Log\n" +
                "public static string LogAdded = \"Log başarıyla eklendi\";\n" +
                "public static string LogDeleted = \"Log başarıyla silindi\";\n" +
                "public static string LogUpdated = \"Log başarıyla güncellendi\";\n" +
                "#endregion Log\n\n\n";

            foreach (var model in models)
            {
                code += "#region " + model + "\n" +
                "public static string " + model + "Added = \"" + model + " başarıyla eklendi\";\n" +
                "public static string " + model + "Deleted = \"" + model + " başarıyla silindi\";\n" +
                "public static string " + model + "Updated = \"" + model + " başarıyla güncellendi\";\n" +
                "#endregion " + model + "\n";
            }

            code += "}\n}";

            File.writeFile(code, path, "Messages.cs");
        }

        public static void writeAutofacBusinessProfile(string path, List<string> models)
        {

            string code = "using Autofac;\n" +
                "using Autofac.Extras.DynamicProxy;\n" +
                "using Business.Abstract;\n" +
                "using Business.Concrete;\n" +
                "using Business.Services.Abstract;\n" +
                "using Business.Services.Concrete;\n" +
                "using Castle.DynamicProxy;\n" +
                "using Core.Utilities.Interceptors;\n" +
                "using Core.Utilities.Security.Jwt;\n" +
                "using DataAccess.Abstract;\n" +
                "using DataAccess.Concrete.EntityFramework;\n\n\n" +
                "namespace Business.DependencyResolvers.Autofac\n" +
                "{\n\n" +
                "public class AutofacBusinessModule : Module\n" +
                "{\n\n" +
                "protected override void Load(ContainerBuilder builder)\n" +
                "{\n" +
                "builder.RegisterType<EfLogDal>().As<ILogDal>();\n\n";

            foreach (var model in models)
            {
                code += "builder.RegisterType<" + model + "Manager>().As<I" + model + "Service>();\n" +
                    "builder.RegisterType<Ef" + model + "Dal>().As<I" + model + "Dal>();\n\n";
            }

            code += "builder.RegisterType<UserManager>().As<IUserService>();\n" +
            "builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();\n\n" +
            "builder.RegisterType<EfUserDal>().As<IUserDal>();\n\n" +
            "builder.RegisterType<AuthManager>().As<IAuthService>();\n" +
            "builder.RegisterType<JwtHelper>().As<ITokenHelper>();\n" +
            "builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();\n\n" +
            "var assembly = System.Reflection.Assembly.GetExecutingAssembly();\n\n" +
            "builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()\n" +
            ".EnableInterfaceInterceptors(new ProxyGenerationOptions()\n" +
            "{\n" +
            "Selector = new AspectInterceptorSelector()\n" +
            "}).SingleInstance();\n}\n}\n}";

            File.writeFile(code, path, "AutofacBusinessModule.cs");
        }

        public static void writeIncludeStatic(string path, List<string> models)
        {

            string code = "using Core.Entities.Concrete;\n" +
                "using Entities.Concrete;\n" +
                "using System;\n" +
                "using System.Linq.Expressions;\n\n" +
                "namespace Business.Statics\n" +
                "{\n\n" +
                "public class IncludeStatic\n" +
                "{\n\n";

            foreach (var model in models)
            {
                code += "public static Expression<Func<" + model + ", object>>[] Include" + model + " = { };\n";
            }

            code += "}\n}";

            File.writeFile(code, path, "IncludeStatic.cs");
        }

    }
}
