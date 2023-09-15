using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnetmvcapp.Models;
using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace dotnetmvcapp.Controllers;

public class BookController : Controller
{
    Uri baseaddress = new Uri("http://localhost:8080/api");
    private readonly HttpClient client;

    public BookController()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        client = new HttpClient(clientHandler);
        client.BaseAddress = baseaddress;
    }
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            List<Book> listBooks = new List<Book>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Book").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                listBooks = JsonConvert.DeserializeObject<List<Book>>(data);
            }
            return View(listBooks);
        }
        catch (Exception ex)
        {
            // Log or print the exception to get more details
            Console.WriteLine("Exception: " + ex.Message);

            // Return an error view or another appropriate response
            return View("Error"); // Assuming you have an "Error" view
        }
    }

[HttpGet]
public IActionResult Search(int id)
{
    try
    {
        List<Book> listBooks = new List<Book>();
        HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/Book/{id}").Result;

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            Book book = JsonConvert.DeserializeObject<Book>(data);

            if (book != null)
            {
                listBooks.Add(book);
            }
        }

        return View(listBooks);
    }
    catch (Exception ex)
    {
        // Log or print the exception to get more details
        Console.WriteLine("Exception: " + ex.Message);

        // Return an error view or another appropriate response
        return View("Error"); // Assuming you have an "Error" view
    }
}


[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
