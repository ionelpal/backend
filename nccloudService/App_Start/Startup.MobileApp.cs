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
            config.EnableSystemDiagnosticsTracing();
            new MobileAppConfiguration()
            .UseDefaultConfiguration()
            .ApplyTo(config);
            //Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new nccloudInitializer());

            nccloudContext db = new nccloudContext();
            db.Database.Initialize(true);
            MobileAppSettingsDictionary settings =
                config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
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
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Tara Winthrop", Address="Tara Winthrop Private Clinic, Nevinstown Lane, Swords, Co. Dublin, Ireland"  },
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Balbriggan Private", Address="1 Main Street, Balbriggan, Co Dublin, Ireland"},
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Swords Clinic", Address="Swords, Main Street Swords, Swords, Ireland" }
                };

            foreach (Location location in locations)
            {
                context.Set<Location>().Add(location);
            }
            List<Customer> customers = new List<Customer>
                {
                    new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="ionelpal@gmail.com", CustomerName="Ionel Pal", Location= locations[0] },
                     new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="elenarpal@gmail.com", CustomerName="Elena Pal", Location=locations[1] },
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
                    new Message {Id = Guid.NewGuid().ToString(), Title="Please Contact us ASP", Details="John needs to be sent to the hospital ASP, we need your concent please. The doctor recomends a surgery.", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="New updates", Details="Thanks for yoy quick reply. So far he is stable and resting", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                     new Message {Id = Guid.NewGuid().ToString(), Title="Good News", Details="Just ley you kno that John had a great day today", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="Updates", Details="All the same tody, no new improvements.", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="Please Contact us ASP", Details="John need to be sent to the hospital ASP, we need your concent please. The docto recomends a surgery", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="New updates", Details="Thanks for yoy quick reply. So far he is stable and resting", WrittenBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                     new Message {Id = Guid.NewGuid().ToString(), Title="New updates", Details="Hi Ionel, Your father had a good day tody. He is making massive progress.", WrittenBy="Adriana",CustomerEmail="ionelpal@gmail.com",  Patient=patients[1] },
                     new Message {Id = Guid.NewGuid().ToString(), Title="Please come by", Details="Hi, please come by to talk about the new recovery plan.", WrittenBy="Adriana",CustomerEmail="ionelpal@gmail.com",  Patient=patients[1] },
                    new Message {Id = Guid.NewGuid().ToString(), Title="Urgent", Details="Hi Ionel, Your father need urgent his new glasses ", WrittenBy="Adriana",CustomerEmail="ionelpal@gmail.com",  Patient=patients[1] },

                      new Message {Id = Guid.NewGuid().ToString(), Title="Urgent", Details="Hi Anthony, Your father need urgent his new glasses ", WrittenBy="Adriana",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },
                        new Message {Id = Guid.NewGuid().ToString(), Title="Updates", Details="Hi Anthony, Today he took his tables without any issues. ", WrittenBy="Elena",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },
                          new Message {Id = Guid.NewGuid().ToString(), Title="Please call", Details="Hi Anthony, Your father need urgent his new glasses ", WrittenBy="Silvia",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },
                            new Message {Id = Guid.NewGuid().ToString(), Title="Updates", Details="Hi Anthony, Ionel is fealing a bit bettrer today!", WrittenBy="Elena",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },
                              new Message {Id = Guid.NewGuid().ToString(), Title="New updates", Details="Hi Anthony, I just like to let you know that he had improved a lot! ", WrittenBy="Adriana",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },
                                new Message {Id = Guid.NewGuid().ToString(), Title="Urgent", Details="Hi Anthony, Your father need urgent to go hospital, please call or come over when ready! ", WrittenBy="Maria",CustomerEmail="anthonypal37@gmail.com",  Patient=patients[2] },

                      new Message {Id = Guid.NewGuid().ToString(), Title="Please call to discuss", Details="We have a situation here!", WrittenBy="Marie", CustomerEmail="elenarpal@gmail.com", Patient=patients[2] }
                };

            foreach (Message message in messages)
            {
                context.Set<Message>().Add(message);
            }


            List<Event> events = new List<Event>
                {
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Christmas Party", EventDetails="Hi all, Tara Winthrop has setup the Christmas party onn the 16th of December, All welcome",  EventDate= new DateTime(2016,11,10,10,0,0), Location = locations[0] },
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Charity Dayy", EventDetails="Balbriggan Private Clinic has a Charity event on the 24th of November @ 16.00, All welcome!",  EventDate= new DateTime(2016,12,11,10,0,0) ,Location = locations[1] } ,
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Halloween Party", EventDetails="Swords Clinic has a Halloween Party on the 31st of October @20.oo hours, All welcome!",  EventDate= new DateTime(2016,09,29,10,0,0), Location = locations[2] },
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Charity Party", EventDetails="Tara Winthrop has a Charity event on Sunday the 24 of September @16.00 hours, All welcome!",  EventDate= new DateTime(2016,11,10,10,0,0), Location = locations[0] },
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Charity Dayy", EventDetails="Balbriggan Private Clinic has a Charity event on the 10th of December @ 20.00, All welcome!",  EventDate= new DateTime(2016,10,10,10,0,0) ,Location = locations[1] } ,
                    new Event {Id = Guid.NewGuid().ToString(),EventTitle="Halloween Party", EventDetails="Halloween Party will be held in the Thrinity Capital hotel on the 31 st @17.00hours.",  EventDate= new DateTime(2016,11,29,10,0,0), Location = locations[2] }
                };

            foreach (Event ev in events)
            {
                context.Set<Event>().Add(ev);
            }



            base.Seed(context);
        }
    }
}

