using Faker.Example;

Faker.Core.Faker faker = new();
int i = faker.Create<int>();
Console.WriteLine(i);

Faker.Core.Config.FakerConfig config = new();
config.Add<User, string, NameGenerator>(u => u.Name);
config.Add<User, int, AgeGenerator>(u => u.Age);
Faker.Core.Faker faker2 = new(config);
User user = faker2.Create<User>();
Console.WriteLine(user);