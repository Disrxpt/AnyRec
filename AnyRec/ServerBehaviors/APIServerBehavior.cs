using AnyRec.Backend;
using AnyRec.Models;
using AnyRec.Server;
using AnyRec.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Text;

namespace AnyRec.ServerBehaviors
{
    internal class APIServerBehavior : ServerBehavior
    {
        public APIServerBehavior()
        {
            this.Port = 3000;
            this.Name = "API";
            this.Listener = APIServerBehavior.Listen;
        }
        public static byte[] Listen(HttpListenerContext ctx, string postdata)
        {
            string api = ctx.Request.Url.AbsolutePath;
            byte[] response = Bytes.StringToByte("[]");
            if (api == "/api/players/v1/" || api == "/api/players/v1/getorcreate")
                response = Bytes.JsonObjectToByte(ProfileHandler.Get2017Profile());
            else if (api == "/api/config/v2")
                response = Bytes.JsonObjectToByte(Configs.GetConfigSettings());
            else if (api == "/api/config/v1/objectives")
                response = Bytes.JsonObjectToByte(Objectives.LoadObjectives());
            else if (api == "/api/config/v1/motd")
                response = Bytes.StringToByte("Welcome to AnyRec by ImagineHacking");
            else if (api == "/api/avatar/v2" || api == $"/api/avatar/v1/{Config.GetInt("PlayerId")}")
                response = Bytes.JsonObjectToByte(Avatar.GetAvatar());
            else if (api == "/api/avatar/v2/set" || api == "/api/avatar/v1/set")
            {
                AvatarModel newavatar = JsonConvert.DeserializeObject<AvatarModel>(postdata);
                Avatar.SetAvatar(newavatar);
                response = Bytes.JsonObjectToByte(new GenericMessage { Success = true, Message = "Updated avatar." });
            }
            else
            {
                Logging.Warning("API", $"{api} is unsupported, returning blank response.");
            }
            return response;
        }
    }
}
