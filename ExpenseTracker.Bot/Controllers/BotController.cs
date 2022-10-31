using ExpensesData.Entities;
using ExpenseTracker.Bot.Repositories;
using ExpenseTracker.Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = ExpensesData.Entities.User;

namespace ExpenseTracker.Bot.Controllers;

[ApiController]
[Route("bot")]
public class BotController : ControllerBase
{

    public readonly UsersRepository _usersRepository;
    public readonly TelegramBotService _botService;
    public readonly RoomsRepository _roomsRepository;
    public readonly OutlaysRepository _outlaysRepository;
    public BotController(UsersRepository usersRepository,
                        TelegramBotService botService,
                        RoomsRepository roomsRepository,
                        OutlaysRepository outlays)
    {
        _outlaysRepository = outlays;
        _usersRepository = usersRepository;
        _botService = botService;
        _roomsRepository = roomsRepository;
    }

    [HttpGet]
    public IActionResult GetMe() => Ok("Working...");

    [HttpPost]
    public async Task PostUpdate(Update update)
    {
        if (update.Type is not UpdateType.Message)
            return;

        var (chatId, message, name) = GetValues(update);

        var user = await FilterUser(chatId, name);

        switch ((EStep)user.Step)
        {
            case EStep.Default: await DefaultStep(user, message); break;

            case EStep.EnterNewRoomName: await CreateNewRoom(user, message); break;

            case EStep.EnterJoinRoomKey: break;

            case EStep.InRoom: await InRoomStep(user, message); break;

            case EStep.EnterOutlayCost: await AddingOutlaCost(user, message); break;
            case EStep.EnterOutlayDescription: await EnteringOutlayDescription(user, message); break;
        }
    }

    public async Task DefaultStep(User user, string messageFromUser)
    {
        switch (messageFromUser)
        {
            case "Create a new room":
                await SetNextStep(user, EStep.EnterNewRoomName,
                                                          "Enter name for a new room"); break;
            case "Join to an existing room":
                await SetNextStep(user, EStep.EnterJoinRoomKey,
                                                          "Enter the key(room's name) for joining to a room"); break;
            default:
                await SendMenu();
                break;
        }

        async Task SendMenu()
        {
            var menu = new List<string>() { "Create a new room", "Join to an existing room" };
            await _botService.SendMessage(user.ChatId, "Choose the menu ⬇️", _botService.GetKeyboardButtons(menu));
        }
    }

    public async Task CreateNewRoom(User user, string messageFromUser)
    {
        var room = new Room
        {
            Name = messageFromUser,
            Key = Guid.NewGuid().ToString("N")[..10],
            Status = RoomStatus.Created,
        };
        await _roomsRepository.AddRoomAsync(room);

        user.RoomId = room.Id;
        user.isAdmin = true;
        user.Step = (int)EStep.InRoom;

        await _usersRepository.UpdateUser(user);

        var menu = new List<string>() { "Add outlay", "Calculate" };
        await _botService.SendMessage(user.ChatId, "Choose the menu ⬇️", _botService.GetKeyboardButtons(menu));
    }

    public async Task InRoomStep(User user, string messageFromUser)
    {
        switch (messageFromUser)
        {
            case "Add outlay": await SetNextStep(user, EStep.EnterOutlayCost, "Enter an outlay ⬇️"); break;

            case "Calculate": await GetTotalCost(); break;

            default:
                await SendMenu();
                break;
        }

        async Task GetTotalCost()
        {
            await _outlaysRepository.CalculateOytlaysAsync();
        }

        async Task SendMenu()
        {
            var menu = new List<string>() { "Add outlay", "Calculate" };
            await _botService.SendMessage(user.ChatId, "Choose the menu ⬇️", _botService.GetKeyboardButtons(menu));
        }
    }

    public async Task EnteringOutlayDescription(User user, string messageFromUser)
    {

        await SetNextStep(user, EStep.EnterOutlayDescription, "Enter description :");
        var outlay = new Outlay
        {
            Description = messageFromUser,
        };
        await _outlaysRepository.UpdateOutlayByRoomName(messageFromUser, outlay);

    }
    public async Task AddingOutlaCost(User user, string messageFromUser)
    {
        switch (messageFromUser)
        {
            case "Add outlay": await AddOutlay(); break;
            default:
               await SendMenu();
                break;
        }

        async Task AddOutlay()
        {
            var room = _roomsRepository.GetRoomByChatId(user.ChatId);
            user.Step = 5;
            var outlay = new Outlay
            {
                Cost = int.Parse(messageFromUser),
                RoomId = room.Id,
                UserId = user.Id,
                User = user,
            };
            await _outlaysRepository.AddOutlaysAsync(outlay);
        }

        async Task SendMenu()
        {
            var menu = new List<string>() { "Back", "Add Description" };
            await _botService.SendMessage(user.ChatId, "", _botService.GetKeyboardButtons(menu));
        }
    }

    


    //Registration methods -------------------------------------------------
    private Tuple<long, string, string> GetValues(Update update)
    {
        var chatId = update.Message!.From!.Id;
        var message = update.Message.Text;
        var name = update.Message!.From!.Username ?? update.Message.From.FirstName;

        return new(chatId, message!, name);
    }

    private async Task<User> FilterUser(long chatId, string userName)
    {
        var user = await _usersRepository.GetUserByChatId(chatId);

        if (user is null)
        {
            user = new User
            {
                ChatId = chatId,
                Name = userName,
            };

            await _usersRepository.AddUserAsync(user);
        }
        return user;

    }

    private async Task SetNextStep(User user, EStep nextStep, string stepMessage)
    {
        await UpdateUserStep(user, nextStep);
        await _botService.SendMessage(user.ChatId, stepMessage);
    }

    private async Task UpdateUserStep(User user, EStep step)
    {
        user.Step = (int)step; //enternewRoom
        await _usersRepository.UpdateUser(user);
    }
}

public enum EStep
{
    Default,
    EnterNewRoomName,
    EnterJoinRoomKey,
    InRoom,
    EnterOutlayCost,
    Calculate,
    EnterOutlayDescription,
}
