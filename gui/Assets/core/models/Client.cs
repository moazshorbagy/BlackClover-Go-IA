using System;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using UnityEngine;

namespace BlackClover
{

    public class Client
    {
        private WebSocket ws;
        //string tosend;
        //private string serverIp = "ws://echo.websocket.org";
        private string serverIp = "ws://192.168.88.221:8080";
        private string myname = "\"Hamada\"";
        string opmovrow;
        string opmovcol;
        int mymovrow;
        int mymovcol;
        string turnnow;
        string mycolor;
        string winner;
        string endreason;
        string invalidreason;
        string myscore;
        string myremainingtime;
        string opscore;
        string opremainingtime;
        int myprisoners;
        int opprisoners;
        string opcolor;
        bool myturn;

        JObject opactionobject;
        //char[,] board = { { ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."},{ ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "."} };
        //char[,] board;
        char[,] board = { { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' } };
       
        State gamestate = new State(0, new char[,] { { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' } }); 
        
        List<Action> OpAction; //to be replaced with shared action list
        List<string> myColor;
        List<Action> myAction;
        List<bool> turn;

        ClientState state = ClientState.INIT;

      public  Client(List<string> myColor, List<Action> OpAction, List<Action> myAction, List<bool> turn)
        {
            this.myColor = myColor;
            this.myAction = myAction;
            this.OpAction = OpAction;
            this.turn = turn;
        }

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


        (int, int) GetMyAction()
        {
            lock (this.turn)
            {
                this.turn.Add(true);
            }

            Action action;
            while (this.myAction.Count == 0)
            {
             
            }
            action = this.myAction[0];
            Debug.Log("server got: " + action.GetX() + " " + action.GetClr());
            lock (this.myAction)
            {
                this.myAction.RemoveAt(0);

            }
            return (action.GetX(), action.GetY());
        }

        void SetOpAction(int X, int Y, string c)
        {

            Action action = new Action(X, Y, char.Parse(c));
            Debug.Log("the action of opponent is received");
            lock (this.OpAction)
            {
                this.OpAction.Add(action);
                Debug.Log("The action X: " + action.GetX());
            }

            
        }


        private void OnMessageHandlerAsync(object sender, MessageEventArgs e)
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


                    if (string.Compare(mycolor, "B") == 0)
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
                    //Thread.Sleep(20000);


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

                        JObject playersobj = JObject.Parse(initstatobj["players"].ToString());
                        JObject player1obj = JObject.Parse(playersobj["B"].ToString());
                        JObject player2obj = JObject.Parse(playersobj["W"].ToString());

                        if (string.Compare(mycolor, "B") == 0)
                        {
                            myremainingtime = player1obj["remainingTime"].ToString();
                            myprisoners = Int32.Parse(player1obj["prisoners"].ToString());
                            opremainingtime = player2obj["remainingTime"].ToString();
                            opprisoners = Int32.Parse(player2obj["prisoners"].ToString());

                        }

                        else
                        {
                            myremainingtime = player2obj["remainingTime"].ToString();
                            myprisoners = Int32.Parse(player2obj["prisoners"].ToString());
                            opremainingtime = player1obj["remainingTime"].ToString();
                            opprisoners = Int32.Parse(player1obj["prisoners"].ToString());
                        }

                        lock (this.myColor)
                        {
                            this.myColor.Add(mycolor); 
                        }

                        if (movloglength % 2 == 0)//even 
                        {
                            if (string.Compare(turnnow, mycolor) == 0)
                            {
                                myturn = true;
                                state = ClientState.THINKING;

                                

                                ( mymovrow,mymovcol) = GetMyAction();

                                if (mymovcol == -1 && mymovrow == -1)
                                {
                                    ws.SendAsync(sendmovpass, OnSendComplete);
                                }
                                else
                                { ws.SendAsync(sendmovplace, OnSendComplete); }

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
                                (mymovrow, mymovcol) = GetMyAction();

                                if (mymovcol == -1 && mymovrow == -1)
                                {
                                    ws.SendAsync(sendmovpass, OnSendComplete);
                                }
                                else
                                { ws.SendAsync(sendmovplace, OnSendComplete); }



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
                    //myturn = true;
                    JObject movobj = JObject.Parse(o["move"].ToString());

                    if (string.Compare(mycolor, "B") == 0)
                        opcolor = "W";
                    else
                        opcolor = "B";

                    if (string.Compare(movobj["type"].ToString(), "pass") == 0)
                    {
                        Console.WriteLine("el move no3ha pass");
                        /* 
                           ******************
                           * NOTIFY THE AGENT THAT THE OPPNENT MADE A PASS MOVE
                           * ****************
                        */
                        //

                        SetOpAction(-1, -1, opcolor);
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

                        /* 
                           ******************
                           * NOTIFY THE AGENT THAT THE OPPNENT MADE A PLACE MOVE
                           * ****************
                        */
                            Console.WriteLine("el move no3ha place");
                            JObject pointobj = JObject.Parse(movobj["point"].ToString());
                            Console.WriteLine(pointobj);

                            opmovrow = pointobj["row"].ToString();
                            opmovcol = pointobj["column"].ToString();
                        Debug.Log(opcolor);

                            SetOpAction(Int32.Parse(opmovrow), Int32.Parse(opmovcol), opcolor);

                    }

                    (mymovrow, mymovcol) = GetMyAction();

                    if (mymovcol == -1 && mymovrow == -1)
                    {
                        ws.SendAsync(sendmovpass, OnSendComplete);
                    }
                    else
                    { ws.SendAsync(sendmovplace, OnSendComplete); }


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

            
            ws.EmitOnPing = true;
            ws.OnOpen += OnOpenHandler;
            ws.OnMessage += OnMessageHandlerAsync;
            ws.OnError += OnErrorHandler;
            ws.OnClose += OnCloseHandler;


            ws.ConnectAsync();

        }
    }
}
