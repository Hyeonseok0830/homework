using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace Castle_Defense_0
{

    public partial class Form1 : Form
    {


        System.Threading.Timer timer_main;
        System.Threading.Timer timer_unit_move;
        System.Threading.Timer timer_skill;
        System.Threading.Timer timer_attack1;
        System.Threading.Timer timer_skill2;

        basecamp mycamp;
        basecamp enemycamp;
        Bitmap unitimg = Properties.Resources.unit;
        Bitmap e_unitimg = Properties.Resources.e_unit;
        Bitmap weapon0 = Properties.Resources.weapon0;
        Bitmap weapon1 = Properties.Resources.weapon1;
        Bitmap weapon2 = Properties.Resources.weapon2;
        Bitmap weapon3 = Properties.Resources.weapon3;

        Bitmap skill = Properties.Resources.skill;
        Bitmap e_weapon0 = Properties.Resources.e_weapon0;
        Bitmap e_weapon1 = Properties.Resources.e_weapon1;
        Bitmap e_weapon2 = Properties.Resources.e_weapon2;
        Bitmap e_weapon3 = Properties.Resources.e_weapon3;

        Bitmap stone = Properties.Resources.stone;

        Bitmap backimg = Properties.Resources.backimg;
        Bitmap mybase = Properties.Resources.mybase;
        Bitmap enemybase = Properties.Resources.enemybase;
        Bitmap scrollright = Properties.Resources.right;
        Bitmap scrollleft = Properties.Resources.left;
        Bitmap healthbar = Properties.Resources.healthbar;
        int Scrollx;
        int max;
        bool skill1 = false;
        bool skill2 = false;
        int skill_x;
        int skill_x2;
        bool first = true;
        int skill_count = 0;
        int skill_count2 = 0;
        int backx;
        int stonex;
        int e_stonex;


        class basecamp
        {
            public int x;
            public int y;
            public int gold; //골드량
            public int base_health; //기지체력            
            public int gold_intervel; //골드 생성속도
            public int armor;
            public int health;
            Bitmap image;
            public basecamp(int x, int y, int g, int bh, int gi, int a, int h, Bitmap img)
            {
                this.x = x;
                this.y = y;
                this.gold = g;
                this.base_health = bh;
                this.gold_intervel = gi;
                this.armor = a;
                this.health = h; //감소할체력
                this.image = img;

            }
            void BaseDestroy()
            {
                //모든타이머 스탑, 승리 화면 띄우기or 메뉴 띄우기    
            }

            public int damage(int damage)
            {
                return this.base_health -= damage;
            }
            public void upgrade()
            {
                this.base_health += 200;
                this.gold_intervel -= 100;
                //this.image = 다른이미지로 바꾸기
            }
            public int getx()
            {
                return this.x;
            }
            public int gety()
            {
                return this.y;
            }
            public void goldup(int g)
            {
                this.gold += g;
            }

        }

        class unit
        {
            public int x;
            public int y = 400;
            public int team; //팀 종류
            public int health; //체력
            public int power; //공격력
            public int armor; //방어력
            public int range; //사거리
            public int gold; //생성 골드량
            public int atttack_speed;
            public int move_speed;
            public int u_health;//감소할 체력
            public int size_x;
            public int size_y;
            public bool moving;
            Bitmap image;
            public unit()
            {
            }
            public unit(int u_x, int t, int h, int p, int a, int r, int g, int a_s, int m_s, int u_h, int s_x, int s_y, bool mv)
            {

                this.x = u_x;
                this.team = t; //팀 종류
                this.health = h; //체력
                this.power = p; //공격력
                this.armor = a; //방어력
                this.range = r; //사거리
                this.gold = g; //생성 골드량
                this.atttack_speed = a_s;
                this.move_speed = m_s;
                this.u_health = u_h;
                this.size_x = s_x;
                this.size_y = s_y;
                this.moving = mv;

            }
            public int gethealth()
            {
                return this.health;
            }
            public int getteam()
            {
                return this.team;
            }
            public int getx()
            {
                return this.x;
            }
            public int gety()
            {
                return this.y;
            }
            public int getr()
            {
                return this.range;
            }
            public int getsize_x()
            {
                return size_x;
            }
            public int getsize_y()
            {
                return size_y;
            }
            ~unit()
            {
            }
            public float attack(unit underattack)
            {
                return underattack.u_health -= this.power * 100 / (100 + underattack.armor);
            }
            public float attack(basecamp bs)
            {
                return bs.health -= this.power * 100 / (100 + bs.armor);
            }
            public void range_attack(unit underattack)
            {
                if (Math.Abs(this.x - underattack.x) < this.range)
                    attack(underattack);                // 범위 내 공격
                if (Math.Abs(this.x - underattack.x) < underattack.range)
                    underattack.attack(this);
            }
            public int die()//상대가 가질 골드량 리턴
            {
                return this.gold / 2;
            }
            public void move(bool moving)
            {
                if (moving)
                {
                    if (this.team == 1)
                    {
                        this.x += this.move_speed;
                    }
                    else
                    {
                        this.x -= this.move_speed;
                    }
                }
                else
                {
                    this.move_speed = 0;
                }
            }


        }
        class tower
        {
            public int power; //공격력
            public int team;
            public int x;
            public int y;
            public int gold; //생성 골드량
            public int atttack_speed;
            public int arrowx;
            public int arrowy;
            public int width;
            public int height;
            public int range;
            public bool bullet;
            public tower(int team, int px)
            {
                this.team = team;
                if (team == 1)
                    this.x = px;
                else
                    this.x = px;
                this.y = 200;
                this.gold = 300;
                this.atttack_speed = 30;
                this.width = 30;
                this.height = 30;
                this.power = 10;
                if (team == 1)
                    this.arrowx = this.x + 40;
                else
                    this.arrowx = this.x - 40;
                this.arrowy = this.y;
                this.range = 400;
                this.bullet = false;
            }
        }
        class bullet
        {
            public int team;
            public int x;
            public int y;
            public double angle;
            public int size;
            public int power;
            public bullet(int team, int x, int y, double angle, int p)
            {
                this.team = team;
                this.x = x;
                this.y = y;
                this.angle = angle;
                this.size = 10;
                this.power = p;
            }
            public void move()
            {
                this.x += (int)((float)Math.Cos(this.angle) * 20);
                this.y += (int)((float)Math.Sin(this.angle) * 20);
            }
        }
        List<tower> tower_list = new List<tower>();
        List<unit> unit_list = new List<unit>();
        List<bullet> bullet_list = new List<bullet>();
        //public static void attack_timer_start()
        //{
        //    System.Threading.TimerCallback callback3 = Timer_attack1_Event;
        //    timer_attack1 = new System.Threading.Timer(callback3, null, 0, 5);
        //}
        
        public Form1()
        {
            //  startfrm = new StartForm(this);
            max = 0;
            backx = 0;
            stonex = 0;
            InitializeComponent();



            Rectangle rect = this.ClientRectangle;

            this.SetStyle(ControlStyles.DoubleBuffer, true); //더블 버퍼링
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);


            mycamp = new basecamp(0, 50, 1000, 1000, 1000, 10, 200, mybase);// x, y, 총골드량, 체력, 골드생성속도 ,방어력,감소할 체력, 이미지
            enemycamp = new basecamp(1300, 50, 1000, 1000, 1000, 10, 200, enemybase);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // startfrm.ShowDialog();
            new Thread(new ThreadStart(Client_send)).Start();
            new Thread(new ThreadStart(Client_recive)).Start();
            
            start_timer();
            
        }
        string msg=" ";
        public void Client_send()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("155.230.248.153"), 7777);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipep);
           // MessageBox.Show("통신연결완료");
            

            Send(client, "Client send connection");
            //int i = 0;
            while (true)
            {
                //i++;
                //byte[] _data = new byte[1024];
                //_data = Encoding.Default.GetBytes( " 번째 데이터 전송");
                Send(client, msg);

                if (msg == "unit1" || msg == "unit2" || msg == "tower" || msg[0]=='s')
                {
                    // Thread.Sleep(20);
                    msg = " ";
                    //Thread.Sleep(20);
                }

                //Recieve(client, ref _data);
                //Console.WriteLine("Client Message : {0}", _data);
                Thread.Sleep(5);
            }
            client.Close();
        }
        public void Client_recive()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("155.230.248.153"), 9999);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(ipep);
            MessageBox.Show("서버와 연결되었습니다.");
            //UdpClient cli = new UdpClient();
            //Byte[] _data = new Byte[1024];
            // client.Receive(_data);
            //String _buf = Encoding.Default.GetString(_data);
            // Console.WriteLine(_buf);
            //_buf = "소켓 접속 확인 됐습니다.";
            // _data = Encoding.Default.GetBytes(_buf);
            //client.Send(_data);
            // client.Close();
            String str = " ";
            //Send(client, "Client recive connection");
            while (true)
            {
                
                Recieve(client, ref str);
                //Console.WriteLine("Client Message : {0}", _data);
                //MessageBox.Show(str);
                if (str == "unit1")
                {
                    recv_Enemy1_Create();
                }
                else if (str == "unit2")
                    recv_Enemy2_Create();
                else if (str == "tower")
                    recv_enemy_tower_Create();
                else if (str[0] == 's')
                {
                    // MessageBox.Show(str);
                    string i;
                    i = str.Substring(1);
                    // MessageBox.Show(i);
                    int x = Convert.ToInt32(i);
                    skill_x2 = 1000 - x;
                    recv_skill_create();
                }
                // Thread.Sleep(3);
            }
            client.Close();
        }

        public bool Send(Socket sock, String msg)
        {
            byte[] data = Encoding.Default.GetBytes(msg);
            int size = data.Length;
            byte[] data_size = new byte[4];
            data_size = BitConverter.GetBytes(size);
            sock.Send(data_size);
            sock.Send(data, 0, size, SocketFlags.None);
            return true;
        }

        public bool Recieve(Socket sock, ref String msg)
        {
            byte[] data_size = new byte[4];
            sock.Receive(data_size, 0, 4, SocketFlags.None);
            int size = BitConverter.ToInt32(data_size, 0);
            byte[] data = new byte[size];
            sock.Receive(data, 0, size, SocketFlags.None);
            msg = Encoding.Default.GetString(data);
            return true;
        }
        public void start_timer()
        {
            System.Threading.TimerCallback callback2 = Timer_main_Event;
            timer_main = new System.Threading.Timer(callback2, null, 0, mycamp.gold_intervel);

            System.Threading.TimerCallback callback1 = timer_unit_move_Event;
            timer_unit_move = new System.Threading.Timer(callback1, null, 0, 10);
        }
        void end_game(int victory)
        {
            if (victory == 1)
            {
                timer_main.Dispose();
                timer_unit_move.Dispose();
                MessageBox.Show("1팀 승리!");// 메세지박스 뜨는데 오래걸리고 타이머.dispose() 두개 반복실행함
                this.Close();


            }
            else
            {
                timer_main.Dispose();
                timer_unit_move.Dispose();
                MessageBox.Show("2팀 승리!");
                this.Close();


            }
        }
        public bool Crush(int x1, int x2, int dis)
        {

            if (Math.Abs(x1 - x2) < dis)
                return true;

            else
                return false;
        }
        bool CircleCrush(float x1, float y1, float x2, float y2, int radius1, int radius2)
        {
            if (Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2))) <= radius1 + radius2)
            {
                return true;
            }
            else
                return false;
        }
        public double getAngle(double x1, double y1, double x2, double y2)
        {
            double dy = y2 - y1;
            double dx = x2 - x1;
            double angle = Math.Atan(dy / dx) * (180.0 / Math.PI);

            if (dx < 0.0)
            {
                angle += 180.0;
            }
            else
            {
                if (dy < 0.0) angle += 360.0;
            }

            return angle;
        }
        private void On_Paint(object sender, PaintEventArgs e)
        {
            //화면 스크롤
            Rectangle r = this.ClientRectangle;
            if (Scrollx > r.Width - 50 && max < 550)
            {
                mycamp.x -= 5;
                enemycamp.x -= 5;
                backx -= 5;
                max += 5;
                skill_x -= 5;
                skill_x2 -= 5;
                for (int i = 0; i < tower_list.Count; i++)
                {
                    tower_list[i].x -= 5;
                    tower_list[i].arrowx -= 5;
                }
                for (int i = 0; i < bullet_list.Count; i++)
                {
                    bullet_list[i].x -= 5;
                }
                //모든 유닛 위치이동
                for (int i = 0; i < unit_list.Count; i++)
                {
                    unit_list[i].x -= 5;
                }

            }
            if (Scrollx < 50 && max > 0)
            {
                mycamp.x += 5;
                enemycamp.x += 5;
                backx += 5;
                max -= 5;
                skill_x += 5;
                skill_x2 += 5;
                for (int i = 0; i < tower_list.Count; i++)
                {
                    tower_list[i].x += 5;
                    tower_list[i].arrowx += 5;
                }
                //모든 유닛 위치 이동
                for (int i = 0; i < unit_list.Count; i++)
                {
                    unit_list[i].x += 5;
                }
                for (int i = 0; i < bullet_list.Count; i++)
                {
                    bullet_list[i].x += 5;
                }

            }
            //

            //바닥그리기
            e.Graphics.DrawImage(backimg, backx, -100, 1550, 798);

            //체력바 및 basecamp그리기
            SolidBrush red_brush = new SolidBrush(Color.Red);
            SolidBrush blue = new SolidBrush(Color.Blue);
            SolidBrush black_brush = new SolidBrush(Color.Black);
            Pen black = new Pen(Color.Black, 3);
            Color[] c = { Color.Yellow, Color.Blue, Color.Red, Color.Black };

            e.Graphics.DrawImage(mybase, mycamp.x, mycamp.y + 150, 200, 200);
            e.Graphics.DrawImage(enemybase, enemycamp.x - 100, enemycamp.y + 150, 200, 200);

            e.Graphics.DrawImage(healthbar, mycamp.x, 20, 300, 120);
            e.Graphics.DrawImage(healthbar, enemycamp.x - 200, 20, 300, 120);

            e.Graphics.FillRectangle(red_brush, mycamp.x + 75, 79, mycamp.base_health * mycamp.health / mycamp.base_health, 13);//체력바

            e.Graphics.FillRectangle(red_brush, enemycamp.x - 125, 79, enemycamp.base_health * enemycamp.health / enemycamp.base_health, 13);//체력바        

            //타워 그리기
            for (int i = 0; i < tower_list.Count; i++)
            {
                //SolidBrush brush = new SolidBrush(c[3]);
                int width = tower_list[i].width;
                int height = tower_list[i].height;
                int x = tower_list[i].x;
                int y = tower_list[i].y;

                Pen pen = new Pen(Color.Black, 5);
                e.Graphics.DrawLine(pen, x, y, tower_list[i].arrowx, tower_list[i].arrowy);
                e.Graphics.FillEllipse(black_brush, new Rectangle(x - (width / 2), y - (height / 2), width, height));
            }

            //유닛 그리기
            for (int i = 0; i < unit_list.Count; i++)
            {
                SolidBrush my = new SolidBrush(c[1]);
                SolidBrush en = new SolidBrush(c[2]);

                e.Graphics.FillRectangle(red_brush, unit_list[i].x - 20, unit_list[i].gety() - 60, unit_list[i].gethealth() * unit_list[i].u_health / unit_list[i].gethealth(), 10);

                if (unit_list[i].getteam() == 1)
                {
                    e.Graphics.DrawImage(unitimg, unit_list[i].getx() - unit_list[i].getsize_x(), unit_list[i].gety() - unit_list[i].getsize_y(),
                        unit_list[i].getsize_x(), unit_list[i].getsize_y());

                    if (unit_list[i].range <= 50)
                    {
                        switch (a)
                        {
                            case 0:
                                e.Graphics.DrawImage(weapon0, unit_list[i].getx(), unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 1:
                                e.Graphics.DrawImage(weapon1, unit_list[i].getx(), unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 2:
                                e.Graphics.DrawImage(weapon2, unit_list[i].getx(), unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 3:
                                e.Graphics.DrawImage(weapon3, unit_list[i].getx(), unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                        }
                    }

                    else if (unit_list[i].range > 50)
                    {

                        e.Graphics.DrawImage(stone, unit_list[i].getx() + stonex, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());

                    }

                }
                else //적유닛
                {
                    e.Graphics.DrawImage(e_unitimg, unit_list[i].getx() - unit_list[i].getsize_x(), unit_list[i].gety() - unit_list[i].getsize_y(),
                       unit_list[i].getsize_x(), unit_list[i].getsize_y());
                    if (unit_list[i].range <= 50)
                    {
                        switch (a)
                        {
                            case 0:
                                e.Graphics.DrawImage(e_weapon0, unit_list[i].getx() - 40, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 1:
                                e.Graphics.DrawImage(e_weapon1, unit_list[i].getx() - 40, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 2:
                                e.Graphics.DrawImage(e_weapon2, unit_list[i].getx() - 40, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;
                            case 3:
                                e.Graphics.DrawImage(e_weapon3, unit_list[i].getx() - 40, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());
                                break;

                        }
                    }
                    else if (unit_list[i].range > 50)
                    {
                        e.Graphics.DrawImage(stone, unit_list[i].getx() - 50 + e_stonex, unit_list[i].gety() - unit_list[i].getsize_y(),
                           unit_list[i].getsize_x(), unit_list[i].getsize_y());

                    }
                }
            }

            for (int i = 0; i < bullet_list.Count; i++)
            {
                e.Graphics.FillEllipse(black_brush, new Rectangle(bullet_list[i].x - bullet_list[i].size / 2, bullet_list[i].y - bullet_list[i].size / 2, bullet_list[i].size, bullet_list[i].size));
            }
            label2.Text = "골드량 : " + mycamp.gold.ToString();
            label1.Text = "총 인원 " + unit_list.Count.ToString();
            if (skill1)//스킬범위 표시
            {
                Pen pen = new Pen(Color.Red, 5);
                e.Graphics.DrawImage(skill, new Rectangle(Scrollx - 300, 200, 600, 200));
            }

            e.Graphics.DrawImage(scrollleft, 0, r.Height / 2, 50, 50);
            e.Graphics.DrawImage(scrollright, r.Width - 50, r.Height / 2, 50, 50);
        }


        void Timer_main_Event(Object obj)
        {
            //타이머에서 수행할 작업을 아래에 작성
            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    mycamp.gold++;

                    for (int i = 0; i < unit_list.Count; i++)
                    {
                        for (int c = 0; c < tower_list.Count(); c++)// 타워의 사정거리 내에 유닛 파악
                        {
                            if (tower_list[c].team != unit_list[i].team && CircleCrush(tower_list[c].x, tower_list[c].y, unit_list[i].x, unit_list[i].y, tower_list[0].range, unit_list[i].size_x + 3))
                            {
                                double angle = getAngle(tower_list[c].x, tower_list[0].y, unit_list[i].x, unit_list[i].y);
                                tower_list[c].arrowx = tower_list[c].x + (int)((float)Math.Cos(angle * Math.PI / 180) * 40);
                                tower_list[c].arrowy = tower_list[c].y + (int)((float)Math.Sin(angle * Math.PI / 180) * 40);
                                bullet_list.Add(new bullet(tower_list[c].team, tower_list[c].x, tower_list[c].y, angle * Math.PI / 180, tower_list[c].power));
                                //tower_list[c].bullet = true;
                            }
                        }
                    }
                }
                ));
            Invalidate();
        }
        bool move_unit;
        int interval = 0;
        bool timer = false;
        void attackstart()
        {
            if (!timer)
            {
                System.Threading.TimerCallback callback = Timer_attack1_Event;
                timer_attack1 = new System.Threading.Timer(callback, null, 0, 100);
            }
        }
        void timer_unit_move_Event(Object obj)
        {
            //타이머에서 수행할 작업을 아래에 작성
            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    interval++;
                    if (interval == 50)
                        interval = 0;
                    for (int i = 0; i < unit_list.Count; i++)
                    {
                        move_unit = true;
                        unit_list[i].moving = true;
                        for (int j = 0; j < unit_list.Count; j++)
                        {
                            if (i != j || unit_list.Count == 1)
                            {
                                if (unit_list[i].team != unit_list[j].team && Crush(unit_list[i].x, unit_list[j].x, unit_list[i].range))//상대 유닛공격
                                {
                                    //unit_list[i].moving = false;
                                    if (interval == unit_list[i].atttack_speed)
                                    {
                                        unit_list[i].attack(unit_list[j]);
                                        attackstart();
                                        timer = true;

                                    }
                                    move_unit = false;
                                    break;
                                }
                                else if (unit_list[i].team == 1 && Crush(unit_list[i].x, enemycamp.x, unit_list[i].range))//상대 기지공격
                                {
                                    if (interval == unit_list[i].atttack_speed)
                                    {

                                        unit_list[i].attack(enemycamp);
                                        attackstart();
                                        timer = true;

                                    }
                                    if (enemycamp.health < 0)
                                    {
                                        end_game(1);
                                        break;
                                    }
                                    move_unit = false;
                                    break;
                                }
                                else if (unit_list[i].team == 2 && Crush(unit_list[i].x, mycamp.x + 100, unit_list[i].range))//우리팀 기지공격 받을때
                                {
                                    if (interval == unit_list[i].atttack_speed)
                                    {
                                        unit_list[i].attack(mycamp);
                                        attackstart();
                                        timer = true;
                                    }
                                    if (mycamp.health < 0)
                                    {
                                        end_game(1);
                                        break;
                                    }
                                    move_unit = false;
                                    break;
                                }
                            }
                        }
                        if (move_unit)
                        {
                            unit_list[i].move(true);
                        }
                        for (int c = 0; c < tower_list.Count(); c++)// 타워의 사정거리 내에 유닛 파악
                        {
                            if (tower_list[c].team != unit_list[i].team && CircleCrush(tower_list[c].x, tower_list[c].y, unit_list[i].x, unit_list[i].y, tower_list[0].range, unit_list[i].size_x + 3))
                            {
                                double angle = getAngle(tower_list[c].x, tower_list[0].y, unit_list[i].x, unit_list[i].y);
                                tower_list[c].arrowx = tower_list[c].x + (int)((float)Math.Cos(angle * Math.PI / 180) * 40);
                                tower_list[c].arrowy = tower_list[c].y + (int)((float)Math.Sin(angle * Math.PI / 180) * 40);

                            }
                        }
                        for (int c = 0; c < bullet_list.Count(); c++)//타워 총알 이동 및 유닛 충돌검사 
                        {
                            if (bullet_list[c].team != unit_list[i].team && CircleCrush(bullet_list[c].x, bullet_list[c].y, unit_list[i].x, unit_list[i].y, bullet_list[0].size, unit_list[i].size_x + 3))
                            {
                                unit_list[i].u_health -= 3;
                                //tower_list[c].bullet = false;
                                bullet_list.RemoveAt(c);
                                break;
                            }
                        }
                    }
                    for (int c = 0; c < bullet_list.Count(); c++)//타워 총알 이동 및 유닛 충돌검사 
                    {
                        bullet_list[c].move();
                        if (bullet_list[c].y > 450)
                        {
                            bullet_list.RemoveAt(c);
                            //tower_list[c].bullet = false;                                
                            break;
                        }
                    }
                    int count = unit_list.Count;
                    for (int i = 0; i < count; i++)//유닛 사망확인
                    {
                        if (unit_list[i].u_health <= 0)
                        {
                            if (unit_list[i].getteam() == 1)
                            {
                                enemycamp.goldup(unit_list[i].die());
                                unit_list.RemoveAt(i);
                                timer = false;
                                timer_attack1.Dispose();
                                a = 0;
                                stonex = 0;
                                i--;
                                count--;
                            }
                            else if (unit_list[i].getteam() == 2)
                            {
                                mycamp.goldup(unit_list[i].die());
                                unit_list.RemoveAt(i);
                                timer = false;
                                timer_attack1.Dispose();
                                a = 0;
                                stonex = 0;
                                i--;
                                count--;
                            }
                        }
                    }
                }
            ));
            Invalidate();
        }
        void timer_skill_Event(Object obj)
        {
            //타이머에서 수행할 작업을 아래에 작성
            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    Random random = new Random();
                    int bullet_x = random.Next(skill_x - 300, skill_x + 300);
                    bullet_list.Add(new bullet(1, bullet_x, 200, 90, 1));
                    //Rectangle(Scrollx - 300, 150, 600, 250)
                    skill_count++;
                    if (skill_count == 100)
                    {
                        skill_count = 0;
                        timer_skill.Dispose();
                    }
                }
            ));
        }
        int a = 0;
        public void Timer_attack1_Event(Object obj)
        {
            //타이머에서 수행할 작업을 아래에 작성
            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    a++;
                    stonex += 10;
                    e_stonex -= 10;
                    if (stonex == 100)
                        stonex = 0;
                    if (e_stonex == -100)
                        e_stonex = 0;
                    if (a == 4)
                        a = 0;
                }
                ));
            Invalidate();
        }
        private void Creat_Unit1(object sender, EventArgs e)
        {

            if (mycamp.gold > 10)
            {
                //  client.sendmsg("unit1");
                msg = "unit1";
                //생성시 x 좌표,팀,체력,공격력,방어력,사거리,생성시 필요골드량,공격속도,이동속도,x크기,y크기
                unit_list.Add(new unit(mycamp.x + 100, 1, 30, 5, 1, 50, 10, 25, 2, 30, 20, 40, true));
                mycamp.gold -= 10;
            }
            Invalidate();
        }

        private void Create_Unit2(object sender, EventArgs e) // 원거리
        {
            if (mycamp.gold > 30)
            {
                //client.sendmsg("unit2");
                msg = "unit2";
                unit_list.Add(new unit(mycamp.x + 100, 1, 30, 10, 1, 100, 30, 25, 2, 30, 20, 40, true));//생성시x좌표,팀,체력,공격력,방어력,사거리,생성시 필요골드량,공격속도,이동속도,x크기,y크기
                mycamp.gold -= 30;
            }
            Invalidate();
        }

        private void EnemyUnit_Create(object sender, EventArgs e)
        {
            unit_list.Add(new unit(enemycamp.x, 2, 30, 10, 1, 100, 10, 25, 2, 30, 20, 40, true));
            Invalidate();
        }

        private void tower_Btn_Click(object sender, EventArgs e)
        {
            // client.sendmsg("tower");
            msg = "tower";
            tower_list.Add(new tower(1, mycamp.x + 100));//팀1의 타워 생성
        }

        private void baseUpgrade(object sender, EventArgs e)
        {
            mycamp.upgrade();
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Scrollx = e.X;
            Invalidate();
        }

        private void skillBtn1_Click(object sender, EventArgs e)
        {

            if (skill1)
                skill1 = false;
            else
                skill1 = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (skill1)
            {
                skill_x = Scrollx;
                // client.sendmsg("s" + skill_x.ToString());
                msg = "s" + skill_x.ToString();
                System.Threading.TimerCallback callback3 = timer_skill_Event;
                timer_skill = new System.Threading.Timer(callback3, null, 0, 200);
                skill1 = false;
                mycamp.gold -= 200;
            }
        }

        public void recv_Enemy1_Create()
        {
            unit_list.Add(new unit(enemycamp.x, 2, 30, 5, 1, 50, 10, 25, 2, 30, 20, 40, true));
            Invalidate();
        }
        public void recv_Enemy2_Create()
        {
            unit_list.Add(new unit(enemycamp.x, 2, 30, 10, 1, 100, 30, 25, 2, 30, 20, 40, true));
            Invalidate();
        }
        public void recv_enemy_tower_Create()
        {
            tower_list.Add(new tower(2, enemycamp.x));
        }
       
        public void recv_skill_create()
        {
            // skill_x = Scrollx;
            System.Threading.TimerCallback callback5 = timer_skill_Event2;
            timer_skill2 = new System.Threading.Timer(callback5, null, 0, 200);
            Invalidate();
        }
        public void recv_tower_create()
        {
            tower_list.Add(new tower(2, enemycamp.x));//팀1의 타워 생성
        }
        public void recv_upgrade()
        {
            enemycamp.upgrade();

        }
        void timer_skill_Event2(Object obj)
        {
            //타이머에서 수행할 작업을 아래에 작성
            this.Invoke(new MethodInvoker(
                delegate ()
                {
                    Random random = new Random();
                    int bullet_x = random.Next(skill_x2 - 300, skill_x2 + 300);
                    bullet_list.Add(new bullet(2, bullet_x, 200, 90, 1));
                    //Rectangle(Scrollx - 300, 150, 600, 250)
                    skill_count2++;
                    if (skill_count2 == 100)
                    {
                        skill_count2 = 0;
                        timer_skill2.Dispose();
                    }

                }
            ));
        }

    }
}