using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseManager.Models
{
    public class WMRepository : IWMRepository
    {
        private readonly MySqlDbContext dbContext;

        public WMRepository(MySqlDbContext context) => dbContext = context;

        public IQueryable<Client> Clients => dbContext.Clients;
        public IQueryable<Product> Products => dbContext.Products;
        public IQueryable<Stock> Stocks => dbContext.Stocks;
        public IQueryable<Driver> Drivers => dbContext.Drivers;
        public IQueryable<Vehicle> Vehicles => dbContext.Vehicles;
        public IQueryable<Incoming> Incomings => dbContext.Incomings;
        public IQueryable<Shipping> Shippings => dbContext.Shippings;
        public IQueryable<Enhancement> Enhancements => dbContext.Enhancements;

        public bool DbInit(bool sample = false)
        {
            if (dbContext.Database.EnsureCreated())
            {
                if (sample)
                {
                    WMDbSample.Sample(dbContext);
                }
                return true;
            }

            return false;
        }

        public void Create<T>(T entity)
        {
            switch (entity)
            {
                case Client client:
                    dbContext.Clients.Add(client);
                    break;

                case Driver driver:
                    dbContext.Drivers.Add(driver);
                    break;

                case Enhancement enhancement:
                    Stock baseStock = dbContext.Stocks.Find(enhancement.BaseStockID);
                    Stock finalStock = dbContext.Stocks.Find(enhancement.FinalStockID);

                    if (baseStock == null || finalStock == null)
                    {
                        throw new ArgumentException("No object found");
                    }

                    enhancement.CreatedAt = DateTime.Now;
                    enhancement.NetWeight = enhancement.GrossWeight - (dbContext.Vehicles?.Find(enhancement.VehicleID).Tare ?? 0);
                    baseStock.Balance -= enhancement.NetWeight;
                    finalStock.Balance += enhancement.NetWeight;

                    dbContext.Enhancements.Add(enhancement);
                    dbContext.Stocks.Update(baseStock);
                    dbContext.Stocks.Update(finalStock);
                    break;

                case Incoming incoming:
                    Stock incomingStock = dbContext.Stocks.Find(incoming.StockID);

                    if (incomingStock == null)
                    {
                        throw new ArgumentException("No object found");
                    }

                    incoming.CreatedAt = DateTime.Now;
                    incoming.NetWeight = incoming.GrossWeight - (dbContext.Vehicles?.Find(incoming.VehicleID).Tare ?? 0);
                    incomingStock.Balance += incoming.NetWeight;

                    dbContext.Incomings.Add(incoming);
                    dbContext.Stocks.Update(incomingStock);
                    break;

                case Product product:
                    dbContext.Products.Add(product);
                    break;

                case Shipping shipping:
                    Stock shippingStock = dbContext.Stocks.Find(shipping.StockID);

                    if (shippingStock == null)
                    {
                        throw new ArgumentException("No object found");
                    }

                    shipping.CreatedAt = DateTime.Now;
                    shipping.NetWeight = shipping.GrossWeight - (dbContext.Vehicles?.Find(shipping.VehicleID).Tare ?? 0);
                    shippingStock.Balance -= shipping.NetWeight;

                    dbContext.Shippings.Add(shipping);
                    dbContext.Stocks.Update(shippingStock);
                    break;

                case Stock stock:
                    dbContext.Stocks.Add(stock);
                    break;

                case Vehicle vehicle:
                    dbContext.Vehicles.Add(vehicle);
                    break;

                default:
                    throw new ArgumentException($"The object of type {entity.GetType()} is not a valid entity.");
            }

            dbContext.SaveChanges();
        }

        public void Update<T>(T entity)
        {
            switch (entity)
            {
                case Client client:
                    if (!dbContext.Clients.Any(c => c.ID == client.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Clients.Update(client);
                    break;

                case Driver driver:
                    if(!dbContext.Drivers.Any(d => d.ID == driver.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Drivers.Update(driver);
                    break;

                case Product product:
                    if (!dbContext.Products.Any(d => d.ID == product.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Products.Update(product);
                    break;

                case Vehicle vehicle:
                    if (!dbContext.Vehicles.Any(d => d.ID == vehicle.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Vehicles.Update(vehicle);
                    break;

                case Enhancement e:
                    break;

                case Incoming i:
                    break;

                case Shipping s:
                    break;

                case Stock s:
                    break;

                default:
                    throw new ArgumentException($"The object of type {entity.GetType()} is not a valid entity.");
            }

            dbContext.SaveChanges();
        }

        public void Delete<T>(T entity)
        {
            switch (entity)
            {
                case Client client:
                    if (!dbContext.Clients.Any(c => c.ID == client.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Clients.Remove(client);
                    break;
                
                case Driver driver:
                    if (!dbContext.Drivers.Any(c => c.ID == driver.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Drivers.Remove(driver);
                    break;
                
                case Enhancement enhancement:
                    if (!dbContext.Enhancements.Any(c => c.ID == enhancement.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Enhancements.Remove(enhancement);
                    break;
                
                case Incoming incoming:
                    if (!dbContext.Incomings.Any(c => c.ID == incoming.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Incomings.Remove(incoming);
                    break;
                
                case Product product:
                    if (!dbContext.Products.Any(c => c.ID == product.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Products.Remove(product);
                    break;
                
                case Shipping shipping:
                    if (!dbContext.Shippings.Any(c => c.ID == shipping.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Shippings.Remove(shipping);
                    break;
                
                case Stock stock:
                    if (!dbContext.Stocks.Any(c => c.ID == stock.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Stocks.Remove(stock);
                    break;
                
                case Vehicle vehicle:
                    if (!dbContext.Vehicles.Any(c => c.ID == vehicle.ID))
                    {
                        throw new ArgumentException("Invalid object reference");
                    }

                    dbContext.Vehicles.Remove(vehicle);
                    break;
                
                default:
                    throw new ArgumentException($"The object of type {entity.GetType()} is not a valid entity.");
            }

            dbContext.SaveChanges();
        }
    }

    public static class WMDbSample
    {
        public static void Sample(MySqlDbContext dbContext)
        {
            Client[] clients = new Client[]
            {
                new Client { Name = "Client 1", Address = "Client Address 1" },
                new Client { Name = "Client 2", Address = "Client Address 2" },
                new Client { Name = "Client 3", Address = "Client Address 3" },
                new Client { Name = "Client 4", Address = "Client Address 4" },
                new Client { Name = "Client 5", Address = "Client Address 5" }
            };
            foreach (var item in clients)
            {
                dbContext.Clients.Add(item);
            }
            dbContext.SaveChanges();

            Product[] products = new Product[]
            {
                new Product { Name = "Product 1", Description = "Product description 1" },
                new Product { Name = "Product 2", Description = "Product description 2" },
                new Product { Name = "Product 3", Description = "Product description 3" },
            };
            foreach (var item in products)
            {
                dbContext.Products.Add(item);
            }
            dbContext.SaveChanges();

            Stock[] stocks = new Stock[]
            {
                new Stock { ProductID = products[0].ID, ClientID = clients[0].ID, Balance = 50 },
                new Stock { ProductID = products[1].ID, ClientID = clients[1].ID, Balance = 50 },
                new Stock { ProductID = products[2].ID, ClientID = clients[2].ID, Balance = 50 },
                new Stock { ProductID = products[0].ID, ClientID = clients[3].ID, Balance = 50 },
                new Stock { ProductID = products[2].ID, ClientID = clients[4].ID, Balance = 50 },
                new Stock { ProductID = products[1].ID, ClientID = clients[2].ID, Balance = 50 },
            };
            foreach (var item in stocks)
            {
                dbContext.Stocks.Add(item);
            }
            dbContext.SaveChanges();

            Driver[] drivers = new Driver[]
            {
                new Driver { Name = "Driver 1", CNH = "12345678901" },
                new Driver { Name = "Driver 2", CNH = "12345678902" },
                new Driver { Name = "Driver 3", CNH = "12345678903" },
                new Driver { Name = "Driver 4", CNH = "12345678904" }
            };
            foreach (var item in drivers)
            {
                dbContext.Drivers.Add(item);
            }
            dbContext.SaveChanges();

            Vehicle[] vehicles = new Vehicle[]
            {
                new Vehicle { Plate1 = "AB1C23", Plate2 = "", Plate3 = "", RNTRC = "01234567", Tare = 13200 },
                new Vehicle { Plate1 = "AB1C24", Plate2 = "AB2C34", Plate3 = "", RNTRC = "01234568", Tare = 14350 },
                new Vehicle { Plate1 = "AB1C25", Plate2 = "AB2C35", Plate3 = "", RNTRC = "01234569", Tare = 13900 },
                new Vehicle { Plate1 = "AB1C26", Plate2 = "AB2C36", Plate3 = "AB3C45", RNTRC = "01234560", Tare = 18150 }
            };
            foreach (var item in vehicles)
            {
                dbContext.Vehicles.Add(item);
            }
            dbContext.SaveChanges();
        }
    }
}