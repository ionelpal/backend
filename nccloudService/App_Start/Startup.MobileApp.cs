using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using nccloudService.DataObjects;
using nccloudService.Models;
using Owin;
using System.Collections.ObjectModel;
using System.Linq;

namespace nccloudService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new nccloudInitializer());

            nccloudContext db = new nccloudContext();
            db.Database.Initialize(true);
           
            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<nccloudContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

     public class nccloudInitializer : CreateDatabaseIfNotExists<nccloudContext>
   // public class nccloudInitializer : DropCreateDatabaseAlways<nccloudContext>
    {
        protected override void Seed(nccloudContext context)
        {
            List<Location> locations = new List<Location>
                {
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Tara Winthrop"},
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Balbriggan Private" },
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Swords Clinic" }
                };

            foreach (Location location in locations)
            {
                context.Set<Location>().Add(location);
            }
            List<Customer> customers = new List<Customer>
                {
                    new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="ionelpal@gmail.com", CustomerName="Ionel Pal", Location= locations[0] },
                     new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="elenarpal@gmail.com", CustomerName="Elena Pal", Location=locations[2] },
                      new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="anthonypal37@gmail.com", CustomerName="Ionel Smith", Location=locations[2]}
                };
            foreach (Customer customer in customers)
            {
                context.Set<Customer>().Add(customer);
            }

            List<Patient> patients = new List<Patient>
                {
                    new Patient {Id = Guid.NewGuid().ToString(), PatientName="John Smith", Location=locations[0], Customers = customers[0] },
                    new Patient {Id = Guid.NewGuid().ToString(), PatientName="Ionel Pal", Location=locations[1], Customers = customers[1]},
                     new Patient {Id = Guid.NewGuid().ToString(), PatientName="Anthony Pal", Location=locations[2], Customers = customers[2]}
                };
            foreach (Patient patient in patients)
            {
                context.Set<Patient>().Add(patient);
            }

            List<Message> messages = new List<Message>
                {
                    new Message {Id = Guid.NewGuid().ToString(), Title="Good News", Details="Just ley you kno that John had a great day today", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="Updates", Details="All the same tody, no new improvements.", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="Please Contact us ASP", Details="John need to be sent to the hospital ASP, we need your concent please. The docto recomends a surgery", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="New updates", Details="Thanks for yoy quick reply. So far he is stable and resting", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },

                     new Message {Id = Guid.NewGuid().ToString(), Title="Message 2", Details="Message detail 2", WrittenBy="Adriana",CustomerEmail="ionelpal@gmail.com",  Patient=patients[1] },
                      new Message {Id = Guid.NewGuid().ToString(), Title="Message 3", Details="Message detail 2", WrittenBy="Marie", CustomerEmail="elenarpal@gmail.com", Patient=patients[2] }
                };

            foreach (Message message in messages)
            {
                context.Set<Message>().Add(message);
            }


            List<Event> events = new List<Event>
                {
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Christmas Party", EventDetails="Event details aboout Christmas here",  EventDate= new DateTime(2016,11,10,10,0,0), Location = locations[0] },
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Charity Dayy", EventDetails="Event details about charity here",  EventDate= new DateTime(2016,12,11,10,0,0) },
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Halloween Party", EventDetails="Event details re here",  EventDate= new DateTime(2016,10,29,10,0,0) }
                };

            foreach (Event ev in events)
            {
                context.Set<Event>().Add(ev);
            }



            base.Seed(context);
        }
    }
}

