using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace NpcScpSl.Commands.RemoteAdmin.NpcCommands
{
    internal class ExecuteCommand : ICommand
    {
        public string Command => "execute";

        public string[] Aliases { get; } = { "exec" };

        public string Description => "Выполняет любую команду от имени NPC по ID.";

        public bool SanitizeResponse => false;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"{NpcCommand.Prefix}.{Command}"))
            {
                response = NpcCommand.NotPermissionMessage;
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Использование: npc execute [NpcID] <команда>";
                return false;
            }

            // Получаем ID NPC и проверяем, является ли он NPC
            if (!int.TryParse(arguments.At(0), out int npcId))
            {
                response = "Неверный ID NPC!";
                return false;
            }

            var npc = Player.Get(npcId);
            if (npc == null || !npc.IsNPC)
            {
                response = "Этот ID не принадлежит NPC!";
                return false;
            }

            // Получаем полную команду, которую нужно выполнить от имени NPC
            string command = string.Join(" ", arguments.Array[arguments.Offset + 1..]);

            // Выполняем команду от имени NPC
            CommandProcessor.ProcessCommand(npc.ReferenceHub, command);
            response = $"Команда '{command}' выполнена от имени NPC с ID {npcId}.";

            return true;
        }
    }
}
