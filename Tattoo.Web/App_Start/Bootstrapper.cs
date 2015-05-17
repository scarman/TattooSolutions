#region Using Directives

using System;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Tattoo.Common.Enumerations;
using Tattoo.Data.Entities;
using Tattoo.Data.Infrastructure;
using Tattoo.Data.Repository;
using Tattoo.Service.Services;
using Tattoo.Web.Core.Authentication;
using Tattoo.Web.Mappings;
using Tattoo.Web.Validation;
using Constants = Tattoo.Common.Constants;

#endregion

namespace Tattoo.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            using (
                var manager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new DatabaseFactory().GetContext()))
                )
            {
                manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                var user = manager.FindByName(Constants.DefaultAdminUserName);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = Constants.DefaultAdminUserName,
                        Nick = Constants.DefaultAdminNickName,
                        Status = UserStatus.Active
                        //EmploymentStatus = EmploymentStatus.Employee
                    };
                    manager.Create(user, Constants.DefaultAdminPassword);
                    manager.AddToRole(user.Id, Role.Administrator.ToString());
                }
                else
                {
                    if (!manager.IsInRole(user.Id, Role.Administrator.ToString()))
                    {
                        manager.AddToRole(user.Id, Role.Administrator.ToString());
                    }
                }
            }

            SetAutofacContainer();

            // Configure AutoMapper
            AutoMapperSetup.Configure();
        }

        public static Func<T> PerHttpSafeResolve<T>()
        {
            return () => DependencyResolver.Current.GetService<T>();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterInstance(PerHttpSafeResolve<IDatabaseFactory>());

            builder.RegisterAssemblyTypes(typeof (UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof (UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof (DefaultFormsAuthentication).Assembly)
                .Where(t => t.Name.EndsWith("Authentication"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.Register(c => new UserStore<ApplicationUser>(new DatabaseFactory().GetContext()))
                .AsImplementedInterfaces();
            builder.Register(c => new IdentityFactoryOptions<UserManager<ApplicationUser>>
            {
                DataProtectionProvider = new DpapiDataProtectionProvider(Constants.ApplicationName)
            });
            builder.Register(
                c =>
                {
                    var manager =
                        new UserManager<ApplicationUser>(
                            new UserStore<ApplicationUser>(new DatabaseFactory().GetContext()));
                    manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                    {
                        AllowOnlyAlphanumericUserNames = false,
                        RequireUniqueEmail = true
                    };
                    // Configure validation logic for passwords
                    manager.PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 6,
                        RequireNonLetterOrDigit = true,
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireUppercase = false,
                    };
                    // Configure user lockout defaults
                    manager.UserLockoutEnabledByDefault = true;
                    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    manager.MaxFailedAccessAttemptsBeforeLockout = 5;
                    // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                    // You can write your own provider and plug in here.
                    manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
                    {
                        MessageFormat = "Your security code is: {0}"
                    });
                    manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
                    {
                        Subject = "SecurityCode",
                        BodyFormat = "Your security code is {0}"
                    });
                    return manager;
                })
                .As<UserManager<ApplicationUser>>().InstancePerRequest();

            builder.RegisterFilterProvider();

            builder.RegisterModule<ValidationModule>();
            var container = builder.Build();

            // Set up the FluentValidation provider factory and add it as a Model validator
            FluentValidationModelValidatorProvider.Configure(
                provider =>
                {
                    provider.ValidatorFactory = new AutofacValidatorFactory(container);
                    provider.AddImplicitRequiredValidator = false;
                });
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}