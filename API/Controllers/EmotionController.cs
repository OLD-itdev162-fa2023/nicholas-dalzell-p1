using Domain;
using Persistence;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmotionForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Happy", "Sad", "Angry", "Excited", "Emotional", "Nervous", "Anxious", "Elated", "Confused"
    };

    private readonly ILogger<EmotionForecastController> _logger;

    private readonly DataContext _context;

    public EmotionForecastController(ILogger<EmotionForecastController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetEmotionForecast")]
    public IEnumerable<EmotionForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new EmotionForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public ActionResult<EmotionForecast> Create()
    {
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine("Insert a new Emotion");

        var forecast = new EmotionForecast()
        {
            Date = new DateTime(),
            TemperatureC= 75,
            Summary = "Tired"
        };

        _context.EmotionForecasts.Add(forecast);
        var success = _context.SaveChanges() > 0;

        if (success) {
            return forecast;
        }

        throw new Exception("Error creating EmotionForecast");
    }
}
