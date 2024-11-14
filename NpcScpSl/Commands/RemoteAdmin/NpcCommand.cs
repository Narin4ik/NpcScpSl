using CommandSystem;
using Exiled.Permissions.Extensions;
using NpcScpSl.Commands.RemoteAdmin.NpcCommands;
using System;

namespace NpcScpSl.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class NpcCommand : ParentCommand
    {
        public static string Prefix = "npc";
        public static string NotPermissionMessage = "У вас недостаточно прав";

        public NpcCommand() => LoadGeneratedCommands();

        public override string Command => "npc";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Управление Npc";

        public bool SanitizeResponse => false;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new CreateCommand());
            RegisterCommand(new RemoveCommand());
            RegisterCommand(new ExecuteCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response) 
        {
            response = "\n Доступные команды:";

            foreach (ICommand command in AllCommands)
            {
                if (sender.CheckPermission($"{Prefix}.{command.Command}"))
                {
                    response += $"\n\n- {command.Command} ({string.Join(", ", command.Aliases)})\n{command.Description}";
                }
            }

            return false;
        }
    }
}
