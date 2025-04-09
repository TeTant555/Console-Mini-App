/* 
Client Side
Give client four choices (Stock Menu, Cashier Menu, Manager Menu, Exist)
If 1 is clicked, show 2 choices (Displaying Inventroy, Product Management (Add new product, upgrade the product))
*/
using System.Linq;
using System;
using System.Collections.Generic;

class Program
{
    class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ProfitPerItem { get; set; }
    }

    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    class Sale
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ProfitPerItem { get; set; }
    }
    static List<Product> products = new List<Product>();
    static List<Sale> sales = new List<Sale>();
    static void Main(string[] args)
    {
        InventoryData();
        bool run = true; 
        while (run)
        {
            Console.WriteLine("\n --- Retail Management System --- ");
            Console.WriteLine("1. Stock Menu");
            Console.WriteLine("2. Casher Menu");
            Console.WriteLine("3. Manager Menu");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option >>> ");

            switch (Console.ReadLine().Trim()) 
            {
                case "1": StockMenu(); break;
                case "2": CashierMenu(); break;
                case "3": ManagerMenu(); break;
                case "4": run = false; break;
                default: Console.WriteLine("Invalid option"); break;
            } 
        }
    }

    static void InventoryData()
    {
        products.Add(new Product { Id = 1, Name = "Ruler", Stock = 20, SellingPrice = 400, ProfitPerItem = 40 });
        products.Add(new Product { Id = 2, Name = "Note Book", Stock = 15, SellingPrice = 1200, ProfitPerItem = 120 });
    }

    static void StockMenu()
    {
        Console.WriteLine("\n --- Stock Menu --- ");
        Console.WriteLine("1. View Inventory Data");
        Console.WriteLine("2. Add/Update Product");
        Console.Write("Select an option: ");

        switch (Console.ReadLine())
        {
            case "1":
                foreach (var product in products)
                    Console.WriteLine(
                    $"ID: {product.Id}\n" +
                    $"Name: {product.Name}\n" +
                    $"Stock: {product.Stock}\n" +
                    $"Price: {product.SellingPrice}\n" +
                    $"Profit: {product.ProfitPerItem}\n"
                    );
                    break;
            case "2":
                Console.Write("Enter Product ID: ");
                int id = int.Parse(Console.ReadLine());
                var CUproduct = products.FirstOrDefault(p => p.Id == id);
                if (CUproduct == null) // Add a product 
                {
                    CUproduct = new Product { Id = id };
                    products.Add(CUproduct);
                }                  
                Console.Write("Name: "); CUproduct.Name = Console.ReadLine();
                Console.Write("Stock: "); CUproduct.Stock = int.Parse(Console.ReadLine());
                Console.Write("Selling Price: "); CUproduct.SellingPrice = decimal.Parse(Console.ReadLine());
                Console.Write("Profit per Item: "); CUproduct.ProfitPerItem = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Product saved");
                break;
            default: 
                Console.WriteLine("Invalid Input");
                StockMenu(); break;
        }
    }

/*
When user clicked 2, It will be add order, show order
Add order (Delete the order that the client submit, calculate the cost with amount and value)
Show order (Display the order with selected products and display the amount of cost) 
*/
    static void CashierMenu()
    {
        Console.WriteLine("\n--- Cashier Menu ---");
        List<CartItem> cart = new List<CartItem>();
        string addMore = "y"; 

        do
        {
            Console.Write("Enter Product ID: ");
            int id = int.Parse(Console.ReadLine());
            var purchaseproduct = products.FirstOrDefault(p => p.Id == id);
            if (purchaseproduct == null || purchaseproduct.Stock <= 0)
            {
                Console.WriteLine("Product not found or out of stock");
                continue;
            }

            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());
            if (quantity > purchaseproduct.Stock)
            {
                Console.WriteLine("Insufficient stock");
                continue;
            }

            cart.Add(new CartItem { Product = purchaseproduct, Quantity = quantity });
            Console.Write("Add another item? (y/n): ");
            addMore = Console.ReadLine() ?? "n";
            if (addMore != "y" && addMore != "n") {
                Console.WriteLine("Invalid Input");
                CashierMenu();
            }
        } while (addMore.ToLower() == "y");

        Console.WriteLine("\nOrder Summary:");
        decimal total = 0;
        foreach (var item in cart)
        {
            decimal subtotal = item.Quantity * item.Product.SellingPrice;
            total += subtotal;
            Console.WriteLine($"{item.Product.Name} x{item.Quantity} = {subtotal:C}");
        }
        Console.WriteLine($"Total: {total:C}");

        //Confirm and process payment (Need)
        Console.Write("Confirm payment? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                foreach (var item in cart)
                {
                    // Reduce stock and record the sale
                    item.Product.Stock -= item.Quantity;
                    sales.Add(new Sale
                    {
                        ProductId = item.Product.Id,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        SellingPrice = item.Product.SellingPrice,
                        ProfitPerItem = item.Product.ProfitPerItem
                    });
                }
                Console.WriteLine("Payment completed.");
            }
        }

        static void ManagerMenu()
        {
            // Manager view for sales reporting
            Console.WriteLine("\n--- Sales Report ---");
            foreach (var sale in sales)
            {
                // Show each sale's details
                Console.WriteLine($"Product ID: {sale.ProductId}, Name: {sale.ProductName}, Quantity Sold: {sale.Quantity}, Price: {sale.SellingPrice}, Profit: {sale.ProfitPerItem * sale.Quantity}");
            }

            // Calculate totals
            decimal totalRevenue = sales.Sum(s => s.SellingPrice * s.Quantity);
            decimal totalProfit = sales.Sum(s => s.ProfitPerItem * s.Quantity);
            Console.WriteLine($"\nTotal Revenue: {totalRevenue:C}, Total Profit: {totalProfit:C}");
        }
}
