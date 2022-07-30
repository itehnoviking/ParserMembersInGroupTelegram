using TL;


const string PATH = "idAndUsername.txt";

using var client = new WTelegram.Client();
var myClient = await client.LoginUserIfNeeded();
Console.WriteLine($"We are logged-in as {myClient.username ?? myClient.first_name + " " + myClient.last_name} (id {myClient.id})");

var chats = await client.Messages_GetAllChats();
foreach (var (id, chat) in chats.chats)
{
    switch (chat)
    {
        case Chat basicChat when basicChat.IsActive:
            Console.WriteLine($"{id}:  Basic chat: {basicChat.title} with {basicChat.participants_count} members");
            break;
        case Channel group when group.IsGroup:
            Console.WriteLine($"{id}: Group {group.username}: {group.title}");
            break;
    }
}

Console.Write("Type a chat ID: ");
var groupId = long.Parse(Console.ReadLine());
var channel = (Channel)chats.chats[groupId];

var partcipants = await client.Channels_GetAllParticipants(channel);
foreach (var (id, user) in partcipants.users)
{
    if (user.username != null)
    {
        using (StreamWriter writer = new StreamWriter(PATH, true))
        {
            await writer.WriteLineAsync($"{user.id}, @{user.username}");
        }

    }
    
}  