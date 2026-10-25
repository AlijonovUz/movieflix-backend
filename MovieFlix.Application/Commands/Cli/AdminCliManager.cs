using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
using MovieFlix.Domain.Entities;
using MovieFlix.Application.Common.Interfaces;
using MovieFlix.Domain.Entities.Enums;
using Spectre.Console;

namespace MovieFlix.Application.Commands.Cli;

public static class AdminCliManager
{
    public static async Task RunAsync(IAppDbContext context)
    {
        while (true)
        {
            AnsiConsole.Clear();
            
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]MovieFlix[/] [grey]• Foydalanuvchilarni boshqarish[/]")
                    .AddChoices(
                        "Foydalanuvchi qo'shish",
                        "Foydalanuvchini o'chirish",
                        "Chiqish"
                    ));

            if (choice == "Chiqish")
            {
                AnsiConsole.MarkupLine("[grey]Konsol boshqaruvi yakunlandi.[/]");
                return;
            }

            switch (choice)
            {
                case "Foydalanuvchi qo'shish":
                    await CreateUserAsync(context);
                    break;
                
                case "Foydalanuvchini o'chirish":
                    await DeleteUserAsync(context);
                    break;
            }
        }
    }

    private static async Task CreateUserAsync(IAppDbContext context)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]Foydalanuvchi qo'shish[/]").LeftJustified());
        AnsiConsole.WriteLine();
        
        var username = AnsiConsole.Ask<string>("Foydalanuvchi nomi: ").Trim();
        
        var userExists = await context.Users.AnyAsync(x => x.Username == username);
        if (userExists)
        {
            ShowError($"Foydalanuvchi nomi '{username}' allaqachon mavjud!");
            return;
        }

        var email = AnsiConsole.Prompt(
            new TextPrompt<string>("Elektron pochta manzil [grey](ixtiyoriy)[/]: ")
                .AllowEmpty());
        email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

        if (email is not null)
        {
            var emailExists = await context.Users.AnyAsync(x => x.Email == email);
            if (emailExists)
            {
                ShowError($"Elektron pochta manzili allaqachon mavjud!");
                return;
            }
        }

        var role = AnsiConsole.Prompt(
            new SelectionPrompt<UserRole>()
                .Title("Foydalanuvchi uchun [green]rol[/] tanlang:")
                .AddChoices(UserRole.User, UserRole.Admin));

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Parol: ")
                .Secret());

        var admin = new User
        {
            Username = username,
            Email = email,
            Role = role,
            PasswordHash = BC.HashPassword(password),
            CreatedAt = DateTime.UtcNow,
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
        
        ShowSuccess($"Admin '{username}' muvaffaqiyatli yaratildi!");
    }

    private static async Task DeleteUserAsync(IAppDbContext context)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]Foydalanuvchi o'chirish[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var input = AnsiConsole.Ask<string>("Foydalanuvchi nomi yoki elektron pochtasini kiriting: ");
        
        var user = await context.Users.FirstOrDefaultAsync(x => x.Username == input || x.Email == input);
        if (user is null)
        {
            ShowError("Foydalanuvchi topilmadi.");
            return;
        }

        var confirm = AnsiConsole.Confirm($"Haqiqatdan ham '[red]{user.Username}[/]'ni o'chirmoqchimisiz?");
        if (!confirm)
        {
            ShowSuccess("O'chirish bekor qilindi.");
            return;
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        ShowSuccess($"'{user.Username}' muvaffaqiyatli o'chirildi!");
    }
    
    private static void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"\n[bold green]✔ Muvaffaqiyatli:[/] {message}");
        AnsiConsole.MarkupLine("\n[grey]Menyuga qaytish uchun Enter bosing...[/]");
        Console.ReadLine();
    }

    private static void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"\n[bold red]✖ Xatolik:[/] {message}");
        AnsiConsole.MarkupLine("\n[grey]Menyuga qaytish uchun Enter bosing...[/]");
        Console.ReadLine();
    }
}