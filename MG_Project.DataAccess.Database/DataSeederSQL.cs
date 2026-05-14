using MG_Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG_Project.DataAccess.Database
{
    public static class DataSeederSQL
    {
        public static void Seed(MGProjectDbContext context)
        {
            // 1. NAJWAŻNIEJSZE: Sprawdź, czy dane już istnieją!
            // Jeśli w tabeli Firm jest cokolwiek, przerywamy.
            if (context.Firms.Any())
            {
                return;
            }

            // 2. Jeśli baza pusta - tworzymy dane
            var address = new Address("Startowa", "Warszawa", "Polska", "00-001", 1, null);

            var firm = new Firm("Firma Startowa SQL", address);

            var emp = new Employee(
                "Adam",
                "Nowak",
                "80010112345",
                new DateTime(1980, 1, 1),
                address,
                "Admin",
                5000,
                0,
                DateTime.Now,
                firm.FirmId,  // FK do firmy
                firm,         // Obiekt firmy
                null,         // FK do działu
                null          // Obiekt działu
            );

            // 3. Dodajemy do DbContext
            context.Firms.Add(firm);
            context.Employees.Add(emp);

            // 4. Zapisujemy transakcję
            context.SaveChanges();
        }
    }
}