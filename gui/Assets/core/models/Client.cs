using System;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlackClover
{
    public class Client
    {
        private WebSocket ws;
        //string tosend;
        //private string serverIp = "ws://echo.websocket.org";
        private string serverIp = "ws://localhost:8080";
        private string myname = "\"yasminaa\"";
        string opmovrow;
        string opmovcol;
        string mymovrow;
        string mymovcol;
        bool myturn;
        string turnnow;
        string mycolor;
        string winner;
        string endreason;
        string invalidreason;
        string myscore;
        string myremainingtime;
        string opscore;
        string opremainingtime;
        int prisoners;

        JObject opactionobject;
        char[,] board = { { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."} };
        int turn = 0; //set initially as black, changes from turnnow
        State gamestate = new State(turn, board);
        Action opaction;
        List<Action> sharedOpAction; //to be replaced with shared action list


        ClientState state = ClientState.INIT;
        enum ClientState
        {
            INIT,
            READY,
            IDLE,
            THINKING,
            AWAITING_MOVE_RESPONSE
        };


        public string getopmovrow()
        {
            return opmovrow;
        }
        public string getopmovcol()
        {
            return opmovcol;
        }
        private void OnCloseHandler(object sender, CloseEventArgs e)
        {
            Console.WriteLine("heyy WebSocket connection closed because" + e.Reason);
            state = ClientState.INIT;




        }
        private void OnSendComplete(bool success)
        {
            Console.WriteLine("Message sent successfully? " + success);



        }



        private void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.IsText)
            {
                JObject o = JObject.Parse(e.Data); //msg from the server

                Console.WriteLine("WebSocket server said: " + e.Data);


                /////to send still dont know when   
                string sendmovplace = "{\"type\":\"MOVE\",\"move\":{\"type\":\"place\",\"point\":{\"row\":" + mymovrow + ",\"column\":" + mymovcol + "}}}";
                string sendmovpass = "{\"type\":\"MOVE\",\"move\":{\"type\":\"pass\"}}";
                string sendmovresign = "{\"type\":\"MOVE\",\"move\":{\"type\":\"resign\"}}";
                //////


                if (string.Compare(o["type"].ToString(), "END") == 0)
                {
                    state = ClientState.READY;
                    Console.WriteLine("game ended");
                    Console.WriteLine("Reason " + o["reason"]);
                    winner = o["winner"].ToString();
                    endreason = o["reason"].ToString();
                    /* 
                     ******************
                     * NOTIFY THE AGENT THAT THE GAME HAS ENDED BUT STILL READY FOR A NEW START
                     * ****************
                     */


                    if (mycolor == "B")
                    {
                        JObject playersobj = JObject.Parse(o["players"].ToString());
                        JObject player1obj = JObject.Parse(playersobj["B"].ToString());
                        myremainingtime = player1obj["remainingTime"].ToString();
                        myscore = player1obj["score"].ToString();
                        JObject player2obj = JObject.Parse(playersobj["W"].ToString());
                        opremainingtime = player2obj["remainingTime"].ToString();
                        opscore = player2obj["score"].ToString();
                    }
                    else
                    {
                        JObject playersobj = JObject.Parse(o["players"].ToString());
                        JObject player1obj = JObject.Parse(playersobj["W"].ToString());
                        myremainingtime = player1obj["remainingTime"].ToString();
                        myscore = player1obj["score"].ToString();
                        JObject player2obj = JObject.Parse(playersobj["B"].ToString());
                        opremainingtime = player2obj["remainingTime"].ToString();
                        opscore = player2obj["score"].ToString();

                    }




                }


                if (string.Compare(o["type"].ToString(), "NAME") == 0)
                {



                    //{"type":"NAME","name":"Client-19824"}
                    // ws.SendAsync(mymsg.ToString(), OnSendComplete);
                    // ws.SendAsync("{\"type\":\"NAME\",\"name\":\"credit\"}", OnSendComplete); 
                    ws.SendAsync("{\"type\":\"NAME\",\"name\":" + myname + "}", OnSendComplete);
                    state = ClientState.READY;
                    Console.WriteLine("Client is ready to play");
                }
                else if (string.Compare(o["type"].ToString(), "START") == 0)
                {



                    if (state == ClientState.READY)
                    {


                        JObject configobj = JObject.Parse(o["configuration"].ToString());
                        JObject initstatobj = JObject.Parse(configobj["initialState"].ToString());
                        //JObject movelogobj = JObject.Parse(configobj["moveLog"].ToString());
                        JArray array = new JArray();
                        array = (JArray)configobj["moveLog"];
                        int movloglength = array.Count;

                        turnnow = initstatobj["turn"].ToString();
                        mycolor = o["color"].ToString();
                        if (movloglength % 2 == 0)//even 
                        {
                            if (string.Compare(turnnow, mycolor) == 0)
                            {
                                myturn = true;
                                state = ClientState.THINKING;

                                /* 
                                   ******************
                                   * NOTIFY THE AGENT THAT THE GAME HAS ENDED BUT STILL READY FOR A NEW START
                                   * ****************
                                */

                            }
                            else
                            {
                                myturn = false;
                                state = ClientState.IDLE;

                            }



                        }
                        else //odd
                        {
                            if (string.Compare(turnnow, mycolor) != 0)
                            {
                                myturn = true;
                                state = ClientState.THINKING;
                            }
                            else
                            {
                                myturn = false;
                                state = ClientState.IDLE;

                            }




                        }



                    }





                    // ws.SendAsync(mymsg.ToString(), OnSendComplete);
                }
                else if (string.Compare(o["type"].ToString(), "MOVE") == 0)
                {
                    state = ClientState.THINKING;
                    JObject movobj = JObject.Parse(o["move"].ToString());
                    if (string.Compare(movobj["type"].ToString(), "pass") == 0)
                    {
                        Console.WriteLine("el move no3ha pass");
                        /* 
                           ******************
                           * NOTIFY THE AGENT THAT THE OPPNENT MADE A PASS MOVE
                           * ****************
                        */

                        if (mycolor == 'B')
                            opcolor = 'W';
                        else
                            opcolor = 'B';
                        opaction = new Action(-1, -1, opcolor);
                        sharedOpAction.Add(opaction);
                        string recievemovpass =  "{\"type\":\"MOVE\",\"move\": {\"type\":\"pass\"} }";
                        opactionobject = JObject.Parse(recievemovpass);
                    }

                    else if (string.Compare(movobj["type"].ToString(), "resign") == 0)
                    { Console.WriteLine("el move no3ha resign");
                        /* 
                           ******************
                           * NOTIFY THE AGENT THAT THE OPPNENT MADE A RESIGN MOVE
                           * ****************
                        */
                    }
                    else if (string.Compare(movobj["type"].ToString(), "place") == 0)
                    {

                        if (string.Compare(movobj["type"].ToString(), "place") == 0)
                        {   /* 
                           ******************
                           * NOTIFY THE AGENT THAT THE OPPNENT MADE A PLACE MOVE
                           * ****************
                        */
                            Console.WriteLine("el move no3ha place");
                            JObject pointobj = JObject.Parse(movobj["point"].ToString());
                            Console.WriteLine(pointobj);

                            opmovrow = pointobj["row"].ToString();
                            opmovcol = pointobj["column"].ToString();

                            if (mycolor == 'B')
                                opcolor = 'W';
                            else
                                opcolor = 'B';
                            opaction =new Action(opmovrow, opmovcol, opcolor);
                            sharedOpAction.Add(opaction);
                            string recievemovplace = "{\"type\":\"MOVE\",\"move\": {\"type\":\"place\",\"point\":{\"row\":" + opmovrow + ",\"column\":"+ opmovcol +"}}} }";
                            opactionobject = JObject.Parse(recievemovplace);
                        }



                    }

                    






                }
            }
            else if (e.IsPing)
            {
                Console.WriteLine("a ping has been received.");
                //  ws.SendAsync(e,OnSendComplete);
            }
            else if (e.IsBinary)
            {
                Console.WriteLine("binary msg recieved");
            }
            else
            {
                Console.WriteLine("trash recieved");
            }


        }



        private void OnOpenHandler(object sender, System.EventArgs e)
        {
            Console.WriteLine("websocket connected    ");
            //send(tosend);
        }



        private void OnErrorHandler(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("error occured is " + e.Message);
        }
        public void Start()
        {

            // tosend = "hello im the client";
            Console.WriteLine("staaaaart");
            ws = new WebSocket(serverIp);
            state = ClientState.INIT;

            if (turnnow == 'B')
            {
                turn = 0;
            }
            else
            {
                turn = 1;
            }

            ws.EmitOnPing = true;
            ws.OnOpen += OnOpenHandler;
            ws.OnMessage += OnMessageHandler;
            ws.OnError += OnErrorHandler;
            ws.OnClose += OnCloseHandler;


            ws.ConnectAsync();

        }
    }
}
