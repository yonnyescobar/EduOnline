using EduOnline.DAL.Entities;
using EduOnline.Enums;
using EduOnline.Helpers;

namespace EduOnline.DAL
{
    public class SeederDb
    {
        private readonly DatabaseContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public SeederDb(DatabaseContext context, IUserHelper userHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _azureBlobHelper = azureBlobHelper;
        }

        public async Task SeederAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await PopulateCategoriesAsync();
            await PopulateCountriesAsync();
            await PopulateRolesAsync();
            await PopulateUserAsync("Admin", "Local", "admin_local@yopmail.com", "3002323232", "102030", "NoPhoto.png", UserType.Administrador);
            await PopulateUserAsync("Estudiante", "Local", "estudiante_local@yopmail.com", "4005656656", "405060", "NoPhoto.png", UserType.Estudiante);

            await _context.SaveChangesAsync();
        }

        private async Task PopulateCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Desarrollo Web", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category { Name = "Ciencias de la Información", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category { Name = "Desarrollo Móvil", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category { Name = "Gestión de Proyectos", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category { Name = "Analítica e Inteligencia Empresarial", CreatedDate = DateTime.Now });
                _context.Categories.Add(new Category { Name = "Marketing Digital", CreatedDate = DateTime.Now });
            }
        }
        private async Task PopulateCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    CreatedDate = DateTime.Now,
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "Antioquia",
                            Cities = new List<City>()
                            {
                                new City { Name = "Medellín", CreatedDate = DateTime.Now },
                                new City { Name = "Envigado", CreatedDate = DateTime.Now },
                                new City { Name = "Bello", CreatedDate = DateTime.Now },
                                new City { Name = "Itagüí", CreatedDate = DateTime.Now },
                                new City { Name = "Barbosa", CreatedDate = DateTime.Now },
                                new City { Name = "Copacabana", CreatedDate = DateTime.Now },
                                new City { Name = "Girardota", CreatedDate = DateTime.Now },
                                new City { Name = "Sabaneta", CreatedDate = DateTime.Now },
                            }
                        },

                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "Cundinamarca",
                            Cities = new List<City>()
                            {
                                new City { Name = "Bogotá", CreatedDate = DateTime.Now },
                                new City { Name = "Engativá", CreatedDate = DateTime.Now },
                                new City { Name = "Fusagasugá", CreatedDate = DateTime.Now },
                                new City { Name = "Villeta", CreatedDate = DateTime.Now },
                            }
                        }
                    }
                });

                _context.Countries.Add(new Country
                {
                    CreatedDate = DateTime.Now,
                    Name = "Argentina",
                    States = new List<State>()
                    {
                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "Buenos Aires",
                            Cities = new List<City>()
                            {
                                new City { Name = "Alberti", CreatedDate = DateTime.Now },
                                new City { Name = "Avellaneda", CreatedDate = DateTime.Now },
                                new City { Name = "Bahía Blanca", CreatedDate = DateTime.Now },
                                new City { Name = "Ezeiza", CreatedDate = DateTime.Now },
                            }
                        },

                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "La Pampa",
                            Cities = new List<City>()
                            {
                                new City { Name = "Parera", CreatedDate = DateTime.Now },
                                new City { Name = "Santa Isabel", CreatedDate = DateTime.Now },
                                new City { Name = "Puelches", CreatedDate = DateTime.Now },
                                new City { Name = "La Adela", CreatedDate = DateTime.Now },
                            }
                        }
                    }
                });

                _context.Countries.Add(new Country
                {
                    CreatedDate = DateTime.Now,
                    Name = "Brasil",
                    States = new List<State>()
                    {
                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "Estado de Paraná",
                            Cities = new List<City>()
                            {
                                new City { Name = "Curitiba", CreatedDate = DateTime.Now },
                                new City { Name = "Londrina", CreatedDate = DateTime.Now },
                                new City { Name = "Maringá", CreatedDate = DateTime.Now },
                                new City { Name = "Foz do Iguaçu", CreatedDate = DateTime.Now },
                            }
                        },

                        new State
                        {
                            CreatedDate = DateTime.Now,
                            Name = "Estado de Bahía",
                            Cities = new List<City>()
                            {
                                new City { Name = "Salvador", CreatedDate = DateTime.Now },
                                new City { Name = "Barro Alto", CreatedDate = DateTime.Now },
                                new City { Name = "São Gabriel", CreatedDate = DateTime.Now },
                                new City { Name = "Barreiras", CreatedDate = DateTime.Now },
                            }
                        }
                    }
                });
            }
        }

        private async Task PopulateRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Administrador.ToString());
            await _userHelper.CheckRoleAsync(UserType.Estudiante.ToString());
        }

        private async Task PopulateUserAsync(
            string firstName,
            string lastName,
            string email,
            string phone,
            string document,
            string image,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);

            if (user == null)
            {
                Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync
                    ($"{Environment.CurrentDirectory}\\wwwroot\\images\\{image}", "users");

                user = new User
                {
                    CreatedDate = DateTime.Now,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    PhotoId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
        }
    }
}
