using HomeFinder.Models;
using HomeFinder.RoleModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HomeFinder.Data
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HomeFinderContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<HomeFinderContext>>()))
            {

                if (context.Users.Any())
                {
                    return;
                }

                var leaseType = new LeaseType { Description = "Du äger bostaden", Name = "Bostadsrätt" };
                var apartment = new PropertyType { IconUrl = "https://cdn.pixabay.com/photo/2021/10/11/23/49/building-6702046_960_720.png", PropertyTypeName = PropertyTypeName.Apartment };
                var useTwiceAdress = new Address { City = "Malmö", Country = "Sverige", PostalCode = "211 55", StreetAddress = "Hantverkaregatan 13" };

                //Lösenord för alla: 1!(FirstName) tex. 1!Gabriel

                // Gabriel är nu insatt som Admin i databasen
                // Sanna och Rikard är insatta som Realtor

                context.Users.AddRange(
                    new HomeFinderUser
                    {
                        // ADMIN

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
                        // Realtor

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
                        // Realtor

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
                        ConcurrencyStamp = "022d4d3e-9233-4bf5-bdf9-f1fe95ee8cd5",
                        PhoneNumber = "073 564 71 88",
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnd = null,
                        LockoutEnabled = true,
                        AccessFailedCount = 0

                    }
                );
                await context.SaveChangesAsync();


                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                    await roleStore.CreateAsync(new IdentityRole { Name = "Realtor", NormalizedName = "REALTOR" });

                    var userStore = new UserStore<HomeFinderUser>(context);
                    foreach (var user in context.Users)
                    {
                        if (user.Id == "f3c9115a-a0a4-4c4e-aa8a-ec960749556c")
                        {
                            await userStore.AddToRoleAsync(user, "Admin");
                        }

                        if (user.Id == "c1a35e52-29f0-4ba3-968f-d1d6b89483fe" || user.Id == "a73917c6-5cd3-47a2-88bb-e76c41840137")
                        {
                            await userStore.AddToRoleAsync(user, "Realtor");
                        }
                    }
                }


                context.PropertyObjects.AddRange(
                    new PropertyObject
                    {
                        Address = new Address { City = "Malmö", Country = "Sverige", PostalCode = "211 55", StreetAddress = "Hantverkaregatan 13" },
                        Area = 34,
                        Description = "Finare etta går ej att hitta",
                        Images = { new HomeFinderImages { AltText = "Utan möbler", Path = "~/Images/lichtraum-1560788_960_720.webp" }, new HomeFinderImages { AltText = "Miljö utanför", Path = "https://cdn.pixabay.com/photo/2018/06/17/11/31/modern-buildings-3480351_960_720.jpg" } },
                        LeaseType = leaseType,
                        ListPrice = 100000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 1,
                        PropertyType = apartment,
                        RealtorId = "c1a35e52-29f0-4ba3-968f-d1d6b89483fe",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 1940
                    },
                    new PropertyObject
                    {
                        Address = useTwiceAdress,
                        Area = 70,
                        Description = "Fin trea centralt i malmö",
                        Images = { new HomeFinderImages { AltText = "Vardagsrum", Path = "~/Images/dining-room-3108037_960_720.webp" }, new HomeFinderImages { AltText = "Badrum", Path = "https://cdn.pixabay.com/photo/2018/01/29/07/55/modern-minimalist-bathroom-3115450_960_720.jpg" } },
                        LeaseType = leaseType,
                        ListPrice = 300000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 3,
                        PropertyType = apartment,
                        RealtorId = "c1a35e52-29f0-4ba3-968f-d1d6b89483fe",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 1940
                    },
                    new PropertyObject
                    {
                        Address = new Address { City = "Malmö", Country = "Sverige", PostalCode = "212 91", StreetAddress = "Fårabäcksvägen 95" },
                        Area = 180,
                        Description = "Fint hus strax utanför malmö",
                        Images = { new HomeFinderImages { AltText = "Utanför huset", Path = "~/Images/facade-4304096_960_720.webp" }, new HomeFinderImages { AltText = "Vardagsrum", Path = "~/Images/architecture-5339245_960_720.jpg" } },
                        LeaseType = leaseType,
                        ListPrice = 500000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 5,
                        PropertyType = new PropertyType { IconUrl = "https://cdn.pixabay.com/photo/2021/07/02/04/48/home-6380863_960_720.png", PropertyTypeName = PropertyTypeName.House },
                        RealtorId = "c1a35e52-29f0-4ba3-968f-d1d6b89483fe",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 2000
                    },
                    new PropertyObject
                    {
                        Address = new Address { City = "Stockholm", Country = "Sverige", PostalCode = "118 28", StreetAddress = "Fatburs brunnsgata 26" },
                        Area = 40,
                        Description = "Liten men fräsch tvåa",
                        Images = { new HomeFinderImages { AltText = "Fick med nästan allt", Path = "~/Images/kitchen-4043098_960_720.webp" }, new HomeFinderImages { AltText = "Innergård", Path = "~/Images/buildings-1336611_960_720.webp" } },
                        LeaseType = leaseType,
                        ListPrice = 4000000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 2,
                        PropertyType = apartment,
                        RealtorId = "a73917c6-5cd3-47a2-88bb-e76c41840137",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 1991
                    },
                    new PropertyObject
                    {
                        Address = new Address { City = "Stockholm", Country = "Sverige", PostalCode = "112 49", StreetAddress = "Igeldammsgatan 14" },
                        Area = 60,
                        Description = "Alldeles perfekt trea",
                        Images = { new HomeFinderImages { AltText = "Kök och vardagsrum", Path = "~/Images/home-5835289_960_720.webp" }, new HomeFinderImages { AltText = "Sovrum", Path = "~/Images/bedroom-416062_960_720.webp" } },
                        LeaseType = leaseType,
                        ListPrice = 5000000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 3,
                        PropertyType = apartment,
                        RealtorId = "a73917c6-5cd3-47a2-88bb-e76c41840137",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 1928
                    },
                    new PropertyObject
                    {
                        Address = new Address { City = "Stockholm", Country = "Sverige", PostalCode = "120 56", StreetAddress = "Sandfjärdsgatan 60" },
                        Area = 140,
                        Description = "Radhus i familjevänligt område",
                        Images = { new HomeFinderImages { AltText = "", Path = "~/Images/architecture-3383067_960_720.webp" }, new HomeFinderImages { AltText = "Vardagsrum", Path = "~/Images/living-room-5570510_960_720.jpg" } },
                        LeaseType = leaseType,
                        ListPrice = 3500000,
                        LotArea = 0,
                        NextShowingDateTime = DateTime.Now.Date,
                        NonLivingArea = 0,
                        NumberOfRooms = 5,
                        PropertyType = new PropertyType { IconUrl = "https://cdn.pixabay.com/photo/2020/03/10/09/11/architecture-4918393_960_720.png", PropertyTypeName = PropertyTypeName.Townhouse },
                        RealtorId = "a73917c6-5cd3-47a2-88bb-e76c41840137",
                        Status = 0,
                        UploadedDate = DateTime.Now.Date,
                        YearBuilt = 2021
                    }
                );
                context.SaveChanges();
                var propertyObjects = context.PropertyObjects.ToList();
                var users = context.Users.ToList();

                context.NoticeOfInterests.AddRange(
                    new NoticeOfInterest { PropertyObject = propertyObjects[4], User = users[0] },
                    new NoticeOfInterest { PropertyObject = propertyObjects[3], User = users[3] },
                    new NoticeOfInterest { PropertyObject = propertyObjects[2], User = users[3] }
                );
                context.PropertyFavorited.AddRange(
                    new PropertyFavoritedByUser { PropertyObject = propertyObjects[4], User = users[0] },
                    new PropertyFavoritedByUser { PropertyObject = propertyObjects[3], User = users[0] },
                    new PropertyFavoritedByUser { PropertyObject = propertyObjects[0], User = users[0] },
                    new PropertyFavoritedByUser { PropertyObject = propertyObjects[5], User = users[3] },
                    new PropertyFavoritedByUser { PropertyObject = propertyObjects[2], User = users[3] }
                );


                context.SaveChanges();


            }
        }
    }
}
