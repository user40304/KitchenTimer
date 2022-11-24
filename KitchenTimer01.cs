using System;
using System.Threading.Tasks;
using System.Timers;

namespace KitchenTimer {
    class KitchenTimer01 {

        private static Timer tTimer;
        private static int count;
        private static string key = "0";

        private static State state_old;
        private static State state = State.Taiki;

        enum State {
            Taiki,
            Keisoku,
            Ichijiteishi,
            Beep,
        }

        static void Main(string[] args) {

            SetTimer();


            while (true) {
                state_old = state;

                switch (state) {
                    case State.Taiki:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                        if (key == "1") {
                            count++;
                        } else if (key == "2") {
                            if (count > 0) {
                                state = State.Keisoku;
                                tTimer.Start();
                            }
                        } else if (key == "3") {
                            count = 0;
                        }

                        WriteInfo();

                        break;

                    case State.Keisoku:
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;

                        if (key == "2") {
                            state = State.Ichijiteishi;
                            tTimer.Stop();
                        } else if (key == "3") {
                            state = State.Taiki;
                            tTimer.Stop();
                            count = 0;
                        }

                        WriteInfo();

                        break;

                    case State.Ichijiteishi:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;

                        if(key == "1") {
                            count++;
                        }else if (key == "2") {
                            state = State.Keisoku;
                            tTimer.Start();
                        } else if (key == "3") {
                            state = State.Taiki;
                            count = 0;
                        }

                        WriteInfo();

                        break;

                    case State.Beep:

                        if (key == "1" || key == "2" || key == "3") {
                            state = State.Taiki;

                            count = 0;
                        }

                        WriteInfo();

                        break;
                    default:
                        Console.WriteLine("エラー");
                        break;
                }

                key = "0";

                // stateの変化を検出
                if (state_old != state) {
                    Console.Clear();

                    continue;
                }

                // キー入力
                // key = Console.ReadLine();
                key = Console.ReadKey().KeyChar.ToString();

            }
        }

        private static void SetTimer() {
            tTimer = new Timer(1000);

            tTimer.Elapsed += TimerEvent;
            tTimer.Enabled = false;
        }

        private static async void TimerEvent(Object source, ElapsedEventArgs e) {
            count--;
            if (count <= 0) {
                state = State.Beep;

            }

            switch (state) {
                case State.Keisoku:
                    WriteInfo();

                    break;

                case State.Beep:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;

                    WriteInfo();
                    tTimer.Stop();

                    while (true) {
                        Console.Beep();

                        await Task.Delay(1500);

                        if (state != State.Beep) {
                            break;
                        }
                    }

                    break;

                default:
                    Console.WriteLine("エラー");
                    break;
            }
        }

        private static void WriteInfo() {
            Console.Clear();

            switch (state) {
                case State.Taiki:
                    Console.WriteLine("情報: 待機");

                    WriteTimer();

                    if (count > 0) {
                        Console.WriteLine("\n入力可能キー: 1 => +1, 2 => タイマースタート, 3 => リセット");
                    } else {
                        Console.WriteLine("\n入力可能キー: 1 => +1, 3 => リセット");
                    }

                    break;

                case State.Keisoku:
                    Console.WriteLine("情報: カウントダウン中");

                    WriteTimer();

                    Console.WriteLine("\n入力可能キー: 2 => タイマーストップ, 3 => リセット");

                    break;

                case State.Ichijiteishi:
                    Console.WriteLine("情報: 一時停止");

                    WriteTimer();

                    Console.WriteLine("\n入力可能キー: 1 => +1, 2 => タイマースタート, 3 => リセット");

                    break;

                case State.Beep:
                    Console.WriteLine("情報: 終了");

                    WriteTimer();

                    Console.WriteLine("\n入力可能キー: 1, 2, 3 => 待機画面に戻る");

                    break;
                default:
                    Console.WriteLine("エラー");
                    break;
            }

            //Console.Write("キー入力> ");

        }

        private static void WriteTimer() {
            if (count < 60) {
                Console.WriteLine("タイマー： " + count + "秒");
            } else {
                Console.WriteLine("タイマー： " + count / 60 + "分" + count % 60 + "秒");
            }
        }
    }
}
