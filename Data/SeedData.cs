using HomeFinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HomeFinder.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HomeFinderContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<HomeFinderContext>>()))
            {
                
                if (context.Users.Any())
                {
                    return;   
                }

                //Lösenord för alla: 1!(FirstName) tex. 1!Gabriel
                context.Users.AddRange(
                    new HomeFinderUser
                    {
                        Id = "f3c9115a-a0a4-4c4e-aa8a-ec960749556c",
                        FirstName = "Gabriel",
                        LastName = "Andersson",
                        Company = null,
                        Address = new Address { City = "Malmö", Country = "Sverige", PostalCode = "211 57", StreetAddress = "S:t Knuts torg 2" },
                        UserName = "Gabriel@mail.com",
                        NormalizedUserName = "GABRIEL@MAIL.COM",
                        Email = "Gabriel@mail.com",
                        NormalizedEmail = "GABRIEL@MAIL.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAELqH5OivfUkkEo43tOX7EtksgP7hk6f1mOtyCyyyd4vJAO8mShhxt6+p2+o1FRkskg==",
                        SecurityStamp = "KJNI4I3ILXP2RCKYC2RJYXWJ2OK4FALD",
                        ConcurrencyStamp = "e7ba4fa1-235a-4801-8894-5aa7dd663eea",
                        PhoneNumber = "026133445",
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0

                    },
                    new HomeFinderUser
                    {
                        Id = "2d8afe96-7228-4c88-8d32-07a780330df8",
                        FirstName = "Sonja",
                        LastName = "Pettersson",
                        Company = null,
                        Address = new Address { City = "Stockholm", Country = "Sverige", PostalCode = "113 20", StreetAddress = "Sankt eriksplan 6 B" },
                        UserName = "Sonja@mail.com",
                        NormalizedUserName = "SONJA@MAIL.COM",
                        Email = "Sonja@mail.com",
                        NormalizedEmail = "SONJA@MAIL.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAEHAnrLjhvimxIrKMstW4FPOmpv59SAhfiwR2/aNz70bSStC6CYuV4r+YrMqxz+rkfQ==",
                        SecurityStamp = "N2LSBBOEG4YIDKEUS7BAIDG3BFNR32IM",
                        ConcurrencyStamp = "a1ea3059-eccb-43e2-9d79-dbc54318a0db",
                        PhoneNumber = "073 345 22 11",
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0
                    },
                    new HomeFinderUser
                    {
                        Id = "a73917c6-5cd3-47a2-88bb-e76c41840137",
                        FirstName = "Rikard",
                        LastName = "Svensson",
                        Company = new Company { Name = "BestSales", OrgNumber = "221177-4321" },
                        Address = new Address { City = "Stockholm", Country = "Sverige", PostalCode = "111 31", StreetAddress = "Trädgårdsgatan 3" },
                        UserName = "Rikard@BestSales.org",
                        NormalizedUserName = "RIKARD@BESTSALES.ORG",
                        Email = "Rikard@BestSales.org",
                        NormalizedEmail = "RIKARD@BESTSALES.ORG",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAEEHvyuliyGkoSjwPCLIT+ZZNUCi4/d0M7IqcgpZ6RX/Fej4Rp40THYlxzv09NWn8YQ==",
                        SecurityStamp = "MWONV6UEKGOKZL54NFVNPPYYXC5W6V7K",
                        ConcurrencyStamp = "1d454a93-6143-49ff-8d40-ec8f8dca58f9",
                        PhoneNumber = "073 860 62 02",
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0

                    },

                   new HomeFinderUser
                   {
                       Id = "c1a35e52-29f0-4ba3-968f-d1d6b89483fe",
                       FirstName = "Sanna",
                       LastName = "Bergström",
                       Company = new Company { Name = "Property4Sale", OrgNumber = "771122-1234" },
                       Address = new Address { City = "Malmö", Country = "Sverige", PostalCode = "211 11", StreetAddress = "Skeppsgatan 19" },
                       UserName = "Sanna@Property4Sale.com",
                       NormalizedUserName = "SANNA@PROPERTY4SALE.COM",
                       Email = "Sanna@Property4Sale.com",
                       NormalizedEmail = "SANNA@PROPERTY4SALE.COM",
                       EmailConfirmed = true,
                       PasswordHash = "AQAAAAEAACcQAAAAEKZaVKwPMIl0mGbP9DoDu++n1dbuGje2STw/ndWCHdg4Pu6vHt/pBkhXcufAPRcsqA==",
                       SecurityStamp = "CCZVULVOY3BEO7LC6ZRHNBDBKK6S74J5",
                       ConcurrencyStamp= "022d4d3e-9233-4bf5-bdf9-f1fe95ee8cd5",
                       PhoneNumber = "073 564 71 88",
                       PhoneNumberConfirmed = false,
                       TwoFactorEnabled = false,
                       LockoutEnd = null,
                       LockoutEnabled = true,
                       AccessFailedCount = 0

                   }
                );  
                context.SaveChanges();
            }
        }
    }
}
