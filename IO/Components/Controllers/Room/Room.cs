using IO.Components.Converter;
using IO.Components.Logger;
using IO.Components.Server;
using IO.Models.Queries;
using IO.Models.Room;
using IO.MovementsData;
using IO.Utils.Attributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO.Components.Controllers.Room
{
    public class Room : Controller
    {
        private bool isReadyToAttack = true;
        private string flagForRoom { get; set; }
        private InfinityRoom infinityRoom = new InfinityRoom();
        private int Online { get; set; }
        private ConcurrentDictionary<string, Player> Players = new ConcurrentDictionary<string, Player>();
        private List<Player> roomPlayers = new List<Player>();
        public Room(IServer server, IConverter converter, ILogger logger) : base(server, converter, logger)
        {

        }

        [Method("ping")]
        public Response Ping(string request, IPEndPoint e)
        {
            return new Response("pong");
        }

        [Method("info")]
        public InfoResponse Info(string request, IPEndPoint e)
        {
            InfoResponse response = new InfoResponse();
            response.online = Online;
            response.server_location = "Рублёвка";

            return response;
        }

        [Method("login")]
        public LoginResponse Login(string request, IPEndPoint e)
        {
            LoginRequest loginRequest = (LoginRequest)
               Converter.GetObject(typeof(LoginRequest), Encoding.UTF8.GetBytes(request));

            LoginResponse response = new LoginResponse();
            if (loginRequest.token == null || loginRequest.token.Length < 5)
            {
                response.status = "fail";
                return response;
            }
            Player player = new Player();
            player.Token = loginRequest.token;
            Players[loginRequest.token] = player;

            Logger.Log("login", $"{loginRequest.token}", ILogger.MessageLevel.Common);
            Online++;

            response.status = "ok";
            return response;

        }
        [Method("logout")]
        public Response Logout(string request, IPEndPoint e)
        {
            LogoutRequest logoutRequest = (LogoutRequest)
               Converter.GetObject(typeof(LogoutRequest), Encoding.UTF8.GetBytes(request));

            Players.TryRemove(logoutRequest.token, out Player player);

            if (player == null)
            {
                return new Response("logout");
            }

            //player.SaveToDB();

            Online--;

            return new Response("logout");
        }
        [Method("searchRoom")]
        public RoomResponse SearchFreeRoomForMode(string request, IPEndPoint e)
        {
            int Place = 5;

            RoomRequest roomRequest = (RoomRequest)
                Converter.GetObject(typeof(RoomRequest), Encoding.UTF8.GetBytes(request));
            LoginRequest loginRequestForEnter = (LoginRequest)
                Converter.GetObject(typeof(LoginRequest), Encoding.UTF8.GetBytes(request));
            Logger.Log("mode", $"{request} {infinityRoom.infinityRoomsDictionary.FirstOrDefault(room => room.Value.Count < Place).Key}", ILogger.MessageLevel.Common);

            var player = Players[loginRequestForEnter.token];
            switch (roomRequest.mode)
            {
                case "infinity":
                    {
                        if (infinityRoom.listPlayer.All(list => list.Token != loginRequestForEnter.token) == true)
                        {
                            if (infinityRoom.infinityRoomsDictionary.Count < 1 || infinityRoom.infinityRoomsDictionary.Values.All(room => room.Count == Place)) //нет комнат или все заняты 
                            {
                                var keyRoom = new KeyGenerator().Generate();
                                roomPlayers.Clear();
                                flagForRoom = keyRoom;

                                if (infinityRoom.infinityRoomsDictionary.Count < 1)
                                    infinityRoom.CreateFirstRoom(infinityRoom.infinityRoomsDictionary, roomPlayers, keyRoom).ToList();

                                else if (infinityRoom.infinityRoomsDictionary.Values.All(room => room.Count == Place))
                                    infinityRoom.CreateNextRoom(infinityRoom.infinityRoomsDictionary, roomPlayers, keyRoom).ToList();
                            }

                            else if (infinityRoom.infinityRoomsDictionary.Values.Any(room => room.Count < Place) == true)
                            {
                                var keyRoom = infinityRoom.infinityRoomsDictionary.FirstOrDefault(room => room.Value.Count < Place).Key;
                                if (flagForRoom != keyRoom)
                                {
                                    roomPlayers.Clear();
                                    flagForRoom = keyRoom;
                                }
                            }
                            player = new SetPosNewPlayer().RandomPosition(player);
                            roomPlayers.Add(player);
                            player.mode = roomRequest.mode;
                            player.keyRoom = flagForRoom;
                            Players[player.Token] = player;
                            infinityRoom.infinityRoomsDictionary[player.keyRoom] = roomPlayers;
                        }
                        break;
                    }
                default:
                    break;
            }
            //Logger.Log("Room", $"{player.keyRoom} {infinityRoom.infinityRoomsDictionary[player.keyRoom]}", ILogger.MessageLevel.Common);

            RoomResponse response = new RoomResponse();
            response.players = infinityRoom.infinityRoomsDictionary[player.keyRoom].ToList();
            response.keyRoom = player.keyRoom;
            response.mode = player.mode;
            return response;
        }
        [Method("gameloop")]
        public MovementResponse Movement(string request, IPEndPoint e)
        {
            MovementRequest movement = (MovementRequest)
                Converter.GetObject(typeof(MovementRequest), Encoding.UTF8.GetBytes(request));

            var player = Players[movement.Token];
            new UpdatePosition(player, movement);
            MovementResponse response = new MovementResponse();

            switch (player.mode)
            {
                case "infinity":
                    var index = infinityRoom.infinityRoomsDictionary[player.keyRoom].FindIndex(t => t.Token == movement.Token); // находим индекс игрока по token и обновляем данные о нем в листе
                    infinityRoom.infinityRoomsDictionary[player.keyRoom][index] = player;
                    if (infinityRoom.bullets.Count > 0)
                    {
                        foreach(var bullet in infinityRoom.bullets)
                        {
                            bullet.ChangePos();
                        }
                    }
                    break;
                case "2":
                    break;
                default:
                    break;
            }
            //for (var i = infinityRoom.infinityRoomsDictionary[player.keyRoom].Count - 1; i >= 0; i--)
            //{
            //    Logger.Log("movement", $"token is {infinityRoom.infinityRoomsDictionary[player.keyRoom][i].Token}, x is {infinityRoom.infinityRoomsDictionary[player.keyRoom][i].x} y is {infinityRoom.infinityRoomsDictionary[player.keyRoom][i].y}", ILogger.MessageLevel.Common);
            //}
            Players[movement.Token] = player;
            response.players = infinityRoom.infinityRoomsDictionary[player.keyRoom].ToList();
            return response;
        }
        [Method("eventOnAttacked")]
        public AttackResponse Attack(string request, IPEndPoint e)
        {
            AttackRequest attackRequest = (AttackRequest)
                    Converter.GetObject(typeof(AttackRequest), Encoding.UTF8.GetBytes(request));
            var player = Players[attackRequest.token];
            if (isReadyToAttack)
            {
                Bullet bullet = new Bullet();
                bullet.SetSettings(player);
                switch (player.mode)
                {
                    case "infinity":
                        {
                            infinityRoom.bullets.Add(bullet);
                            break;
                        }
                    default:
                        break;
                }   
                isReadyToAttack = false;
                var timer = new Timer(Cooldown, null, (int)(attackRequest.attack_speed * 1000), -1);
                return new AttackResponse();
            }
            else
            {
                return null;
            }
        }
        private void Cooldown(object o)
        {
            isReadyToAttack = true;
        }
    }
}
