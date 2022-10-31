using ExpensesData.Context;
using ExpenseTracker.Bot.Options;
using ExpenseTracker.Bot.Repositories;
using ExpenseTracker.Bot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExpensesDbContext>(option => 
        option.UseSqlite(builder.Configuration.GetConnectionString("ExpansesBotDb")));

builder.Services.Configure<ExpensesBotOptions>(
    builder.Configuration.GetSection(nameof(ExpensesBotOptions)));


builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<RoomsRepository>();
builder.Services.AddScoped<TelegramBotService>();
builder.Services.AddScoped<OutlaysRepository>();
builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

//https://api.telegram.org/bot5377466927:AAESOaA7GSxVkjopO6S0GlDmlEepTTEQwno/setwebhook?url=https://677c-84-54-86-1.eu.ngrok.io/bot

//https://0af7-84-54-86-1.eu.ngrok.io/bot