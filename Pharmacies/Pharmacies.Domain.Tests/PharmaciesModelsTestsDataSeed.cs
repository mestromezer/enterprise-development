using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.Domain.Tests;

public static class PharmaciesModelsTestsDataSeed
{
    public static List<Pharmacy> Pharmacies { get; } =
    [
        new Pharmacy
        {
            Number = 1, Name = "Аптека №1", Phone = "123-456-7890", Address = "Улица Ленина, 12, Красноглинский район",
            DirectorFullName = "Иванов Иван Иванович"
        },

        new Pharmacy
        {
            Number = 2, Name = "Аптека №2", Phone = "098-765-4321", Address = "Проспект Мира, 55, Центральный район",
            DirectorFullName = "Петров Петр Петрович"
        },

        new Pharmacy
        {
            Number = 3, Name = "Аптека №3", Phone = "123-789-4560", Address = "Улица Гагарина, 33, Заводской район",
            DirectorFullName = "Сидоров Сидор Сидорович"
        },

        new Pharmacy
        {
            Number = 4, Name = "Аптека №4", Phone = "321-654-0987", Address = "Улица Пушкина, 10, Октябрьский район",
            DirectorFullName = "Кузнецова Анна Сергеевна"
        },

        new Pharmacy
        {
            Number = 5, Name = "Аптека №5", Phone = "543-210-9876", Address = "Проспект Победы, 89, Кировский район",
            DirectorFullName = "Коваленко Максим Игоревич"
        }
    ];


    public static List<Price> Prices { get; } =
    [
        new Price
        {
            Manufacturer = "Bayer", ProductionTime = new DateTime(2023, 5, 10), IfCash = true,
            SellerOrganizationName = "ООО \"Аптеки Красноглинского района\"", Cost = 150.00m, SellTime = new DateTime(2023, 9, 1)
        },

        new Price
        {
            Manufacturer = "Pharma Inc.", ProductionTime = new DateTime(2023, 7, 20), IfCash = false,
            SellerOrganizationName = "ООО \"Аптеки Красноглинского района\"", Cost = 200.00m, SellTime = new DateTime(2023, 10, 1)
        },

        new Price
        {
            Manufacturer = "Novartis", ProductionTime = new DateTime(2023, 4, 15), IfCash = true,
            SellerOrganizationName = "ООО \"Аптеки Красноглинского района\"", Cost = 120.50m, SellTime = new DateTime(2023, 8, 15)
        },

        new Price
        {
            Manufacturer = "Pfizer", ProductionTime = new DateTime(2023, 6, 1), IfCash = false,
            SellerOrganizationName = "ООО \"Аптеки Куйбышевского района\"", Cost = 300.75m, SellTime = new DateTime(2023, 9, 30)
        },

        new Price
        {
            Manufacturer = "AstraZeneca", ProductionTime = new DateTime(2023, 3, 18), IfCash = true,
            SellerOrganizationName = "ООО \"Аптеки Куйбышевского района\"", Cost = 250.00m, SellTime = new DateTime(2023, 9, 5)
        }
    ];

    public static List<Position> Positions { get; } =
    [
        new Position
        {
            Code = 101,
            Name = "Аспирин",
            ProductGroup = new ProductGroup { Id = 1, Name = "Обезболивающие" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 1, Name = "НПВС" }],
            Quantity = 50,
            Pharmacy = Pharmacies[0], // Аптека №1
            Price = Prices[0] // Цена от Bayer
        },

        new Position
        {
            Code = 102,
            Name = "Парацетамол",
            ProductGroup = new ProductGroup { Id = 2, Name = "Жаропонижающие" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 1, Name = "НПВС" }],
            Quantity = 100,
            Pharmacy = Pharmacies[1], // Аптека №2
            Price = Prices[1] // Цена от Pharma Inc.
        },

        new Position
        {
            Code = 103,
            Name = "Амоксициллин",
            ProductGroup = new ProductGroup { Id = 3, Name = "Антибиотики" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 2, Name = "Антибактериальные" }],
            Quantity = 30,
            Pharmacy = Pharmacies[2], // Аптека №3
            Price = Prices[2] // Цена от Novartis
        },

        new Position
        {
            Code = 104,
            Name = "Ибупрофен",
            ProductGroup = new ProductGroup { Id = 1, Name = "Обезболивающие" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 1, Name = "НПВС" }],
            Quantity = 60,
            Pharmacy = Pharmacies[3], // Аптека №4
            Price = Prices[3] // Цена от Pfizer
        },

        new Position
        {
            Code = 105,
            Name = "Цетиризин",
            ProductGroup = new ProductGroup { Id = 4, Name = "Антигистаминные" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 3, Name = "Противоаллергические" }],
            Quantity = 40,
            Pharmacy = Pharmacies[4], // Аптека №5
            Price = Prices[4] // Цена от AstraZeneca
        },

        new Position
        {
            Code = 106,
            Name = "Омепразол",
            ProductGroup = new ProductGroup { Id = 5, Name = "Противоязвенные" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 4, Name = "Ингибиторы протонной помпы" }],
            Quantity = 25,
            Pharmacy = Pharmacies[0], // Аптека №1
            Price = Prices[0] // Цена от Bayer
        },

        new Position
        {
            Code = 107,
            Name = "Метформин",
            ProductGroup = new ProductGroup { Id = 6, Name = "Гипогликемические" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 5, Name = "Противодиабетические" }],
            Quantity = 80,
            Pharmacy = Pharmacies[1], // Аптека №2
            Price = Prices[1] // Цена от Pharma Inc.
        },

        new Position
        {
            Code = 108,
            Name = "Лоперамид",
            ProductGroup = new ProductGroup { Id = 7, Name = "Противодиарейные" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 6, Name = "Противодиарейные" }],
            Quantity = 90,
            Pharmacy = Pharmacies[2], // Аптека №3
            Price = Prices[2] // Цена от Novartis
        },

        new Position
        {
            Code = 109,
            Name = "Дротаверин",
            ProductGroup = new ProductGroup { Id = 8, Name = "Спазмолитики" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 7, Name = "Спазмолитики" }],
            Quantity = 70,
            Pharmacy = Pharmacies[3], // Аптека №4
            Price = Prices[3] // Цена от Pfizer
        },

        new Position
        {
            Code = 110,
            Name = "Аллопуринол",
            ProductGroup = new ProductGroup { Id = 9, Name = "Противоподагрические" },
            PharmaceuticalGroups = [new PharmaceuticalGroup { Id = 8, Name = "Противоподагрические" }],
            Quantity = 35,
            Pharmacy = Pharmacies[4], // Аптека №5
            Price = Prices[4] // Цена от AstraZeneca
        }
    ];
}